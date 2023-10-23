using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WebAPIGroup2.Models.POJO;

namespace WebAPIGroup2.Models.DTO
{
    public class DeliveryInfoDTO
    {
        public int Id { get; set; }

        public int? UserId { get; set; }

        public string? Email { get; set; }

        public string? DeliveryAddress { get; set; }

        public string? Phone { get; set; }

        public string? CustomName { get; set; }


        public virtual ICollection<ContentEmailDTO>? ContentEmails { get; set; } = new List<ContentEmailDTO>();
    }
}
