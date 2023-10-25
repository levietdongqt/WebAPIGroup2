using WebAPIGroup2.Models.POJO;

namespace WebAPIGroup2.Models.DTO;

public class CollectionDTO
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? ImageUrl { get; set; }
    
    public int CategoryId { get; set; }
    
    public virtual CategoryTempDTO Category { get; set; }
    
    public virtual ICollection<CollectionTemplateDTO> CollectionTemplates { get; set; } = new List<CollectionTemplateDTO>();
    public virtual List<TemplateDTO> TemplateDTO { get; set; } = new List<TemplateDTO>();
}

public class CollectionWithTemplateDTO
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public List<TemplateDTO> TemplateNames { get; set; }
}
