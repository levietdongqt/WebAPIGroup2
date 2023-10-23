namespace WebAPIGroup2.Models.DTO;

public class ImageDTO
{
    public int Id { get; set; }

    public int MyImagesId { get; set; }
    
    public string? ImageUrl { get; set; }
    
    public string? FolderName { get; set; }
    
    public bool? Status { get; set; }

    public int? Index { get; set; }
    
    public DateTime? CreateDate { get; set; }
    
    public virtual MyImageDTO? MyImages { get; set; }
}