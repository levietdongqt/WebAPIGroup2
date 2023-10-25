using Microsoft.EntityFrameworkCore;
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

        public  List<Image> getByIdList(List<int> list)
        {
            try
            {
                var images = _context.Images.Include(t => t.MyImages).ThenInclude(t=> t.ProductDetails).Where(t => list.Contains(t.Id)).ToList();
                if(images.Count > 0)
                {
                    return images;
                }
                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
          
        }
    }
}
