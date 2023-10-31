namespace User.Management.API.Models.DTO
{
    public class StayDTO
    {
        public int StayId { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string ImageUrl { get; set; }
        public int MaxGuests { get; set; }
        public int UserProfileId { get; set; }
    }
}
