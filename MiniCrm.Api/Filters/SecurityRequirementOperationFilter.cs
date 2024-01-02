using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MiniCrm.Api.Filters
{
    public sealed class SecurityRequirementOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            // Authentication schemes is mapped to scope
            var classRequiredScopes = context.MethodInfo.DeclaringType?
                .GetCustomAttributes(true)
                .OfType<AuthorizeAttribute>()
                .Select(attr => attr.AuthenticationSchemes)
                .Distinct()
                .ToArray();
            var methodRequiredScopes = context.MethodInfo
                .GetCustomAttributes(true)
                .OfType<AuthorizeAttribute>()
                .Select(attr => attr.AuthenticationSchemes)
                .Distinct()
                .ToArray();

            var requireAuth = false;
            var id = string.Empty;
            if ((classRequiredScopes?.Contains(JwtBearerDefaults.AuthenticationScheme) ?? false)
                || (methodRequiredScopes?.Contains(JwtBearerDefaults.AuthenticationScheme) ?? false))
            {
                requireAuth = true;
                id = JwtBearerDefaults.AuthenticationScheme;
            }
            else if ((classRequiredScopes?.Contains(OpenIdConnectDefaults.AuthenticationScheme) ?? false)
                || (methodRequiredScopes?.Contains(OpenIdConnectDefaults.AuthenticationScheme) ?? false))
            {
                requireAuth = true;
                id = OpenIdConnectDefaults.AuthenticationScheme;
            }

            if (requireAuth && !string.IsNullOrEmpty(id))
            {
                operation.Responses.Add("401", new OpenApiResponse { Description = "Unauthorized" });
                operation.Security = new List<OpenApiSecurityRequirement>
            {
                new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = id, }
                        },
                        Array.Empty<string>()
                    }
                }
            };
            }
        }
    }
}
