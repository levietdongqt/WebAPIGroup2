namespace WebAPIGroup2.Models.DTO
{
    public class UpLoadDTO
    {
        public int userID { get; set; }
        public int templateID { get; set; }

        public float imageArea { get; set; }

        public IFormFile[] files { get; set; } 

    }
}
