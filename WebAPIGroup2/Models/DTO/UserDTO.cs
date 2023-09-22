using WebAPIGroup2.Models.POJO;

namespace WebAPIGroup2.Models.DTO
{
    public class UserDTO
    {
        public int Id { get; set; }

        public string? UserName { get; set; }

        public string? Password { get; set; }
        public string? FullName { get; set; }

        public string? Email { get; set; }

        public bool? EmailConfirmed { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string? Address { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Status { get; set; }
        public string? Role { get; set; }

    }
}
