using AutoMapper;
using WebAPIGroup2.Models;
using WebAPIGroup2.Models.DTO;
using WebAPIGroup2.Models.POJO;
using WebAPIGroup2.Respository.Inteface;
using WebAPIGroup2.Service.Inteface;

namespace WebAPIGroup2.Service.Implement
{
    public class PurchaseOrderService : IPurchaseOrderService
    {
        private readonly IPurchaseOrderRepo _purchaseOrderRepo;
        private readonly IMapper _mapper;
        public PurchaseOrderService(IPurchaseOrderRepo purchaseOrderRepo,IMapper mapper)
        {
            _purchaseOrderRepo = purchaseOrderRepo;
            _mapper = mapper;
        }
        public async Task<dynamic> GetPurchaseOrderByMonth()
        {
            var po = await _purchaseOrderRepo.GetSumPriceTotalByMonth();
            if(po == null)
            {
                return null;
            }
            return po;
        }
        public async Task<IEnumerable<PurchaseOrderDTO>> GetPurchaseOrderAll()
        {
            var purchaseOrders = await _purchaseOrderRepo.GetAllAsync();

            if (purchaseOrders != null)
            {
                return _mapper.Map<IEnumerable<PurchaseOrderDTO>>(purchaseOrders);
            }
            return new List<PurchaseOrderDTO>();
        }
        public async Task<PurchaseOrderDTO?> GetPurchaseOrderByUserId(int userId, string status)
        {
            PurchaseOrderDTO? result = null;
    
            switch (status)
            {
                case PurchaseStatus.Temporary:
                case PurchaseStatus.InCart:
                case PurchaseStatus.Received:
                case PurchaseStatus.ToShip:
                case PurchaseStatus.OrderPaid:
                    var purchaseOrder = await _purchaseOrderRepo.getPurchaseOrder(userId, status);
                    if (purchaseOrder != null)
                    {
                        result = _mapper.Map<PurchaseOrderDTO>(purchaseOrder);
                    }
                    break;
            }
            return result;
        }
    }
}
