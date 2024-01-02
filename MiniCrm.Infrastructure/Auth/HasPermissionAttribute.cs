using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniCrm.Infrastructure.Auth
{
    /// <summary>
    /// This attribute can be applied in the same places as the [Authorize] would go
    /// This will only allow users which has a role containing the enum Permission passed in
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = false)]
    public sealed class HasPermissionAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// This creates an HasPermission attribute with a Permission enum members
        /// </summary>
        /// <param name="permission"></param>
        public HasPermissionAttribute(string permission)
        {
            if (string.IsNullOrEmpty(permission))
                throw new ArgumentException("permission is required.", nameof(permission));

            Permission = permission;
            Policy = PolicyPrefix + Separator + permission;
        }

        public string Permission { get; }

        public  const string PolicyPrefix = "PERMISSION_";
        private const string Separator = "$$";

        public static string GetPermissionFromPolicy(string policyName)
        {
            return policyName[(PolicyPrefix.Length + 2)..];
        }
    }
}
