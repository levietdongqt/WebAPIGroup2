﻿using WebAPIGroup2.Models;
using WebAPIGroup2.Models.POJO;

namespace WebAPIGroup2.Respository.Inteface
{
    public interface IUserRepo :  IBaseRepository<User, int>
    {
        public Task<User> GetUser(LoginRequestDTO loginRequest);
        public Task<User> GetUserByEmail(string? userEmail);
        
        public Task<dynamic> GetTotalUsersByMonth();
        public Task<User?> GetOrderByUserId(int id);

        public Task<int> CountUserNormal();

        public Task<int> CountUserGoogle();
        Task<List<User>> getPurchaseList(string? search, string? st);
    }
}
