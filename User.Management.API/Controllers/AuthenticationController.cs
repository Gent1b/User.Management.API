using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using User.Management.API.Models;
using User.Management.API.Models.Authentication.Login;
using User.Management.API.Models.Authentication.SignUp;
using User.Management.API.Services;
using User.Management.Service.Models;
using User.Management.Service.Services;

namespace User.Management.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, IEmailService emailService, IAuthenticationService authenticationService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _emailService = emailService;
            _authenticationService = authenticationService;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterUser registerUser, string role)
        {
            var result = await _authenticationService.RegisterUser(registerUser, role);

            if (result.IsSuccess)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpGet]
        public async Task<IActionResult> TestEmail()
        {
            var message = new Message(new string[] { "gentitba50@gmail.com" }, "Test", "Jaumin");

            for (int i = 0; i < 20; i++)
            {
                _emailService.SendEmail(message);
                await Task.Delay(5000); // Delay for 5 seconds (5000 milliseconds) before sending the next email.
            }

            return Ok(new ApiResponse
            {
                IsSuccess = true,
                StatusCode = StatusCodes.Status201Created,
                Message = "Email Created Successfully"
            });
        }

        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var result = await _userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                {
                    return Ok(new ApiResponse
                    {
                        IsSuccess = true,
                        Message = "Email Verified Successfully",
                        StatusCode = StatusCodes.Status200OK,
                        Response = "Email Verified Successfully"
                    });
                }
            }
            return BadRequest(new ApiResponse
            {
                IsSuccess = false,
                Message = "This User Doesn't exist!",
                StatusCode = StatusCodes.Status500InternalServerError,
                Response = "This User Doesn't exist!"
            });
        }


        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            var loginResult = await _authenticationService.Login(loginModel);

            if (loginResult.IsSuccess)
            {
                return Ok(loginResult);
            }
            else
            {
                return Unauthorized(loginResult);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("forgot-password")]
        public async Task<IActionResult> ForgotPassword([Required] string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var forgotPasswordLink = Url.Action(nameof(ResetPassword), "Authentication", new { token, email = user.Email }, Request.Scheme);
                var result = await _authenticationService.ForgotPassword(email, forgotPasswordLink);

                if (result.IsSuccess)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }
            return BadRequest(new ApiResponse
            {
                IsSuccess = false,
                StatusCode = StatusCodes.Status400BadRequest,
                Message = "User not found.",
                Response = "User not found."
            });
        }



        [HttpGet("reset-password")]
        public async Task<IActionResult> ResetPassword(string token, string email)
        {
            var model = new ResetPassword { Token = token, Email = email };
            return Ok(new ApiResponse
            {
                IsSuccess = true,
                StatusCode = StatusCodes.Status200OK,
                Message = "Reset Password",
                Response = model
            });
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPassword resetPassword)
        {
            var result = await _authenticationService.ResetPassword(resetPassword.Email, resetPassword.Token, resetPassword.Password);

            if (result.IsSuccess)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }


    }
}