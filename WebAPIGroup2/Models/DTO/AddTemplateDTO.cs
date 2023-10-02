using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using WebAPIGroup2.Models.POJO;

namespace WebAPIGroup2.Models.DTO
{
    public class AddTemplateDTO
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public decimal? PricePlus { get; set; }

        public bool? Status { get; set; }

        public int? QuantitySold { get; set; }

        public DateTime? CreateDate { get; set; }

        
        public List<DescriptionTemplateDTO> DescriptionTemplates { get; set; } = new List<DescriptionTemplateDTO>();

        [FromForm]
        public IFormFile[] formFileList { get; set; }


    }
}
