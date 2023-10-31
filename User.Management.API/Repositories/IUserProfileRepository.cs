using User.Management.API.Models;

namespace User.Management.API.Repositories
{
    public interface IUserProfileRepository
    {
        Task<UserProfile> GetUserProfileAsync(string userId);
        Task CreateUserProfileAsync(UserProfile userProfile);
        Task UpdateUserProfileAsync(UserProfile userProfile);
        Task<UserProfile> GetUserProfileById(int id);

    }
}
