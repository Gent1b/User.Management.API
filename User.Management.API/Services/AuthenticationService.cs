using Azure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using User.Management.API.Models;
using User.Management.API.Models.Authentication.Login;
using User.Management.API.Models.Authentication.SignUp;
using User.Management.API.Repositories;
using User.Management.Service.Models;
using User.Management.Service.Services;

namespace User.Management.API.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager; // Replace ApplicationUser with your custom user class
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        private readonly IUserProfileRepository _userProfileRepository;

        public AuthenticationService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, IEmailService emailService,IUserProfileRepository userProfileRepository)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _emailService = emailService;
            _userProfileRepository = userProfileRepository;
        }

        public Task ForgotPassword(string email)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse> Login(LoginModel loginModel)
        {
            var user = await _userManager.FindByNameAsync(loginModel.Username);
            if (user != null && await _userManager.CheckPasswordAsync(user, loginModel.Password))
            {
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                var userRoles = await _userManager.GetRolesAsync(user);
                foreach (var role in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, role));
                }

                // Check if the email is verified
                bool isEmailVerified = await _userManager.IsEmailConfirmedAsync(user);

                // Add a custom claim to the JWT token to indicate email verification status
                authClaims.Add(new Claim("EmailVerified", isEmailVerified.ToString()));

                // Add a custom claim to the JWT token to include the user role
                authClaims.Add(new Claim("UserRole", string.Join(",", userRoles)));

                var jwtToken = GetToken(authClaims);

                return new ApiResponse
                {
                    IsSuccess = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Login Successful",
                    Response = new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                        expiration = jwtToken.ValidTo,
                        emailVerified = isEmailVerified,
                        userName = user.UserName,
                        Email = user.Email,
                        Id = user.Id,
                        userRole = userRoles
                    }
                };
            }
            else
            {
                // Return an unauthorized response
                return new ApiResponse
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status401Unauthorized,
                    Message = "Unauthorized",
                    Response = "Invalid username or password"
                };
            }
        }

        public async Task<ApiResponse> RegisterUser(RegisterUser registerUser, string role)
        {
            var existingUserCheckResult = await CheckExistingUser(registerUser.Email, registerUser.UserName);
            if (!existingUserCheckResult.IsSuccess)
            {
                return existingUserCheckResult;
            }

            if (!await _roleManager.RoleExistsAsync(role))
            {
                return new ApiResponse
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Role doesn't exist",
                    Response = null
                };
            }

            var user = new ApplicationUser
            {
                Email = registerUser.Email,
                UserName = registerUser.UserName,
            };

            var result = await _userManager.CreateAsync(user, registerUser.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, role);

                // Create the user profile
                var userProfile = new UserProfile
                {
                    AspNetUserId = user.Id,
                    // Set other properties from the registration model
                    FullName = registerUser.FullName,
                    Country = registerUser.Country,
                    Age = registerUser.Age
                };

                // Save the user profile
                _userProfileRepository.CreateUserProfileAsync(userProfile);

                var emailConfirmationResult = await SendEmailConfirmation(user);
                if (emailConfirmationResult.IsSuccess)
                {
                    return new ApiResponse
                    {
                        IsSuccess = true,
                        StatusCode = StatusCodes.Status201Created,
                        Message = "User Created Successfully",
                        Response = result
                    };
                }
            }

            return new ApiResponse
            {
                IsSuccess = false,
                StatusCode = StatusCodes.Status500InternalServerError,
                Message = "Failed to Create User!",
                Response = result
            };
        }


        private async Task<ApiResponse> CheckExistingUser(string email, string userName)
        {
            try
            {
                var existingUserByEmail = await _userManager.FindByEmailAsync(email);
                var existingUserByUserName = await _userManager.FindByNameAsync(userName);

                if (existingUserByEmail != null || existingUserByUserName != null)
                {
                    return new ApiResponse
                    {
                        IsSuccess = false,
                        StatusCode = StatusCodes.Status409Conflict,
                        Message = existingUserByEmail != null ? "Email already exists" : "Username already exists",
                        Response = "Try Using a different email or username"
                    };
                }

                return new ApiResponse
                {
                    IsSuccess = true, // No conflicts
                    StatusCode = StatusCodes.Status200OK,
                    Message = "No conflicts",
                    Response = null
                };
            }
            catch (Exception ex)
            {
                // Handle the exception, log it, and return an appropriate response.
                return new ApiResponse
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = "An error occurred while checking for existing users",
                    Response = ex.Message // You can customize this error message.
                };
            }
        }
        public async Task<ApiResponse> ForgotPassword(string email, string forgotPasswordLink)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var message = new Message(new string[] { user.Email! }, "Forgot Password Link", forgotPasswordLink);
                _emailService.SendEmail(message);
                return new ApiResponse
                {
                    IsSuccess = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = $"Password Change request has been sent to {user.Email}. Please check your email."
                };
            }
            return new ApiResponse
            {
                IsSuccess = false,
                StatusCode = StatusCodes.Status400BadRequest,
                Message = "Couldn't send a link to the email. Please try again.",
                Response = "Couldn't send a link to the email. Please try again."
            };
        }
        public async Task<ApiResponse> ResetPassword(string email, string token, string newPassword)
        {
            var user = await _userManager.FindByEmailAsync(email);
            ApiResponse response;

            if (user != null)
            {
                var resetPassResult = await _userManager.ResetPasswordAsync(user, token, newPassword);
                if (resetPassResult.Succeeded)
                {
                    response = new ApiResponse
                    {
                        IsSuccess = true,
                        StatusCode = StatusCodes.Status200OK,
                        Message = "Password changed successfully",
                        Response = resetPassResult // You can include additional response data if needed.
                    };
                }
                else
                {
                    var errors = resetPassResult.Errors.Select(error => new { Code = error.Code, Description = error.Description });
                    response = new ApiResponse
                    {
                        IsSuccess = false,
                        StatusCode = StatusCodes.Status400BadRequest,
                        Message = "Password change failed",
                        Response = errors // Include errors in the response.
                    };
                }
            }
            else
            {
                response = new ApiResponse
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Couldn't change the password. Please try again.",
                    Response = null // You can include additional response data if needed.
                };
            }

            return response;
        }



        private async Task<ApiResponse> SendEmailConfirmation(ApplicationUser user) // Replace ApplicationUser with your custom user class
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var email = Uri.EscapeDataString(user.Email);
            var tokenEncoded = Uri.EscapeDataString(token);

            var confirmationLink = $"https://localhost:7266/api/Authentication/ConfirmEmail?token={tokenEncoded}&email={email}";
            var message = new Message(new string[] { user.Email! }, "Confirmation email link", confirmationLink!);

            _emailService.SendEmail(message);

            return new ApiResponse
            {
                IsSuccess = true,
                StatusCode = StatusCodes.Status200OK,
                Message = "Email confirmation sent",
                Response = null
            };
        }



        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(1),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );
            return token;
        }

        public Task ResetPassword(ResetPassword resetPassword)
        {
            throw new NotImplementedException();
        }
    }
}