namespace WebAPIGroup2.Models.DTO;

public class CategoryDTO
{
    public int Id { get; set; }

    public string? Name { get; set; }
}

public class CategoryWithTemplateDTO
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public List<String> TemplateNames { get; set; }
}