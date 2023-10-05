using System.Text.Json.Serialization;
using WebAPIGroup2.Models.POJO;

namespace WebAPIGroup2.Models.DTO
{
    public class UserDTO
    {
        public int Id { get; set; }

        public string? Password { get; set; }
        public string? FullName { get; set; }

        public string? Email { get; set; }

        public bool? EmailConfirmed { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string? Address { get; set; }

        public string? Phone { get; set; }

        public string? Status { get; set; }
        public string? Role { get; set; }
        [JsonIgnore]
        public string oldPassword { get; set; }
        [JsonIgnore]
        public string newPassword { get; set; }
    }

    
}
