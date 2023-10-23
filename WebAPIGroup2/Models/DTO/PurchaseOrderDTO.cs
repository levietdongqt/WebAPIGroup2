using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WebAPIGroup2.Models.POJO;

namespace WebAPIGroup2.Models.DTO
{
    public class PurchaseOrderDTO
    {
        public int Id { get; set; }

        public int? UserId { get; set; }

        public int? DeliveryInfoId { get; set; }

        public string? CreditCard { get; set; }

        public decimal? PriceTotal { get; set; }

        public string Status { get; set; } = null!;

        public DateTime? CreateDate { get; set; }

        //public virtual ICollection<UserDTO> User { get; set; } = new List<UserDTO>();
        public virtual UserDTO User { get; set; } =  new UserDTO();
        public virtual DeliveryInfoDTO Delivery { get; set; } = new DeliveryInfoDTO();


        //public virtual ICollection<DeliveryInfoDTO> Deliverys { get; set; } = new List<DeliveryInfoDTO>();


    }
}
