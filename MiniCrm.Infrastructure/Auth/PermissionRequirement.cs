using Microsoft.AspNetCore.Authorization;
using MiniCrm.Core.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniCrm.Infrastructure.Auth
{
    public class PermissionRequirement : IAuthorizationRequirement
    {
        public static string ClaimType => MiniCrmClaimsType.Permissions;
        public string Permission { get; }

        public PermissionRequirement(string permission)
        {
            if (string.IsNullOrEmpty(permission))
                throw new ArgumentException("permission is required.", nameof(permission));

            Permission = permission;
        }
    }
}
