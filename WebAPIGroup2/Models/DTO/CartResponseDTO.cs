namespace WebAPIGroup2.Models.DTO
{
    public class CartResponseDTO
    {
        public string image { get; set; }
        public string templateName { get; set; }

        public int? quantity { get; set; }

        public decimal? price { get; set; }
        public double? width { get; set; }

        public double? length { get; set; }

        public string materialPage { get; set; }

    }
}
