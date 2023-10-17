using Microsoft.EntityFrameworkCore;
using WebAPIGroup2.Models;
using WebAPIGroup2.Models.POJO;
using WebAPIGroup2.Respository.Inteface;

namespace WebAPIGroup2.Respository.Implement
{
    public class ContentEmailRepo : GenericRepository<ContentEmail>, IContentEmailRepo
    {
        public ContentEmailRepo(MyImageContext context) : base(context)
        {
        }
        
        public Task<ContentEmail?> GetByIDAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
