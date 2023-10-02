using Microsoft.EntityFrameworkCore;
using WebAPIGroup2.Models;
using WebAPIGroup2.Models.POJO;
using WebAPIGroup2.Respository.Inteface;

namespace WebAPIGroup2.Respository.Implement;

public class SizeRepo : GenericRepository<PrintSize>,ISizeRepo
{
    private readonly MyImageContext _context;
    private readonly GenericRepository<PrintSize> _genericRepository;
    public SizeRepo(MyImageContext context, GenericRepository<PrintSize> genericRepository) : base(context)
    {
        _context = context;
        _genericRepository = genericRepository;
    }

    public async Task<IEnumerable<PrintSize>?> GetAllAsync()
    {
        return await _genericRepository.GetAllAsync();
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

    public async Task<bool> InsertAsync(PrintSize entity)
    {
        return await _genericRepository.InsertAsync(entity);
    }

    public async Task<bool> InsertAllAsync(List<PrintSize> list)
    {
        return await _genericRepository.InsertAllAsync(list);
    }

    public async Task<bool> UpdateAsync(PrintSize entity)
    {
        return await _genericRepository.UpdateAsync(entity);
    }

    public async Task<bool> UpdateAllAsync(List<PrintSize> list)
    {
        return await _genericRepository.UpdateAllAsync(list);
    }

    public async Task<bool> DeleteAsync(PrintSize entity)
    {
        return await _genericRepository.DeleteAsync(entity);
    }

    public async Task<bool> DeleteAllAsync(List<PrintSize> list)
    {
        return await _genericRepository.DeleteAllAsync(list);
    }
}