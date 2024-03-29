﻿using MiniCrm.Core.Data.Entities;
using MiniCrm.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniCrm.Core.Data.Entities.Users
{
    public class User : BaseEntity
    {
        public long Id { get; set; }
        public required string IdentityId { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public string? MiddleName { get; set; }
        public required string Salt { get; set; }
        public required string Password { get; set; }
        public string? Address { get; set; }
        public required string Email { get; set; }
        public required bool EmailVerified { get; set; }
        public required string MobileNumber { get; set; }
        public bool? MobileNumberVerified { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public GenderType? Gender { get; set; }
        public string? Country { get; set; }
        public string? State { get; set; }
        public RoleType RoleType { get; set; }
        public StatusType Status { get; set; }
        public UserRole UserRole { get; set; }
        public User()
        {
            UserRole = new UserRole();
           
        }
    }
}
