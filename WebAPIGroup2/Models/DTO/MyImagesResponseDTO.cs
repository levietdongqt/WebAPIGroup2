using WebAPIGroup2.Models.POJO;

namespace WebAPIGroup2.Models.DTO
{
    public class MyImagesResponseDTO
    {
        public int Id { get; set; }
        public List<Image> images { get; set; } = new List<Image>();

        public string templateName { get; set; }

        public double? pricePlusPerOne { get; set; }

        public int? templateId { get; set; }


        public List<PrintSizeDTO> printSizes { get; set; }
        public DateTime? createDate { get; set; }

    }
}
