using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MiniCrm.Core.Utility
{
    public class Constants
    {
        public const string UserDbSchema = "user";
    }
    public partial class ThisAssembly
    {
        [GeneratedCode("ThisAssembly.AssemblyInfo", "1.4.1")]
        [CompilerGenerated]
        public static partial class Info
        {
            public const string Company =
    """
MiniCrm.Api
""";
            public const string Configuration =
    """
Debug
""";
            public const string Copyright =
    $"""
2023 MiniCrm Plc.
""";
            public const string FileVersion =
    """
1.0.0.0
""";
            public const string InformationalVersion =
    """
1.0.0
""";
            public const string Product =
    """
MiniCrm.Api
""";
            public const string Title =
    """
MiniCrm.Api
""";
            public const string Version =
    """
1.0.0.0
""";
        }
    }

    public class MiniCrmClaimsType
    {
        public const string Permissions = "permissions";
    }
}
