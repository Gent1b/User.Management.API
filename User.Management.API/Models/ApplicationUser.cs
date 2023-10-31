﻿using Microsoft.AspNetCore.Identity;

namespace User.Management.API.Models
{
    public class ApplicationUser: IdentityUser
    {
        public UserProfile UserProfile { get; set; }

    }
}
