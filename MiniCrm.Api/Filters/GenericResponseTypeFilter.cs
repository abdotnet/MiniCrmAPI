using Microsoft.OpenApi.Models;
using MiniCrm.Core.Contracts;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MiniCrm.Api.Filters;


public class GenericResponseTypeFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        operation.Responses ??= new OpenApiResponses();

        // Assuming your CoreCoopODataResponse type is used as a return type
        if (context.MethodInfo.ReturnType == typeof(MiniCrmODataResponse))
        {
            // Add or update the response type
            operation.Responses["200"] = new OpenApiResponse
            {
                Description = "OK",
                Content = new Dictionary<string, OpenApiMediaType>
                {
                    ["application/json"] = new OpenApiMediaType
                    {
                        Schema = context.SchemaGenerator.GenerateSchema(typeof(MiniCrmODataResponse), context.SchemaRepository)
                    }
                }
            };
        }
    }
}
