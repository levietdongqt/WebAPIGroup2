using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebAPIGroup2.Models;
using WebAPIGroup2.Models.DTO;
using WebAPIGroup2.Models.POJO;
using WebAPIGroup2.Respository.Inteface;
using WebAPIGroup2.Service.Inteface;

namespace WebAPIGroup2.Service.Implement;

public class SizeService : ISizeService
{
    private readonly ISizeRepo _sizeRepo;
    private readonly IMapper _mapper;
    private readonly MyImageContext _context;

    public SizeService(ISizeRepo sizeRepo, IMapper mapper, MyImageContext context)
    {
        _context = context;
        _mapper = mapper;
        _sizeRepo = sizeRepo;
    }
    public async Task<IEnumerable<SizeDTO>?> GetAll()
    {
        var getSize = await _sizeRepo.GetAllAsync();
        return _mapper.Map<IEnumerable<SizeDTO>>(getSize);
    }

    public async Task<SizeDTO?> GetSizeById(int id)
    {
        var getSize = await _sizeRepo.GetByIDAsync(id);
        return _mapper.Map<SizeDTO>(getSize);
    }

    public async Task<bool> UpdateSize(SizeDTO size)
    {
        var getSize = await _context.PrintSizes.SingleOrDefaultAsync(s => s.Id == size.Id);
        if (getSize == null)
        {
            return false;
        }
        getSize.Width = size.Width;
        getSize.Length = size.Length;
        getSize.CreateDate = DateTime.Now;
        var sizeUpdate = await _sizeRepo.UpdateAsync(getSize);
        return sizeUpdate;
    }
    public async Task<bool> DeleteSize(int id)
    {
        var getSize = await _context.PrintSizes.SingleOrDefaultAsync(s => s.Id == id);
        if (getSize != null)
        {
            return await _sizeRepo.DeleteAsync(getSize);
        }
        else
        {
            return false;
        }
    }

    public async Task<SizeDTO> CreateSize(SizeDTO size)
    {
        var mapSize = _mapper.Map<PrintSize>(size);
        mapSize.CreateDate =  DateTime.Now;
        var create = await _sizeRepo.InsertAsync(mapSize);
        if (create)
        {
            var Size = _mapper.Map<SizeDTO>(mapSize);
            return Size;
        }
        return null;
    }
}