namespace WebAPIGroup2.Models.DTO;

public class TemplateDTO
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public decimal? PricePlus { get; set; }

    public bool? Status { get; set; }

    public int? QuantitySold { get; set; }

    public DateTime? CreateDate { get; set; }
    
    public List<int> CategoryIds { get; set; }
}
public class TemplateWithCategoryDTO
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public decimal? PricePlus { get; set; }

    public bool? Status { get; set; }

    public int? QuantitySold { get; set; }

    public DateTime? CreateDate { get; set; }
    
    public List<String> CategoryNames { get; set; }
} 