using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniCrm.Core.Interfaces
{
    public interface IFrontEndUrlHelper
    {
        string Link(string path, Dictionary<string, string?> queryParameters);
    }
}
