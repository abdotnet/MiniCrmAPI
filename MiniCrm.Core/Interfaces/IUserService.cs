using MiniCrm.Core.Contracts.Users;

namespace MiniCrm.Core.Interfaces
{
    public interface IUserService
    {
        Task<int> Register(UserRequest userRequest, CancellationToken cancellationToken);
        Task<int> UpdateProfile(UserRequest userRequest, CancellationToken cancellationToken);
        Task<IEnumerable<UserResponse>> Get(CancellationToken cancellationToken);
        Task<UserResponse?> Get(string id, CancellationToken cancellationToken);
    }
}
