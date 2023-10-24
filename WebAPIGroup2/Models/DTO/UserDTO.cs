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

        public string? Avatar { get; set; }

        public string? Status { get; set; }
    
        public string? Role { get; set; }
    
        public virtual ICollection<DeliveryInfoDTO> DeliveryInfos { get; set; } = new List<DeliveryInfoDTO>();

        public virtual ICollection<FeedBackDTO> FeedBacks { get; set; } = new List<FeedBackDTO>();
  
        public virtual ICollection<ReviewTempDTO> Reviews { get; set; } = new List<ReviewTempDTO>();
        public bool? Gender { get; set; }
        public virtual ICollection<PurchaseOrderDTO> PurchaseOrders { get; set; } = new List<PurchaseOrderDTO>();
    }


    
}
