using Microsoft.AspNetCore.Authorization;
using MiniCrm.Core.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniCrm.Infrastructure.Auth
{
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            PermissionRequirement requirement)
        {
            var user = context.User;
            if ((!user.Identity?.IsAuthenticated) ?? true)
            {
                context.Fail();
                return Task.CompletedTask;
            }

            var userPermissions = user?.Claims
                .Where(c => string.Equals(c.Type, MiniCrmClaimsType.Permissions,
                    StringComparison.OrdinalIgnoreCase))
                .Select(c => c.Value);

            if (!userPermissions?.Any() ?? true)
            {
                context.Fail();
                return Task.CompletedTask;
            }

            var hasRequiredPermissions = userPermissions?.Contains(requirement.Permission);
            // identity has all required permissions
            if (hasRequiredPermissions == true)
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            // identity does not have any of the required permissions
            context.Fail();
            return Task.CompletedTask;
        }
    }
}
