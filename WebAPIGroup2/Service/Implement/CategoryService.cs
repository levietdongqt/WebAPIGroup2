using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebAPIGroup2.Models;
using WebAPIGroup2.Models.DTO;
using WebAPIGroup2.Models.POJO;
using WebAPIGroup2.Respository.Inteface;
using WebAPIGroup2.Service.Inteface;

namespace WebAPIGroup2.Service.Implement;

public class CategoryService : ICategoryService
{
    private readonly IMapper _mapper;
    private readonly ICategoryRepo _categoryRepo;
    private readonly MyImageContext _context;
    public CategoryService( IMapper mapper, ICategoryRepo categoryRepo, MyImageContext context)
    {
        _mapper = mapper;
        _categoryRepo = categoryRepo;
        _context = context;
    }
    public async Task<IEnumerable<CategoryDTO>?> GetAllasync()
    {
        var categories = await _categoryRepo.GetAllAsync();
        var mappedCategories = _mapper.Map<IEnumerable<CategoryDTO>>(categories);
        return mappedCategories;
    }

    public async Task<CategoryDTO?> GetCategoryById(int id)
    {
        var category = await _categoryRepo.GetByIDAsync(id);
        var mappedCategory = _mapper.Map<CategoryDTO>(category);
        return mappedCategory;
    }

    public async Task<CategoryWithTemplateDTO> GetCategoryWithTemplate(int id)
    {
        var category = await _categoryRepo.GetCategoryWithTemplate(id);
        return _mapper.Map<CategoryWithTemplateDTO>(category);
    }

    public async Task<bool> UpdateCategory(CategoryDTO category)
    {
        var getCategory = await _context.Categories.SingleOrDefaultAsync(x => x.Id == category.Id);
        if (getCategory != null)
        {
            getCategory.Id = category.Id;
            getCategory.Name = category.Name;
            var updatedCategory = await _categoryRepo.UpdateAsync(getCategory);
            return updatedCategory;
        }
        return false;
    }

    public async Task<bool> DeleteCategory(int id)
    {
        var getCategory = await _context.Categories.SingleOrDefaultAsync(x=> x.Id == id);
        if (getCategory == null)
        {
            return false;
        }
        var deletedCategory = await _categoryRepo.DeleteAsync(getCategory);
        return deletedCategory;
    }

    public async Task<CategoryDTO> CreateCategory(CategoryDTO category)
    {
        var mappedCategory = _mapper.Map<Category>(category);
        var createCategory = await _categoryRepo.InsertAsync(mappedCategory);
        if (createCategory)
        {
            var categoryDto = _mapper.Map<CategoryDTO>(mappedCategory);
            return categoryDto;
        }
        return null;
    }
    
}