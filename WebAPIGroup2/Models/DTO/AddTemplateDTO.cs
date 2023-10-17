using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using WebAPIGroup2.Models.POJO;

namespace WebAPIGroup2.Models.DTO
{
    public class AddTemplateDTO
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? PricePlusPerOne { get; set; }

        public bool? Status { get; set; }

        public int? QuantitySold { get; set; }

        public DateTime? CreateDate { get; set; }

        
        public List<DescriptionTemplateDTO> DescriptionTemplates { get; set; } = new List<DescriptionTemplateDTO>();

        public IFormFile[]? formFileList { get; set; }

        public List<CollectionDTO> collectionDTOs { get; set; } = new List<CollectionDTO>();

        public List<SizeDTO> sizeDTOs { get; set; } = new List<SizeDTO>();
    }
}
