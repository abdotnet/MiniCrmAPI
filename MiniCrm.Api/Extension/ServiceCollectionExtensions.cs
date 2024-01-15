using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.OData.Batch;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OData.Edm;
using MiniCrm.Api.Filters;
using MiniCrm.Core.Data;
using MiniCrm.Core.Data.Persistence;
using MiniCrm.Core.Interfaces;
using MiniCrm.Core.Interfaces.Integrations.sms;
using MiniCrm.Core.Models;
using MiniCrm.Core.Providers;
using MiniCrm.Infrastructure.Auth;
using MiniCrm.Infrastructure.InfraHelpers;
using MiniCrm.Infrastructure.Integrations.Email;
using MiniCrm.Infrastructure.Integrations.Sms;
using MiniCrm.Infrastructure.Services;
using PiiTypes;
using Quartz;
using Quartz.AspNetCore;
using StackExchange.Redis;
using System.Configuration;
using System.Text;
using System.Text.Json.Serialization;

namespace MiniCrm.Api.Extension
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSharedInfrastructure(this IServiceCollection services,
      IConfiguration configuration, string serviceName
  )
        {
            //string buildVersion,
           // string[] activitySources, string[] metricSources
            services
                //.AddAllAuth(configuration)
                .AddAuthorization()
                // Register our custom Authorization handler
                .AddSingleton<IAuthorizationHandler, PermissionHandler>()
                 .AddSingleton<ISmsService, SmsService>()
                  .AddSingleton<IEmailService, EmailService>()
                  .AddSingleton<IUserService, UserService>()
                // Overrides the DefaultAuthorizationPolicyProvider with our own
                .AddSingleton<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>()
                .AddSingleton(FrontEndUrlHelper.Create(configuration.GetValue<string>("CLIENT_URL")
                    ?? throw new ConfigurationErrorsException("CLIENT_URL is required")))
                .AddQuartz()
                .AddControllers()
                .ConfigureApplicationPartManager(manager =>
                    manager.FeatureProviders.Add(new InternalControllerFeatureProvider()));

            services.Configure<SmsSettings>(configuration.GetSection("SmsConfig"));
            return services;
        }

        public static IServiceCollection AddModuleDbContext<TDbContext, TIDbContext>(
    this IServiceCollection services,
    IConfiguration configuration,
    string sectionName = "Default")
    where TDbContext : ModuleDbContext<TDbContext>, TIDbContext
        {
            services.AddModuleDbContext<TDbContext>(configuration, sectionName)
                .AddScoped(typeof(TIDbContext), provider => provider.GetRequiredService<TDbContext>());

            return services;
        }


        public static IServiceCollection AddSwaggerGen(this IServiceCollection services,
  IConfiguration configuration)
        {
            services.AddSwaggerGen(c =>
            {
                c.AddBearerAuth();
                c.OperationFilter<SecurityRequirementOperationFilter>();
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                c.OperationFilter<ODataEnableQueryOperationFilter>();
                c.OperationFilter<GenericResponseTypeFilter>();
                c.DocumentFilter<SwaggerODataControllerDocumentFilter>();
                c.EnableAnnotations();
            });
            return services;
        }

        public static IServiceCollection AddRedisDependentServices(this IServiceCollection services,
        string applicationName, IConnectionMultiplexer? redisConnection)
        {
            if (redisConnection is not null)
            {
                services.AddStackExchangeRedisCache(options =>
                    options.ConnectionMultiplexerFactory = () => Task.FromResult(redisConnection))
                    .AddDataProtection()
                    .SetApplicationName(applicationName)
                    .PersistKeysToStackExchangeRedis(redisConnection);
            }
            else
            {
                services.AddDistributedMemoryCache()
                    .AddDataProtection()
                    .SetApplicationName(applicationName)
                    .PersistKeysToFileSystem(new DirectoryInfo(Path.Combine(Path.GetTempPath(), "CoreCoop_data_protection_keys")));
            }

            return services;
        }

    
        private static IServiceCollection AddModuleDbContext<TDbContext>(this IServiceCollection services,
        IConfiguration configuration,
        string sectionName = "Default") where TDbContext : ModuleDbContext<TDbContext>
        {
            var connectionString = configuration.GetConnectionString(sectionName);
            return services.AddDbContext<TDbContext>((provider, options) =>
            {
                EnableDebugLoggingInDev(provider, options);
                options.UseSqlServer(connectionString);
            });
        }

        public static IServiceCollection AddODataEndpoints(this IServiceCollection services, string route, IEdmModel edmModel)
        {
            services.AddControllers(options =>
            {
                options.Filters.Add(typeof(ResponseFormatter));
            })
            .AddJsonOptions(opt =>
            {
                opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            })
            .AddOData(options => options.EnableQueryFeatures()
                .AddRouteComponents(
                    routePrefix: $"odata/{route}",
                    model: edmModel, new DefaultODataBatchHandler()
                )
            );


            return services;
        }

        private static IServiceCollection AddAllAuth(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection(nameof(JwtSettings)).Get<JwtSettings>()
                ?? throw new ConfigurationErrorsException("jwtSettings section is required");

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opts =>
                {
                    opts.SaveToken = true;
                    opts.RequireHttpsMetadata = false;
                    opts.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.AccessTokenSecret)),
                        ValidIssuer = jwtSettings.Issuer,
                        ValidAudience = jwtSettings.Audience,
                        ClockSkew = TimeSpan.Zero,
                        ValidAlgorithms = new[] { "HS256" },
                        PropertyBag = new Dictionary<string, object> {
                        { nameof(jwtSettings.AccessTokenExpirationMinutes), jwtSettings.AccessTokenExpirationMinutes },
                        { nameof(jwtSettings.RefreshTokenExpirationMinutes), jwtSettings.RefreshTokenExpirationMinutes },
                        }
                    };
                });
            return services;
        }


        public static IServiceCollection AddQuartzWithScheduler(this IServiceCollection services)
        {
            return services.AddQuartz()
                .AddQuartzServer(options =>
                {
                    options.WaitForJobsToComplete = true;
                    options.AwaitApplicationStarted = true;
                })
                .AddSingleton(provider => provider.GetRequiredService<ISchedulerFactory>()
                    .GetScheduler()
                    .GetAwaiter()
                    .GetResult()); 
        }

        private static IServiceCollection AddPiiAesEncoder(this IServiceCollection services, IConfiguration configuration)
        {
            var aesKey = configuration.GetValue<string>("Pii:AesKey") ?? throw new ConfigurationErrorsException("Pii:AesKey is required");
            var aesEncoder = new PiiAsAes128(aesKey);
            PiiAesStringConverter.SetEncoder(aesEncoder);
            return services.AddSingleton<IPiiEncoder>(aesEncoder);
        }
        private static void EnableDebugLoggingInDev(IServiceProvider provider, DbContextOptionsBuilder optionsBuilder)
        {
            var env = provider.GetRequiredService<IHostEnvironment>();
            if (env.IsDevelopment())
            {
                optionsBuilder.UseLoggerFactory(provider.GetRequiredService<ILoggerFactory>())
                    .EnableSensitiveDataLogging();
            }
        }

    }


}
