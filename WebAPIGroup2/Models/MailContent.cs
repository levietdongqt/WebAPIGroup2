namespace WebAPIGroup2.Models
{
    public class MailContent
    {
        public string Email { get; set; }
        public string Subject { get; set; }
        public string htmlhtmlMessage { get; set; }

        public string type { get; set; }

        public MailContent() { }

        public MailContent(string email, string subject, string htmlhtmlMessage, string type)
        {
            Email = email;
            Subject = subject;
            this.htmlhtmlMessage = htmlhtmlMessage;
            this.type = type;
        }
    }
}
