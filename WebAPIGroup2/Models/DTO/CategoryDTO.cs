using WebAPIGroup2.Models.POJO;

namespace WebAPIGroup2.Models.DTO;

public class CategoryDTO
{
    public int Id { get; set; }

    public string? Name { get; set; }

    //public virtual ICollection<CategoryTemplate> CategoryTemplates { get; set; } = new List<CategoryTemplate>();
}

public class CategoryWithTemplateDTO
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public List<String> TemplateNames { get; set; }
}