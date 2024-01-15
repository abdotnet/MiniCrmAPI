

using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MiniCrm.Api.Config;
using MiniCrm.Api.Extension;
using MiniCrm.Core.Data;
using MiniCrm.Core.Data.Persistence.Users;
using MiniCrm.Core.Interfaces;
using MiniCrm.Core.Interfaces.DbContext;
using MiniCrm.Core.Utility;
using MiniCrm.Infrastructure.Middleware;
using MiniCrm.Infrastructure.Services;
using Serilog;
using StackExchange.Redis;
using System.Configuration;
using System.Globalization;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console(formatProvider: CultureInfo.InvariantCulture)
    .CreateBootstrapLogger();

var corsPolicy = "CorsPolicy";

IConnectionMultiplexer? redisConnection = null;

try
{
    var builder = WebApplication.CreateBuilder(args);
    const string serviceName = ThisAssembly.Info.Product;
    //const string buildVersion = ThisAssembly.Info.Version;
    // builder.Host.ConfigureLogging(serviceName, buildVersion);
    // Add services to the container.

    builder.Services.AddControllers();
    builder.Services.AddModuleDbContext<UserDBContext, IUserDbContext>(builder.Configuration);
   // builder.Services.AddSharedInfrastructure(builder.Configuration, serviceName);
    
    builder.Services.AddScoped<IUserService, UserService>();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddSession();
    redisConnection = await ConnectionMultiplexerFactory.CreateConnection(builder.Configuration);
    builder.Services.AddRedisDependentServices(serviceName, redisConnection);
    //builder.Services.AddExceptionHandler<ExceptionHandlingMiddleware>((c,d) =>c.ExceptionHandler);
  //  builder.use
    var app = builder.Build();


    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
   // await DoMigration(app);

    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseRouting();
   

    app.UseStatusCodePages();
    // Add OData /$query middleware
    app.UseODataQueryRequest();
    app.UseODataBatching();

    app.UseCors(corsPolicy);
  
    app.UseAuthorization();

    app.UseMiddleware<ExceptionHandlingMiddleware>();
    app.MapControllers().RequireCors(corsPolicy);
    //app.MapReverseProxy();

    app.Run();

}
catch (Exception ex) when (ex is not HostAbortedException)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    if (redisConnection is not null)
    {
        await redisConnection.CloseAsync(true);
        await redisConnection.DisposeAsync();
    }
    await Log.CloseAndFlushAsync();
}

static async Task DoMigration(WebApplication host)
{
    await using var scope = host.Services.CreateAsyncScope();
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<UserDBContext>();
        await context.Database.MigrateAsync();
        await Seed.CreateSeedDataAsync(context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occured during migration");
    }
}