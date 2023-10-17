namespace WebAPIGroup2.Models.DTO;

public class PaginationDTO<T>
{
    public int limit { get; set; }
    public int Page { get; set; }
    public int totalRows { get; set; }
    public List<T> Items { get; set; }
}