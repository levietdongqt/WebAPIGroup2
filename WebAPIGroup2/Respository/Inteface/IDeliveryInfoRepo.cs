﻿using WebAPIGroup2.Models.POJO;

namespace WebAPIGroup2.Respository.Inteface
{
    public interface IDeliveryInfoRepo : IBaseRepository<DeliveryInfo, int>
    {
        Task<DeliveryInfo?> getByAdress(int userID,string address);
    }
}
