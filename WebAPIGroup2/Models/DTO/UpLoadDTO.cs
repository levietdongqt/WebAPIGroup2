namespace WebAPIGroup2.Models.DTO
{
    public interface UpLoadDTO
    {
        public int userID { get; set; }
        public int templateID { get; set; }

        public float imageArea { get; set; }

        IFormFile[] files { get; set; } 

    }
}
