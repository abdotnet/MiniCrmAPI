using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Extensions;
using Microsoft.AspNetCore.OData.Routing;
using Microsoft.OData.Edm;
using MiniCrm.Core.Contracts;

namespace MiniCrm.Api.Filters
{
    public sealed class ResponseFormatter : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception is not null || !(
                context.HttpContext.Response.StatusCode == StatusCodes.Status200OK ||
                context.HttpContext.Response.StatusCode == StatusCodes.Status201Created))
            {
                return;
            }

            var odataFeature = context.HttpContext.ODataFeature();
            var contextName = odataFeature?.Path?.GetEdmType()?.AsElementType().FullTypeName();

            if (context.Result is ObjectResult objectResult && objectResult.Value != null && objectResult?.Value is IEnumerable<object> data)
            {
                context.Result = new JsonResult(ApiResponse<object>.GetResponse(objectResult.Value, data.Count().ToString(), contextName));
            }
        }
    }
}
