using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace MiniCrm.Infrastructure.Auth
{
    public class PermissionAuthorizationPolicyProvider : DefaultAuthorizationPolicyProvider
    {

        public PermissionAuthorizationPolicyProvider(
            IOptions<AuthorizationOptions> options) : base(options) { }

        public override async Task<AuthorizationPolicy?> GetPolicyAsync(
            string policyName)
        {
            if (!policyName.StartsWith(HasPermissionAttribute.PolicyPrefix, StringComparison.OrdinalIgnoreCase))
                return await base.GetPolicyAsync(policyName);

            // Will extract the permissions from the string (Create, Update..)
            var permission = HasPermissionAttribute.GetPermissionFromPolicy(policyName);

            // Here we create the instance of our requirement
            var requirement = new PermissionRequirement(permission);

            // Now we use the builder to create a policy, adding our requirement
            return new AuthorizationPolicyBuilder()
                .AddRequirements(requirement).Build();
        }
    }
}
