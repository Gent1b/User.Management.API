namespace User.Management.API.Models
{
    public class Stay
    {
        public int StayId { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string ImageUrl { get; set; }
        public int MaxGuests { get; set; }

        // Foreign key to UserProfile
        public int UserProfileId { get; set; }

        // Navigation property to UserProfile
        public UserProfile UserProfile { get; set; }
    }

}
