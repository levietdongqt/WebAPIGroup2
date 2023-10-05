using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebAPIGroup2.Models.DTO
{
    public class ContentEmailDTO
    {
        public int Id { get; set; }

        public int? DeliveryInfoId { get; set; }

        public string? SubjectEmail { get; set; }

        public string? BodyEmail { get; set; }

        public string? Type { get; set; }
    }
}
