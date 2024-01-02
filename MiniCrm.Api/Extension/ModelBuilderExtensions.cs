using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MiniCrm.Api.Extension
{
    public static class ModelBuilderExtensions
    {
        public static void InitializeModuleModelBuilder(this ModelBuilder modelBuilder,
            Action<ModelBuilder> baseOnModelCreating,
            Assembly assembly,
            string? schema)
        {
            if (!string.IsNullOrWhiteSpace(schema))
            {
                modelBuilder.HasDefaultSchema(schema);
            }
            baseOnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(assembly);
        }
    }

}
