using WebAPIGroup2.Models;
using WebAPIGroup2.Models.POJO;
using WebAPIGroup2.Respository.Inteface;

namespace WebAPIGroup2.Respository.Implement
{
    public class ImageRepo : GenericRepository<Image>, IImageRepo
    {
        public ImageRepo(MyImageContext context) : base(context)
        {
        }
        public Task<Image?> GetByIDAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
