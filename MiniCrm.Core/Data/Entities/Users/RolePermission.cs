using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniCrm.Core.Data.Entities.Users
{
    public class RolePermission
    {
        public long Id { get; set; }
        public long RoleId { get; set; }
        public long PermissionId { get; set; }
    }
}
