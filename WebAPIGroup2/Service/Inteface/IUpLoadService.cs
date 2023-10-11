using WebAPIGroup2.Models.DTO;
using WebAPIGroup2.Models.POJO;

namespace WebAPIGroup2.Service.Inteface
{
    public interface IUpLoadService
    {
        Task<bool> CreateOrder(OrderDTO orderDTO);
        Task<List<MyImage>> LoadMyImages(int userID);
        Task<List<string>> SaveImages(string folderName, int templateID, IFormFile[] files);
        Task<MyImage> SaveToDBTemporary(string folderName,UpLoadDTO upLoadDTO, List<string> imagesUrls);
        Task<bool> ValidateFiles(IFormFile[] files);
        Task<string> ValidateRequestData(UpLoadDTO upLoadDTO);
    }
}
