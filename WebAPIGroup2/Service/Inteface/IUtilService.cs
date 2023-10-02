using WebAPIGroup2.Models;
using WebAPIGroup2.Models.DTO;

namespace WebAPIGroup2.Service.Inteface
{
    //Write Code reused
    public interface IUtilService
    {
        public Task<MailContent> SendEmailAsync(MailContent mailContent);

        public Task<string> ValidateCodeAsync(string code, UserDTO user);

        public Task<string> Upload(IFormFile formFile);

        public Task<List<string>> UploadMany(IFormFile[] formFile);
    }
}
