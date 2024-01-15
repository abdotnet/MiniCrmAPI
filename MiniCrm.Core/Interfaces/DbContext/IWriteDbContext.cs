using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniCrm.Core.Interfaces.DbContext
{
    public interface IWriteDbContext
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
