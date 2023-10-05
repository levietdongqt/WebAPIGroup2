using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPIGroup2.Models.DTO
{
    public class MaterialPageDTO
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public decimal? InchSold { get; set; }

        public double? PricePerInch { get; set; }

        public bool? Status { get; set; }

        public string? Description { get; set; }
    }
}
