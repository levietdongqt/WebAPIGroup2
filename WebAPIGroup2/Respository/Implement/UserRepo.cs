using Microsoft.EntityFrameworkCore;
using WebAPIGroup2.Models;
using WebAPIGroup2.Models.POJO;
using WebAPIGroup2.Respository.Inteface;

namespace WebAPIGroup2.Respository.Implement
{
    public class UserRepo : GenericRepository<User>, IUserRepo
    {
        public UserRepo(MyImageContext context) : base(context)
        {
        }

        public async Task<int> CountUserGoogle()
        {
            var count = await _context.Users.Where(x=>x.Password == null).CountAsync();
            return count;
        }

        public async Task<int> CountUserNormal()
        {
            var count = await _context.Users.Where(x => x.Password != null).CountAsync();
            return count;
        }

        public async Task<User?> GetByIDAsync(int id)
        {
            var user = await _context.Users.Include(x=>x.Reviews.OrderByDescending(x=>x.ReviewDate)).Include(x => x.FeedBacks).Include(x=>x.DeliveryInfos).ThenInclude(c=>c.ContentEmails)
                .FirstOrDefaultAsync(x => x.Id.Equals(id));
            if(user == null)
            {
                return null;
            }
            return user;
        }

        public async Task<List<User>> getPurchaseList(string? search, string? st)
        {
           var list =await  _context.Users.Include(t => t.PurchaseOrders).Where(t => t.FullName.ToLower() == search.ToLower()).ToListAsync();
           
            foreach (var item in list)
            {
                
            }
            if (list.Count > 0)
            {
                return list;
            }

            return null;

        }

        public async Task<dynamic> GetTotalUsersByMonth()
        {
            var yearNow = DateTime.Now.Year;
            var userList = await _context.Users.ToListAsync();
            var list = userList
            .Where(x=> x.CreateDate.Value.Year == yearNow)
            .GroupBy(x => new { Month = x.CreateDate.GetValueOrDefault().Month })
            .Select(g => new
                {
                    label = g.Key.Month,
                    value = g.Count()
                })
                .ToList();
            return list;
        }

        public async Task<User?>  GetUser(LoginRequestDTO loginRequest)
        {
            string role = loginRequest.isClient ? "user" : "admin";
            var user = await _context.Users.FirstOrDefaultAsync(t => t.Email.Equals(loginRequest.email) && t.Role.Equals(role) && t.Status.Equals(UserStatus.Enabled));
            return user;
        }
        public async Task<User> GetUserByEmail(string? userEmail)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email.Equals(userEmail));
        }

        public async Task<User?> GetOrderByUserId(int id)
        {
            return await _context.Users.Include(x=>x.Reviews).Include(u => u.PurchaseOrders).ThenInclude(u => u.MyImages)
                .FirstOrDefaultAsync(u => u.Id.Equals(id));
        }
        
    }
}
