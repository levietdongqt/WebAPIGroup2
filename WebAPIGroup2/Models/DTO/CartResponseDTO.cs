namespace WebAPIGroup2.Models.DTO
{
    public class CartResponseDTO
    {
        public List<string> images { get; set; }
        public string templateName { get; set; }

        public int? quantity { get; set; }

        public decimal? price { get; set; }
        public double? width { get; set; }

        public double? length { get; set; }

        public string materialPage { get; set; }
        
        public int productId { get; set; }

    }
}
