using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using User.Management.API.Models;

namespace User.Management.API
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Stay> Stays { get; set; } // Add DbSet for the Stay entity


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure the relationship between ApplicationUser and UserProfile
            builder.Entity<UserProfile>()
                .HasOne(up => up.AspNetUser)
                .WithOne(au => au.UserProfile)
                .HasForeignKey<UserProfile>(up => up.AspNetUserId);

            // Configure the relationship between UserProfile and Stay
            builder.Entity<UserProfile>()
                .HasMany(up => up.Stays)
                .WithOne(s => s.UserProfile)
                .HasForeignKey(s => s.UserProfileId);


            SeedRoles(builder);
        }

        private static void SeedRoles(ModelBuilder builder)
        {
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = "1",
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                    ConcurrencyStamp = "1"
                },
                new IdentityRole
                {
                    Id = "2",
                    Name = "User",
                    NormalizedName = "USER",
                    ConcurrencyStamp = "2"
                },
                new IdentityRole
                {
                    Id = "3",
                    Name = "HR",
                    NormalizedName = "HR",
                    ConcurrencyStamp = "3"
                }
            );
        }
    }
}
