using Microsoft.EntityFrameworkCore;
using MiniCrm.Core.Data.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniCrm.Core.Interfaces.DbContext
{
    public interface IUserDbContext : IWriteDbContext
    {
        DbSet<User> Users { get; set; }
        DbSet<Role> Roles { get; set; }
        DbSet<Permission> Permissions { get; set; }
        DbSet<UserRole> UserRoles { get; set; }
        DbSet<RolePermission> RolePermissions { get; set; }
    }
}
