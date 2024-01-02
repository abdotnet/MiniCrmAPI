using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using MiniCrm.Core.Extension;
using MiniCrm.Core.Utility;
using NetUlid;

namespace MiniCrm.Core.Data
{
    public abstract class ModuleDbContext<TDbContext> : DbContext where TDbContext : DbContext
    {
        protected abstract string Schema { get; }

        /// <inheritdoc />
        protected ModuleDbContext(DbContextOptions<TDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.InitializeModuleModelBuilder(base.OnModelCreating, GetType().Assembly, Schema);
            // add other custom configuration after this line
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            var sqlServerBuilder = new SqlServerDbContextOptionsBuilder(optionsBuilder);
            sqlServerBuilder.MigrationsAssembly(typeof(TDbContext).Assembly.FullName);
            sqlServerBuilder.MigrationsHistoryTable("__EFMigrationsHistory", Schema);
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder
                .Properties<Ulid>()
                .HaveConversion<UlidStringConverter>();
        }

        public IEnumerable<T> ExecuteStoredProcedure<T>(string storedProcedureName, params SqlParameter[] parameters) where T : class
        {
            string formattedStoredProcedureName = $"EXEC {storedProcedureName} {string.Join(", ", parameters.Select(p => p.ParameterName))}";
            // Execute the stored procedure
            return Set<T>().FromSqlRaw(formattedStoredProcedureName, parameters).ToList();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return base.SaveChangesAsync(true, cancellationToken);
        }
    }
}