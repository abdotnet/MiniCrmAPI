using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NetUlid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniCrm.Core.Utility
{
    public class UlidStringConverter : ValueConverter<Ulid, string>
    {
        public UlidStringConverter()
            : base(
                v => v.ToString(),
                v => Ulid.Parse(v))
        {
        }
    }

}
