using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace User.Management.API.Models
{
    public class UserProfile
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("AspNetUser")]
        public string AspNetUserId { get; set; }

        public string FullName { get; set; }
        public string Country { get; set; }
        public int Age { get; set; }

        // You can add other user-specific fields here

        public virtual ApplicationUser AspNetUser { get; set; }

        [JsonIgnore]

        public List<Stay> Stays { get; set; }


    }
}
