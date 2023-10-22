namespace WebAPIGroup2.Models.DTO;

public class PurchaseOrderDTO
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public int? DeliveryInfoId { get; set; }
    
    public string? CreditCard { get; set; }
    
    public decimal? PriceTotal { get; set; }
    
    public string Status { get; set; } = null!;

    public DateTime? CreateDate { get; set; }
    
    public virtual DeliveryInfoDTO? DeliveryInfo { get; set; }

    public virtual ICollection<MyImageDTO> MyImages { get; set; } = new List<MyImageDTO>();
    
}