using WebAPIGroup2.Models;
using WebAPIGroup2.Models.POJO;
using WebAPIGroup2.Respository.Inteface;

namespace WebAPIGroup2.Respository.Implement
{
    public class DescriptionTemplateRepo : GenericRepository<DescriptionTemplate>, IDescriptionTemplateRepo
    {
        public DescriptionTemplateRepo(MyImageContext context) : base(context)
        {
        }

        public Task<DescriptionTemplate?> GetByIDAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}