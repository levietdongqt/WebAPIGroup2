using WebAPIGroup2.Models.DTO;

namespace WebAPIGroup2.Service.Inteface
{
    public interface IUpLoadService
    {
        Task<List<string>> SaveImages(int userID, int templateID, IFormFile[] files);
        Task<bool> SaveProductDetails(UpLoadDTO upLoadDTO, List<string> imagesUrls);
        Task<bool> ValidateFiles(IFormFile[] files);
        Task<bool> ValidateRequestData(UpLoadDTO upLoadDTO);
    }
}
