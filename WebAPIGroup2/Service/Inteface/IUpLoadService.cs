using Microsoft.AspNetCore.Mvc;
using WebAPIGroup2.Models.DTO;
using WebAPIGroup2.Models.POJO;

namespace WebAPIGroup2.Service.Inteface
{
    public interface IUpLoadService
    {
        Task<bool> AddToCart(OrderDTO orderDTO);
        Task<List<CartResponseDTO>> LoadCart(int userID);
        Task<List<MyImagesResponseDTO>> LoadMyImages(int userID);
        Task<List<string>> SaveImages(string folderName, int? templateID, IFormFile[] files);
        Task<MyImage> SaveToDBTemporary(string folderName,UpLoadDTO upLoadDTO, List<string> imagesUrls);
        Task<List<int>> SaveToDBWithNoTemplate(string folderName, UpLoadDTO upLoadDTO, List<string> imagesUrls);
        Task<bool> UpdateCart(int productDetailID,int quantity);
        Task<bool> ValidateFiles(IFormFile[] files);
        Task<string> ValidateRequestData(UpLoadDTO upLoadDTO);
    }
}
