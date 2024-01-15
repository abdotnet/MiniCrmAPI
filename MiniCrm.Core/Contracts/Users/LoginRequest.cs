using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniCrm.Core.Contracts.Users
{
    public class LoginRequest
    {
        [EmailAddress]
        public required string EmailAddress { get; set; }
        public required string Password { get; set; }
    }
}
