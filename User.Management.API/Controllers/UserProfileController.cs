using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using User.Management.API.Models;
using User.Management.API.Services;

namespace User.Management.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        private readonly IUserProfileService _userProfileService;

        public UserProfileController(IUserProfileService userProfileService)
        {
            _userProfileService = userProfileService;
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserProfile(string userId)
        {
            var userProfile = await _userProfileService.GetUserProfileAsync(userId);
            if (userProfile == null)
            {
                return NotFound(new ApiResponse
                {
                    IsSuccess = false,
                    Message = "User profile not found",
                    StatusCode = StatusCodes.Status404NotFound,
                    Response = null
                });
            }

            return Ok(new ApiResponse
            {
                IsSuccess = true,
                Message = "User profile retrieved successfully",
                StatusCode = StatusCodes.Status200OK,
                Response = userProfile
            });
        }

        [HttpGet("profile/{id}")]
        public async Task<IActionResult> GetUserProfileById(int id)
        {
            var userProfile = await _userProfileService.GetUserProfileById(id);
            if (userProfile == null)
            {
                return NotFound(new ApiResponse
                {
                    IsSuccess = false,
                    Message = "User profile not found",
                    StatusCode = StatusCodes.Status404NotFound,
                    Response = null
                });
            }

            return Ok(new ApiResponse
            {
                IsSuccess = true,
                Message = "User profile retrieved successfully",
                StatusCode = StatusCodes.Status200OK,
                Response = userProfile
            });
        }


        [HttpPut]
        public async Task<IActionResult> UpdateUserProfile([FromBody] UserProfile userProfile)
        {
            await _userProfileService.UpdateUserProfileAsync(userProfile);
            return Ok(new ApiResponse
            {
                IsSuccess = true,
                Message = "User profile updated successfully",
                StatusCode = StatusCodes.Status200OK,
                Response = null
            });
        }

        // Implement other actions as needed
    }
}
