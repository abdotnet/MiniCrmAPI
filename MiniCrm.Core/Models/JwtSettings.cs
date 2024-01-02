using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniCrm.Core.Models
{
    public class JwtSettings
    {
        public string AccessTokenSecret { get; set; } = null!;
        public string RefreshTokenSecret { get; set; } = null!;
        public int AccessTokenExpirationMinutes { get; set; }
        public int RefreshTokenExpirationMinutes { get; set; }
        public string Issuer { get; set; } = null!;
        public string Audience { get; set; } = null!;
    }
}
