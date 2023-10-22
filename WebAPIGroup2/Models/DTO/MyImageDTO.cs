namespace WebAPIGroup2.Models.DTO;

public class MyImageDTO
{
    public int Id { get; set; }

    public int? TemplateId { get; set; }

    public int? PurchaseOrderId { get; set; }

    public bool? Status { get; set; }

    public virtual ICollection<ImageDTO> Images { get; set; } = new List<ImageDTO>();
    
    public virtual ICollection<ProductDetailDTO> ProductDetails { get; set; } = new List<ProductDetailDTO>();

}