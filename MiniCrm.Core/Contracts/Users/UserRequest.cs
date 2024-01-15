using MiniCrm.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniCrm.Core.Contracts.Users
{
    public class UserRequest
    {
        public UserRequest()
        {
            MobileNumberVerified = false;
            EmailVerified = false;
        }
        public required string IdentityId { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public string? Address { get; set; }
        [EmailAddress]
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required bool EmailVerified { get; set; }
        public required string MobileNumber { get; set; }
        public bool? MobileNumberVerified { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public GenderType? Gender { get; set; }
        public string? Country { get; set; }
        public string? State { get; set; }
        public RoleType RoleType { get; set; }
        public StatusType Status { get; set; }

    }
}
