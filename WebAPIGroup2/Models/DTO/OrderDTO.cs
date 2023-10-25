namespace WebAPIGroup2.Models.DTO
{
    public class OrderDTO
    {
        public int userID { get; set; }
        public int myImageID {  get; set; }

        public int? templateId { get; set; }
        public int? materialPageId { get; set; }

        public int temlateSizeId { get; set; }
        public string? imageArea { get; set; }

        public int quantity { get; set; }

    }
}
