using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniCrm.Core.Data.Entities.Users
{
    public class UserRole
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public long RoleId { get; set; }
    }
}
