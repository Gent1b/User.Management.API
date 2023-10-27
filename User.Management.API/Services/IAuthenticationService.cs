using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using User.Management.API.Models;
using User.Management.API.Models.Authentication.Login;
using User.Management.API.Models.Authentication.SignUp;

namespace User.Management.API.Services
{
    public interface IAuthenticationService
    {
        Task<ApiResponse> RegisterUser(RegisterUser registerUser, string role);
        Task<ApiResponse> Login(LoginModel loginModel);
        Task<ApiResponse> ForgotPassword(string email, string forgotPasswordLink);
        Task<ApiResponse> ResetPassword(string email, string token, string newPassword);


    }
}
