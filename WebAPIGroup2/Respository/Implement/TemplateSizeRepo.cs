using WebAPIGroup2.Models;
using WebAPIGroup2.Models.POJO;
using WebAPIGroup2.Respository.Inteface;

namespace WebAPIGroup2.Respository.Implement
{
    public class TemplateSizeRepo : GenericRepository<TemplateSize>, ITemplateSizeRepo
    {
        public TemplateSizeRepo(MyImageContext context) : base(context)
        {
        }

        public Task<TemplateSize?> GetByIDAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
