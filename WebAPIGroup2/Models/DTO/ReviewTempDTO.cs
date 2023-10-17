namespace WebAPIGroup2.Models.DTO;

public class ReviewTempDTO
{
    public int Id { get; set; }

    public int? TemplateId { get; set; }

    public int? UserId { get; set; }
    public string? Content { get; set; }

    public double? Rating { get; set; }

    public DateTime? ReviewDate { get; set; }
    
}