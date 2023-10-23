using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MimeKit.Cryptography;
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
        private readonly MyImageContext _context;
        private readonly IMapper _mapper;
        private readonly IUserRepo _userRepo;
        private readonly IDeliveryInfoRepo _deliveryInfoRepo;
        private readonly IUserService _userService;


        public PurchaseOrderService(IPurchaseOrderRepo purchaseOrderRepo, IMapper mapper, MyImageContext context , IUserRepo userRepo, IDeliveryInfoRepo deliveryInfoRepo, IUserService userService)
        {
            _purchaseOrderRepo = purchaseOrderRepo;
            _mapper = mapper;
            _context = context;
            _userRepo = userRepo;
            _deliveryInfoRepo = deliveryInfoRepo;
            _userService = userService;
            
        }

        public async Task<IEnumerable<PurchaseOrderDTO>?> GetAllAsync(string? search, string? st, int page, int pageSize)
        {
            List<PurchaseOrderDTO?> list = new List<PurchaseOrderDTO?>();

            var purs = await _purchaseOrderRepo.GetAllAsync();
            if (purs == null)
            {
                return null;
            }
            else
            {
                if (!string.IsNullOrEmpty(search) && !string.IsNullOrEmpty(st))
                {
                    search = search.ToLower();
                    purs = purs.Where(p => (p.Status == st) && (p.User.Email.Contains(search)));
                }
                else if (!string.IsNullOrEmpty(st))
                {
                    purs = purs.Where(p => p.Status == st);
                }
                else if (!string.IsNullOrEmpty(search))
                {
                    purs = purs.Where(p => p.User.Email.Contains(search));
                }

                purs = purs.Skip((page - 1) * pageSize).Take(pageSize);

            }
            list = _mapper.Map<List<PurchaseOrderDTO>>(purs);

            foreach (var pur in list)
            {
                // Check if UserId is not null
                if (pur.UserId.HasValue)
                {
                    var user = await _userRepo.GetByIDAsync(pur.UserId.Value);
                    if (user != null)
                    {
                        var userDTO = new UserDTO { FullName = user.FullName , Email = user.Email };
                        pur.User = userDTO;

                    }
                }

                // Check if DeliveryInfoId is not null
                if (pur.DeliveryInfoId.HasValue)
                {
                    var delivery = await _deliveryInfoRepo.GetByIDAsync(pur.DeliveryInfoId.Value);
                    if (delivery != null)
                    {
                        var deliveryDTO = _mapper.Map<DeliveryInfoDTO>(delivery);
                        pur.Delivery = deliveryDTO;
                    }
                }
            }



            return list;
        }


        public async Task<PurchaseOrderDTO> GetPurchaseOrdersByIDAsync(int id)
        {
            var pur = await _purchaseOrderRepo.GetByIDAsync(id);
            if (pur == null) return null;
            var purchaseOrderDTO = _mapper.Map<PurchaseOrderDTO>(pur);
            return purchaseOrderDTO;
        }

        public async Task<IEnumerable<PurchaseOrderDTO>?> GetPurchaseOrdersByStatus(int userID, List<string> statuses)
        {
            try
            {
                // Sử dụng hàm trong repository để lấy danh sách đơn hàng
                var purchaseOrders = await _purchaseOrderRepo.GetPurchaseOrdersByStatus(userID, statuses);
                if (purchaseOrders == null) return null;

                // Sử dụng AutoMapper để ánh xạ danh sách PurchaseOrder thành danh sách PurchaseOrderDTO
                var purchaseOrderDTOs = _mapper.Map<IEnumerable<PurchaseOrderDTO>>(purchaseOrders);

                return purchaseOrderDTOs;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public async Task<PurchaseOrderDTO> UpdatePurchaseOrder(PurchaseOrderDTO purchaseOrderDTO)
        {
            var pur = await _context.PurchaseOrders.SingleOrDefaultAsync(p => p.Id == purchaseOrderDTO.Id);
            if (pur != null)
            {
                pur.Status = purchaseOrderDTO.Status;
                var update = await _purchaseOrderRepo.UpdateAsync(pur);
                if (!update) return null;
            }
            var purDTO = _mapper.Map<PurchaseOrderDTO>(pur);
            return purDTO;
        }


        public async Task<dynamic> GetPurchaseOrderByMonth()
        {
            var po = await _purchaseOrderRepo.GetSumPriceTotalByMonth();
            if (po == null)
            {
                return null;
            }
            return po;
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
