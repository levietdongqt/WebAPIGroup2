using WebAPIGroup2.Models.POJO;

namespace WebAPIGroup2.Models.DTO;

public class CollectionDTO
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? ImageUrl { get; set; }

    //public virtual ICollection<CategoryTemplate> CategoryTemplates { get; set; } = new List<CategoryTemplate>();
}

public class CollectionWithTemplateDTO
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public List<String> TemplateNames { get; set; }
}