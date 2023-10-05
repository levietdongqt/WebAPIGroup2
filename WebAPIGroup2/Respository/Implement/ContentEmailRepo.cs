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

        public Task<DeliveryInfo?> GetByIDAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
