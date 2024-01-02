using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MiniCrm.Core.Providers
{
    public sealed class InternalControllerFeatureProvider : ControllerFeatureProvider
    {
        protected override bool IsController(TypeInfo typeInfo)
        {
            return typeInfo.IsClass && !typeInfo.IsAbstract
                && !typeInfo.ContainsGenericParameters
                && !typeInfo.IsDefined(typeof(NonControllerAttribute))
                && (typeInfo.Name.EndsWith("Controller", StringComparison.OrdinalIgnoreCase)
                    || typeInfo.IsDefined(typeof(ControllerAttribute)));
        }
    }
}
