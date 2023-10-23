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

        public async Task<DeliveryInfo?> getByAdress(int userID, string address)
        {
            var deliveryInfo = await _context.DeliveryInfos.Where(t => t.DeliveryAddress.Equals(address) && t.UserId == userID).FirstOrDefaultAsync();
            return deliveryInfo;
        }

        public async Task<DeliveryInfo?> GetByIDAsync(int id)
        {
            return await _context.DeliveryInfos.FirstOrDefaultAsync(X=>X.Id == id);
        }
    }
}
