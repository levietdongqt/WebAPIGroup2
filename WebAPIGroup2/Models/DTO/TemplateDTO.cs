using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WebAPIGroup2.Models.POJO;

namespace WebAPIGroup2.Models.DTO
{
    public class TemplateDTO
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public decimal? PricePlus { get; set; }

        public bool? Status { get; set; }

        public int? QuantitySold { get; set; }

        public DateTime? CreateDate { get; set; }

        public virtual ICollection<DescriptionTemplateDTO> DescriptionTemplates { get; set; } = new List<DescriptionTemplateDTO>();


        public virtual ICollection<TemplateImageDTO> TemplateImages { get; set; } = new List<TemplateImageDTO>();
    }
}
