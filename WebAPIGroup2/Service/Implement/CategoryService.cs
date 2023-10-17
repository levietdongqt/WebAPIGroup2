using AutoMapper;
using WebAPIGroup2.Models.DTO;
using WebAPIGroup2.Models.POJO;
using WebAPIGroup2.Respository.Inteface;
using WebAPIGroup2.Service.Inteface;

namespace WebAPIGroup2.Service.Implement;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepo _categoryRepo;
    private readonly ICollectionRepo _collectionRepo;
    private readonly ITemplateRepo _templateRepo;
    private readonly IMapper _mapper;
    public CategoryService(ICategoryRepo categoryRepo, IMapper mapper, ICollectionRepo collectionRepo, ITemplateRepo templateRepo)
    {
        _categoryRepo = categoryRepo;
        _mapper = mapper;
        _collectionRepo = collectionRepo;
        _templateRepo = templateRepo;
    }
    public async Task<List<CategoryDTO>> GetAllCategories()
    {
        var result = await _categoryRepo.GetAllCategoryAsync();
        var categories = _mapper.Map<List<CategoryDTO>>(result);
        foreach (var category in categories)
        {
            var tempCollections = new List<CollectionDTO>();
            foreach (var collection in category.Collections)
            {
                var collectionEntity = await _collectionRepo.GetByIDAsync(collection.Id);
                var collectionDto = _mapper.Map<CollectionDTO>(collectionEntity);
                tempCollections.Add(collectionDto);
            }
            category.Collections = tempCollections;
        }
        return categories;
    }
    public async Task<CategoryDTO> GetCategoryById(int id)
    {
        var result = await _categoryRepo.GetByIDAsync(id);
        if (result == null) return null;
        var categories = _mapper.Map<CategoryDTO>(result);
    
        var tempCollections = new List<CollectionDTO>();
        foreach (var item in categories.Collections)
        {
            var collection = await _collectionRepo.GetByIDAsync(item.Id);
            var collections = _mapper.Map<CollectionDTO>(collection);
            tempCollections.Add(collections);
            if (collections == null) return null;
            foreach (var x in collection.CollectionTemplates)
            {
                var template = await _templateRepo.GetByIDAsync(x.TemplateId.Value);
                var templates = _mapper.Map<TemplateDTO>(template);
                collections.TemplateDTO.Add(templates);
            }
        }
        categories.Collections = tempCollections; 
    
        return categories;
    }

    public async Task<CategoryDTO?> UpdateCategory(int id,CategoryDTO category)
    {
        var result = await _categoryRepo.GetByIDAsync(id);
        if(result == null) return null;
        category.Id = result.Id;
        category.Name = result.Name;
        category.ImageUrl = result.ImageUrl;
        var update = await _categoryRepo.UpdateAsync(result);
        if (update)
        {
            return _mapper.Map<CategoryDTO>(result);
        }
        else
        {
            return null;
            
        }
    }

    public async Task<bool> DeleteCategory(int id)
    {
        var result = await _categoryRepo.GetByIDAsync(id);
        var deleted = result != null && await _categoryRepo.DeleteAsync(result);
        if (deleted)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public async Task<CategoryDTO?> CreateCategory(CategoryDTO category)
    {
        var result = _mapper.Map<Category>(category);
        var create = await _categoryRepo.InsertAsync(result);
        if (create)
        {
            return _mapper.Map<CategoryDTO>(result);
        }
        else
        {
            return null;
        }
    }
}
