using Microsoft.EntityFrameworkCore;
using WebAPIGroup2.Models;
using WebAPIGroup2.Models.POJO;
using WebAPIGroup2.Respository.Inteface;

namespace WebAPIGroup2.Respository.Implement
{
    public class MaterialPageRepo : GenericRepository<MaterialPage>, IMaterialPageRepo
    {
        public MaterialPageRepo(MyImageContext context) : base(context)
        {
        }

        public async  Task<MaterialPage?> GetByIDAsync(int id)
        {
            return await _context.MaterialPages.FirstOrDefaultAsync(m => m.Id == id);
        }
    }
}
