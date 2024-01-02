using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniCrm.Core.Data.Entities.Users
{
    public class Permission
    {
        public long Id { get; set; }
        public required string Name { get; set; }
    }
}
