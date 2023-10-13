namespace WebAPIGroup2.Models
{
    public class LoginRequestDTO
    {
        public string email {  get; set; }
        public string password { get; set; }

        public bool isClient { get; set; }
    }
}
