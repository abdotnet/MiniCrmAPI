using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiniCrm.Core.Data.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniCrm.Core.Data.Persistence.Users.Configuration
{
    public class UserConfiguration : BaseEntityConfiguration<User>
    {
        public override void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasQueryFilter(t => !t.Deleted);
            builder.HasKey(m => m.Id);
            builder.HasIndex(m => m.Email)
                .IsUnique();
            builder.Property(m => m.Email)
             .HasMaxLength(128);
            builder.HasIndex(m => m.MobileNumber)
                .IsUnique();
            builder.Property(m => m.MobileNumber)
              .HasMaxLength(32);
            builder.Property(m => m.IdentityId)
                .HasMaxLength(64);
            builder.Property(m => m.FirstName)
                .HasMaxLength(64);
            builder.Property(m => m.LastName)
                .HasMaxLength(64);
            builder.Property(m => m.MiddleName)
                .HasMaxLength(64);
        }
    }
}
