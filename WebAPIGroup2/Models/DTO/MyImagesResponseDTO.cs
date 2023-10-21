using WebAPIGroup2.Models.POJO;

namespace WebAPIGroup2.Models.DTO
{
    public class MyImagesResponseDTO
    {
        public int Id { get; set; }
        public List<string> images { get; set; } = new List<string>();

        public string templateName { get; set; }

        public int? templateId { get; set; }


        public List<PrintSizeDTO> printSizes { get; set; }
        public DateTime? createDate { get; set; }

    }
}
