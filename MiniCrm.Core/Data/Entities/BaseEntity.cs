using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniCrm.Core.Data.Entities
{
    public class BaseEntity
    {
        public BaseEntity()
        {
            CreatedAt = DateTimeOffset.Now.UtcDateTime;
            Deleted = false;
        }
        public DateTimeOffset? DeletedAt { get; set; }
        public bool Deleted { get; set; }
        public DateTimeOffset? DeletedById { get; set; }
        public string? CreatedByID { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public string? LastUpdatedById { get; set; }
        public DateTimeOffset? LastUpdatedAt { get; set; }
    }
}
