
﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WebAPIGroup2.Models.POJO;
using System.Text.Json.Serialization;

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

        [JsonIgnore]
        public virtual ICollection<CategoryTemplateDTO> CategoryTemplates { get; set; } = new List<CategoryTemplateDTO>();
        public virtual List<CategoryDTO> CategoriesDTO { get; set; } = new List<CategoryDTO>();

        [JsonIgnore]
        public virtual ICollection<TemplateSizeDTO> TemplateSizes { get; set; } = new List<TemplateSizeDTO>();

        public virtual List<SizeDTO> SizesDTO { get; set; } = new List<SizeDTO>();
    }
}

