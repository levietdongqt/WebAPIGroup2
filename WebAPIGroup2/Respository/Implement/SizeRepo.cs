using Microsoft.EntityFrameworkCore;
using WebAPIGroup2.Models;
using WebAPIGroup2.Models.POJO;
using WebAPIGroup2.Respository.Inteface;

namespace WebAPIGroup2.Respository.Implement;

public class SizeRepo : GenericRepository<PrintSize>,ISizeRepo
{
    public SizeRepo(MyImageContext context) : base(context)
    {
    }

    public async Task<PrintSize?> GetByIDAsync(int id)
    {
        var result = await _context.PrintSizes.FirstOrDefaultAsync(s => s.Id == id);
        if (result == null)
        {
            return null;
        }

        return result;

    }
}