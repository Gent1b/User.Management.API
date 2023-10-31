using User.Management.API.Models;
using User.Management.API.Repositories;

namespace User.Management.API.Services
{
    public class UserProfileService :IUserProfileService
    {
        private readonly IUserProfileRepository _userProfileRepository;

        public UserProfileService(IUserProfileRepository userProfileRepository)
        {
            _userProfileRepository = userProfileRepository;
        }

        public async Task<UserProfile> GetUserProfileAsync(string userId)
        {
            return await _userProfileRepository.GetUserProfileAsync(userId);
        }

        public async Task CreateUserProfileAsync(UserProfile userProfile)
        {
            await _userProfileRepository.CreateUserProfileAsync(userProfile);
        }

        public async Task UpdateUserProfileAsync(UserProfile userProfile)
        {
            await _userProfileRepository.UpdateUserProfileAsync(userProfile);
        }

        public async Task<UserProfile> GetUserProfileById(int id)
        {
            var userProfile = await _userProfileRepository.GetUserProfileById(id);
            return userProfile; // You're returning the UserProfile entity.
        }

        // Implement other methods as needed
    }
}
