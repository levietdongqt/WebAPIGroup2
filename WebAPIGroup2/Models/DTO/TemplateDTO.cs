
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

        public float? PricePlus { get; set; }

        public bool? Status { get; set; }

        public int? QuantitySold { get; set; }

        public DateTime? CreateDate { get; set; }

        public virtual ICollection<DescriptionTemplateDTO> DescriptionTemplates { get; set; } = new List<DescriptionTemplateDTO>();


        public virtual ICollection<TemplateImageDTO> TemplateImages { get; set; } = new List<TemplateImageDTO>();

        [JsonIgnore]
        public virtual ICollection<CollectionTemplateDTO> CollectionTemplates { get; set; } = new List<CollectionTemplateDTO>();
        public virtual List<CollectionDTO> CollectionsDTO { get; set; } = new List<CollectionDTO>();

        [JsonIgnore]
        public virtual ICollection<TemplateSizeDTO> TemplateSizes { get; set; } = new List<TemplateSizeDTO>();

        public virtual List<SizeDTO> SizesDTO { get; set; } = new List<SizeDTO>();

        public virtual ICollection<ReviewDTO> Reviews { get; set; } = new List<ReviewDTO>();
    }
}

