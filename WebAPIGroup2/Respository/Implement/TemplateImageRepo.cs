using Microsoft.EntityFrameworkCore;
using WebAPIGroup2.Models;
using WebAPIGroup2.Models.POJO;
using WebAPIGroup2.Respository.Inteface;

namespace WebAPIGroup2.Respository.Implement
{
    public class TemplateImageRepo : GenericRepository<TemplateImage>, ITemplateImageRepo
    {
        public TemplateImageRepo(MyImageContext context) : base(context)
        {
        }

        public  Task<TemplateImage?> GetByIDAsync(int id)
        {
            return _context.TemplateImages.Include(i => i.Template).FirstOrDefaultAsync(i => i.Id == id);
        }

       
    }

}
