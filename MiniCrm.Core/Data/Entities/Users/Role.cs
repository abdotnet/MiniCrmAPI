using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniCrm.Core.Data.Entities.Users
{
    public class Role : BaseEntity
    {
        public long Id { get; set; }
        public required string Name { get; set; }
        public bool IsSystemRole { get; set; }
        public Collection<Permission> Permissions { get; set; }
        public Role()
        {
            Permissions = new Collection<Permission>();
        }
    }
}
