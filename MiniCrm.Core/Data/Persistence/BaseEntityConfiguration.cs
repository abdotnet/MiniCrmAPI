using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiniCrm.Core.Data.Entities;

namespace MiniCrm.Core.Data.Persistence;
public abstract class BaseEntityConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
    where TEntity : BaseEntity
{
    public virtual void Configure(EntityTypeBuilder<TEntity> entity)
    {
        entity.HasQueryFilter(t => !t.Deleted);
    }
}

