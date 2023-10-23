using WebAPIGroup2.Models;
using WebAPIGroup2.Models.POJO;
using WebAPIGroup2.Respository.Inteface;

namespace WebAPIGroup2.Respository.Implement
{
    public class MonthlySpendingRepo : GenericRepository<MonthlySpending>, IMonthlySpendingRepo
    {
        public MonthlySpendingRepo(MyImageContext context) : base(context)
        {
        }

        public Task<MonthlySpending?> GetByIDAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
