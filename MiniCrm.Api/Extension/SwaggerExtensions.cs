using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MiniCrm.Api.Extension
{
    public static class SwaggerExtensions
    {
        private sealed record OAuthScope(string Key, string Value);

        private sealed record SwaggerAuthOptions(Uri AuthUrl, Uri TokenUrl,
       string ClientId, IEnumerable<OAuthScope>? Scopes = null)
        {
            public Dictionary<string, string> GetParsedScopes()
            {
                return Scopes is null
                    ? new Dictionary<string, string>()
                    : Scopes.DistinctBy(s => s.Key).ToDictionary(s => s.Key, s => s.Value);
            }
        }

        private sealed record SwaggerAuthConfig(
            string AuthenticationScheme, string Description,
            Uri AuthUrl, Uri TokenUrl, IDictionary<string, string> Scopes);

        public static void UseSwaggerUiWithPkce(this WebApplication app)
        {
            app.UseSwaggerUI(options =>
            {
                options.OAuthClientId(app.Configuration.GetValue<string>("Keycloak:SwaggerAuthOptions:ClientId"));
                options.OAuthAppName("CoreCoop by Africa Prudential");
                options.OAuthClientSecret("");
                options.OAuthScopes("openid", "email", "profile");
                options.OAuthScopeSeparator(" ");
                options.OAuthUsePkce();
            });
        }

        public static void AddBearerAuth(this SwaggerGenOptions options)
        {
            options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer 12345abcdef')",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = JwtBearerDefaults.AuthenticationScheme
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = JwtBearerDefaults.AuthenticationScheme
                    }
                },
                Array.Empty<string>()
            }
        });
        }

    }
}
