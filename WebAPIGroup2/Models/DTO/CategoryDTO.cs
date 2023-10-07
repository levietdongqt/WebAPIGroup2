using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebAPIGroup2.Models.DTO
{
    public class CategoryDTO
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? ImageUrl { get; set; }

        public virtual ICollection<CollectionDTO> Collections { get; set; } = new List<CollectionDTO>();
    }
}
