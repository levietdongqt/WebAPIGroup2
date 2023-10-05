using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebAPIGroup2.Models.DTO
{
    public class FeedBackDTO
    {
        public int Id { get; set; }

        public string? Content { get; set; }

        public int? UserId { get; set; }

        public string? Email { get; set; }

        public DateTime? FeedBackDate { get; set; }
    }
}
