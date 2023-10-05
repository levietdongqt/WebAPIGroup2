using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WebAPIGroup2.Models.POJO;

namespace WebAPIGroup2.Models.DTO
{

    public class DescriptionTemplateDTO
    {     
        public int Id { get; set; }
  
        public string? Title { get; set; }
   
        public string? Description { get; set; }

        public int? TemplateId { get; set; }
        
    }
}
