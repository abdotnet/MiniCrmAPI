using Microsoft.EntityFrameworkCore;
using MiniCrm.Core.Data.Entities.Users;
using MiniCrm.Core.Interfaces.DbContext;
using MiniCrm.Core.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniCrm.Core.Data.Persistence.Users
{
    public sealed class UserDBContext : ModuleDbContext<UserDBContext>, IUserDbContext
    {
        public UserDBContext(DbContextOptions<UserDBContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Role> Roles { get; set; } = null!;
        public DbSet<Permission> Permissions { get; set; } = null!;
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }

        protected override string Schema => Constants.UserDbSchema;
    }
}
