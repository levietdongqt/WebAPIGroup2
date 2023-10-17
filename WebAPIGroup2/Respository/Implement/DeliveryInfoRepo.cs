using Microsoft.EntityFrameworkCore;
using WebAPIGroup2.Models;
using WebAPIGroup2.Models.POJO;
using WebAPIGroup2.Respository.Inteface;

namespace WebAPIGroup2.Respository.Implement
{
    public class DeliveryInfoRepo : GenericRepository<DeliveryInfo>, IDeliveryInfoRepo
    {
        public DeliveryInfoRepo(MyImageContext context) : base(context)
        {
        }

        public async Task<DeliveryInfo?> GetByIDAsync(int id)
        {
            return await _context.DeliveryInfos.FirstOrDefaultAsync(X=>X.Id == id);
        }
    }
}
