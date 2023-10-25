using Microsoft.EntityFrameworkCore;
using WebAPIGroup2.Models;
using WebAPIGroup2.Models.POJO;
using WebAPIGroup2.Respository.Inteface;

namespace WebAPIGroup2.Respository.Implement
{
    public class PurchaseOrderRepo : GenericRepository<PurchaseOrder>, IPurchaseOrderRepo
    {
        public PurchaseOrderRepo(MyImageContext context) : base(context)
        {
        }

        public async Task<PurchaseOrder?> GetByIDAsync(int id)
        {
            var pur = await _context.PurchaseOrders.FirstOrDefaultAsync(x => x.Id == id);
            return pur; // Trả về kết quả tìm thấy hoặc null nếu không tìm thấy
        }


        public async Task<PurchaseOrder> getPurchaseOrder(int userID, string status)
        {
            using (var context = new MyImageContext())
            {
                try
                {
                    if (status.Equals(PurchaseStatus.Temporary))
                    {
                        var purchase = await context.PurchaseOrders.FirstOrDefaultAsync(t => t.UserId == userID && (t.Status == status || t.Status == PurchaseStatus.InCart));
                        if (purchase == null)
                        {
                            throw new Exception("Purchare is not exist!");
                        }
                        return purchase;
                    }
                    var purchase2 = await context.PurchaseOrders.FirstOrDefaultAsync(t => t.UserId == userID && t.Status == status);
                    if (purchase2 == null)
                    {
                        throw new Exception("Purchare is not exist!");
                    }
                    return purchase2;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return null;
                }
            }

        }

        public async Task<IEnumerable<PurchaseOrder>> GetPurchaseOrdersByStatus(int userID, List<string> statuses)
        {
            try
            {
                var purchases = await _context.PurchaseOrders
                    .Where(po => po.UserId == userID && statuses.Contains(po.Status))
                    .ToListAsync();

                return purchases;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }


        public async Task<dynamic> GetSumPriceTotalByMonth()
        {
            List<int> Months = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
            var yearNow = DateTime.Now.Year;
            var query = from month in Months
                        join po in _context.PurchaseOrders
                        on month equals po.CreateDate.Value.Month into gj
                        from subPo in gj.DefaultIfEmpty()
                        where (subPo == null || (subPo != null && subPo.CreateDate.Value.Year == yearNow && subPo.Status == "Received"))
                        group subPo by new { Month = month } into g
                        select new
                        {
                            month = g.Key.Month,
                            sum = g.Sum(x => (x != null) ? x?.PriceTotal ?? 0 : 0),
                            status = g.Count(x => (x != null && x.Status == "Received"))
                        };
            return query.ToList();

        }

        public async Task<int> CountPurchaseInWeek()
        {
            DateTime now = DateTime.Now.Date;
            int currentDayOfWeek = (int)now.DayOfWeek;
            DateTime startWeek = now.AddDays(-currentDayOfWeek); // lay duoc ngay dau tuan
            DateTime endWeek = startWeek.AddDays(6);

            var count = _context.PurchaseOrders
                        .Where(po => po.CreateDate >= startWeek && po.CreateDate <= endWeek)
                        .CountAsync();
            return await count;
        }

        public async Task<IEnumerable<PurchaseOrder>> GetAllPurchaseOrders()
        {
            var data = await _context.PurchaseOrders
        .Include(p => p.User) // Kết hợp thông tin về người dùng
        .ToListAsync();

            return data;
        }
    }
}
