using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MiniCrm.Api.Filters;
public sealed class SwaggerODataControllerDocumentFilter : IDocumentFilter
{
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        // remove controller
        foreach (var apiDescription in context.ApiDescriptions)
        {
            if (apiDescription.ActionDescriptor is ControllerActionDescriptor descriptor
                && descriptor.ControllerName == "Metadata")
            {
                swaggerDoc.Paths.Remove($"/{apiDescription.RelativePath}");
            }
        }

        // remove schemas
        foreach (var (key, _) in swaggerDoc.Components.Schemas)
        {
            if (key.Contains("Edm", StringComparison.InvariantCultureIgnoreCase) || key.StartsWith("OData", StringComparison.InvariantCultureIgnoreCase))
            {
                swaggerDoc.Components.Schemas.Remove(key);
            }
        }
    }
}
