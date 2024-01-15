using MiniCrm.Core.Data.Entities.Users;
using MiniCrm.Core.Data.Persistence.Users;
using MiniCrm.Core.Enums;
using MiniCrm.Core.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniCrm.Core.Data
{
    public class Seed
    {
        public static async Task<int> CreateSeedDataAsync(UserDBContext context)
        {
            var anyRoles = context.Roles.Any();

            if (!anyRoles)
            {
                var roles = GetDefaultRoles();

                foreach (var role in roles)
                {
                    context.Roles.Add(role);
                }
            }

            var anyPermission = context.Permissions.Any();

            if (!anyPermission)
            {
                var permissions = GetDefaultPermission();

                foreach (var permission in permissions)
                {
                    context.Permissions.Add(permission);
                }
            }

            var anyUser = context.Users.Any();

            if (!anyUser)
            {
                string salt = Guid.NewGuid().ToString();

                User user = new()
                {
                    Email = "minicrm@gmail.com",
                    EmailVerified = false,
                    FirstName = "Super Admin",
                    IdentityId = Guid.NewGuid().ToString(),
                    LastName = "Omolaja",
                    MobileNumber = "2348130230146",
                    Salt = salt,
                    Password = Helpers.ToSha512(Constants.DefaultPassword + salt),
                    Address = "Lagos,Nigeria",
                    Gender = GenderType.Male,
                    DateOfBirth = DateTime.Now.AddYears(30),
                    Country = "NG",
                    Status = StatusType.Approved,
                    RoleType = RoleType.IsSuperAdmin,
                    State = "LG",
                   
                };
                user.UserRole = new UserRole()
                {
                    UserId = user.Id,
                    RoleId = 3,
                };
                context.Users.Add(user);

            }


            int resp = context.SaveChanges();

            static List<Role> GetDefaultRoles()
            {
                return new List<Role>()
                {
                     new Role()
                     {
                          IsSystemRole = false,
                           Name ="User"
                     },
                      new Role()
                     {
                          IsSystemRole = false,
                           Name ="Admin"
                     },
                         new Role()
                     {
                          IsSystemRole = false,
                           Name ="SuperAdmin"
                     },
                            new Role()
                     {
                          IsSystemRole = true,
                           Name ="System"
                     }
                };
            }

            static List<Permission> GetDefaultPermission()
            {
                return new List<Permission>()
                {
                     new Permission()
                     {
                           Name ="CanSaveUser"
                     },
                      new Permission()
                     {
                           Name ="CanGetUser"
                     },
                         new Permission()
                     {
                           Name ="CanUpdateUser"
                     },
                            new Permission()
                     {
                           Name ="CanDeleteUser"
                     }
                };
            }

            return await Task.FromResult(resp);

        }
    }
}
