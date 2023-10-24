namespace WebAPIGroup2.Models.DTO;

public class ProductDetailDTO
{
    public int Id { get; set; }

    public int? MaterialPageId { get; set; }

    public int? TemplateSizeId { get; set; }

    public int? MyImageId { get; set; }
    
    public decimal? Price { get; set; }

    public int? Quantity { get; set; }

    public bool? Status { get; set; }

    public DateTime? CreateDate { get; set; }
    
    public virtual MaterialPageDTO? MaterialPage { get; set; }
    
    public virtual MyImageDTO? MyImage { get; set; }
    
    public virtual TemplateSizeDTO? TemplateSize { get; set; }
}