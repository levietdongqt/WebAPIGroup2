﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPIGroup2.Controllers.UserModule;
using WebAPIGroup2.Models.DTO;
using WebAPIGroup2.Models.POJO;

namespace WebAPIGroup2.Service.Inteface
{
    public interface ICartService
    {
        Task<bool> AddToCart(OrderDTO orderDTO);
        Task<bool> AddToCartAllSimple(OrderDTO orderDTO);
        Task<DeliveryInfo> createDeliveryInfo(CartController.PurchaseDTO purchaseDTO);
        Task<PurchaseOrder> createOrder(CartController.PurchaseDTO purchaseDTO, DeliveryInfo deliveryInfo,string status);
        Task<bool> deleteAllCart(List<int> productIdList);
        Task<bool> deleteFolder(int purchaseID);
        Task<bool> deleteProductDetail(int productDetailID);
        Task<List<CartResponseDTO>> LoadCart(int userID);
        Task<bool> sendMail(CartController.PurchaseDTO purchaseDTO, DeliveryInfo deliveryInfo);
        Task<bool> UpdateCart(int productDetailID, int quantity);
    }
}
