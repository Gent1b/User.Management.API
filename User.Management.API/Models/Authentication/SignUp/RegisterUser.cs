﻿using System.ComponentModel.DataAnnotations;

namespace User.Management.API.Models.Authentication.SignUp
{
    public class RegisterUser
    {
        [Required(ErrorMessage = "User Name is Required")]
        public string? UserName { get; set; }
        [EmailAddress]
        [Required(ErrorMessage = "Email is Required")]

        public string? Email { get; set;}

        [Required(ErrorMessage ="Password is required")]
        public string? Password { get; set;}
/*        public string Role { get; set; }
*/
    }
}
