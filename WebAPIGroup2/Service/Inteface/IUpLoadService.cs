namespace WebAPIGroup2.Service.Inteface
{
    public interface IUpLoadService
    {
        Task<List<string>> SaveImages(int userID, int templateID, IFormFile[] files);
        Task<bool> SaveProductDetails(int userID, int templateID, List<string> imagesUrls);
        Task<bool> ValidateFiles(IFormFile[] files);
    }
}
