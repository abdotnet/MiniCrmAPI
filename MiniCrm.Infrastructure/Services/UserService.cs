using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MiniCrm.Core.Contracts;
using MiniCrm.Core.Contracts.Users;
using MiniCrm.Core.Data.Entities.Users;
using MiniCrm.Core.Enums;
using MiniCrm.Core.Interfaces;
using MiniCrm.Core.Interfaces.DbContext;
using MiniCrm.Core.Utility;
using Quartz;

namespace MiniCrm.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IUserDbContext _userDbContext;
        public UserService(IUserDbContext userDbContext)
        {
            _userDbContext = userDbContext;
        }
        public async Task<int> Register(UserRequest userRequest, CancellationToken cancellationToken)
        {
            if (userRequest == null) throw new ArgumentNullException(nameof(userRequest));

            var isUserExist = await _userDbContext.Users.AnyAsync(c => (c.Email == userRequest.Email || c.MobileNumber == userRequest.MobileNumber), cancellationToken);

            if (isUserExist) throw new ObjectAlreadyExistsException(nameof(userRequest));

            Mapper mapper = MappingConfiguration.InitializeUserAutomapper();
            userRequest.IdentityId = Guid.NewGuid().ToString();
            var user = mapper.Map<User>(userRequest);
            user.IdentityId = userRequest.IdentityId;
            user.Salt = Guid.NewGuid().ToString();
            user.Password = Helpers.ToSha512(userRequest.Password + user.Salt);
            user.Status = StatusType.Approved;
            user.RoleType = RoleType.IsUser;

            _userDbContext.Users.Add(user);
            int response = await _userDbContext.SaveChangesAsync(cancellationToken);

            // Role 
            int enumValue = (short)user.RoleType;
            var role = await _userDbContext.Roles.FirstOrDefaultAsync(c => c.Id == enumValue, cancellationToken: cancellationToken);

            if (response > 0 && role is not null)
            {
                _userDbContext.UserRoles.Add(new UserRole()
                {
                    UserId = user.Id,
                    RoleId = role.Id
                });
                await _userDbContext.UserRoles.SingleOrDefaultAsync(cancellationToken: cancellationToken);
            }

            return response;
        }

        public async Task<IEnumerable<UserResponse>> Get(CancellationToken cancellationToken)
        {
            var users = await _userDbContext.Users.Select(c => new UserResponse()
            {
                Email = c.Email,
                EmailVerified = c.EmailVerified,
                FirstName = c.FirstName,
                LastName = c.LastName,
                MobileNumber = c.MobileNumber,
                IdentityId = c.IdentityId,
                Password = c.Password,
                Salt = c.Salt,
                Gender = c.Gender,
                Status = c.Status,
                State = c.State,
                DateOfBirth = c.DateOfBirth,
                RoleType = c.RoleType,
                Country = c.Country,
                MobileNumberVerified = c.MobileNumberVerified,
                Address = c.Address,
                Id = c.Id,
            }).ToListAsync(cancellationToken);

            return users;
        }
        public async Task<UserResponse?> Get(string id, CancellationToken cancellationToken)
        {
            var user = await _userDbContext.Users.Where(c => c.IdentityId == id).Select(c => new UserResponse()
            {
                Email = c.Email,
                EmailVerified = c.EmailVerified,
                FirstName = c.FirstName,
                LastName = c.LastName,
                MobileNumber = c.MobileNumber,
                IdentityId = c.IdentityId,
                Password = c.Password,
                Salt = c.Salt,
                Gender = c.Gender,
                Status = c.Status,
                State = c.State,
                DateOfBirth = c.DateOfBirth,
                RoleType = c.RoleType,
                Country = c.Country,
                MobileNumberVerified = c.MobileNumberVerified,
                Address = c.Address,
                Id = c.Id,
            }).FirstOrDefaultAsync(cancellationToken);

            return user;
        }

        public async Task<LoginResponse> Login(LoginRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            return await Task.FromResult( new LoginResponse());
        }

        public async Task ChangePassword()
        {
            await Task.FromResult(0);
            return;
        }

        public async Task ForgetPassword()
        {
            await Task.FromResult(0);
            return;
        }
        public async Task<int> UpdateProfile(UserRequest userRequest, CancellationToken cancellationToken)
        {
            if (userRequest == null) throw new ArgumentNullException(nameof(userRequest));

            Mapper mapper = MappingConfiguration.InitializeUserAutomapper();

            var user_ = await _userDbContext.Users.FirstOrDefaultAsync(c => c.IdentityId == userRequest.IdentityId, cancellationToken);

            if (user_ is null) throw new ArgumentNullException(nameof(user_));

            user_.DateOfBirth = userRequest.DateOfBirth;
            user_.Address = userRequest.Address;
            _userDbContext.Users.Update(user_);

            int response = await _userDbContext.SaveChangesAsync(cancellationToken);
            return response;

        }

    }
}
