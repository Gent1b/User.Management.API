using Microsoft.EntityFrameworkCore;
using User.Management.API.Models;

namespace User.Management.API.Repositories
{
    public class UserProfileRepository : IUserProfileRepository
    {
        private readonly ApplicationDbContext _context;

        public UserProfileRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UserProfile> GetUserProfileAsync(string userId)
        {
            return await _context.UserProfiles.FirstOrDefaultAsync(up => up.AspNetUserId == userId);
        }

        public async Task CreateUserProfileAsync(UserProfile userProfile)
        {
            _context.UserProfiles.Add(userProfile);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUserProfileAsync(UserProfile userProfile)
        {
            _context.UserProfiles.Update(userProfile);
            await _context.SaveChangesAsync();
        }
        public async Task<UserProfile> GetUserProfileById(int id)
        {
            return await _context.UserProfiles.FirstOrDefaultAsync(u => u.Id == id);
        }
    }
}
