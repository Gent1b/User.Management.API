using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Authorize(Roles = "Admin")]
[Route("api/[controller]")]
[ApiController]
public class AdminController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;

    public AdminController(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
    }

    // API endpoint to list all users
    [HttpGet("users")]
    public async Task<IActionResult> ListUsers()
    {
        var users = await _userManager.Users.ToListAsync(); // You may need to adjust this based on your data access layer.
        return Ok(users);
    }

    // API endpoint to delete a user by ID
    [HttpDelete("delete-user/{id}")]
    public async Task<IActionResult> DeleteUser(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        var result = await _userManager.DeleteAsync(user);
        if (result.Succeeded)
        {
            return Ok("User deleted successfully");
        }
        else
        {
            return BadRequest("User deletion failed");
        }
    }
}
