using System.Text.Json.Serialization;

namespace WebAPIGroup2.Models.DTO
{
    public class AddUserDTO
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
        public bool? Gender { get; set; }
        public IFormFile? formFile { get; set; }

        public string? oldPassword { get; set; }
        public string? newPassword { get; set; }
    }
}
