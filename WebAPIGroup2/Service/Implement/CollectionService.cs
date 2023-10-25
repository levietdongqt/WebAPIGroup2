using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebAPIGroup2.Models;
using WebAPIGroup2.Models.DTO;
using WebAPIGroup2.Models.POJO;
using WebAPIGroup2.Respository.Inteface;
using WebAPIGroup2.Service.Inteface;

namespace WebAPIGroup2.Service.Implement;

public class CollectionService : ICollectionService
{
    private readonly IMapper _mapper;
    private readonly ICollectionRepo _collectionRepo;
    private readonly ICategoryRepo _categoryRepo;
    private readonly MyImageContext _context;
    public CollectionService( IMapper mapper, ICollectionRepo collectionRepo, MyImageContext context,ICategoryRepo categoryRepo)
    {
        _mapper = mapper;
        _collectionRepo = collectionRepo;
        _context = context;
        _categoryRepo = categoryRepo;
    }
    public async Task<IEnumerable<CollectionDTO>?> GetAllasync()
    {
        var collections = await _collectionRepo.GetAllAsync();
        var mappedCollections = _mapper.Map<IEnumerable<CollectionDTO>>(collections);
        return mappedCollections;
    }

    public async Task<CollectionDTO?> GetCollectionById(int id)
    {
        var collection = await _collectionRepo.GetByIDAsync(id);
        var mappedCollection = _mapper.Map<CollectionDTO>(collection);
        return mappedCollection;
    }

    public async Task<PaginationDTO<CollectionWithTemplateDTO>> GetCollectionWithTemplate(int id,int page = 1,int limit = 1,bool status = true)
    {
        var collection = await _collectionRepo.GetCollectionWithTemplate(id,page,limit,status);
        return _mapper.Map<PaginationDTO<CollectionWithTemplateDTO>>(collection);
    }

    public async Task<bool> UpdateCollection(CollectionDTO collection)
    {
        var getCollection = await _context.Collections.SingleOrDefaultAsync(x => x.Id == collection.Id);
        if (getCollection != null)
        {
            getCollection.Id = collection.Id;
            getCollection.Name = collection.Name;
            var updatedCategory = await _collectionRepo.UpdateAsync(getCollection);
            return updatedCategory;
        }
        return false;
    }

    public async Task<bool> DeleteCollection(int id)
    {
        var getCollection = await _context.Collections.SingleOrDefaultAsync(x=> x.Id == id);
        if (getCollection == null)
        {
            return false;
        }
        var deletedCollection = await _collectionRepo.DeleteAsync(getCollection);
        return deletedCollection;
    }

    public async Task<CollectionDTO> CreateCollection(CollectionDTO collection)
    {
        var mappedCollection = _mapper.Map<Collection>(collection);
        var createCollection = await _collectionRepo.InsertAsync(mappedCollection);
        if (createCollection)
        {
            var collectionDto = _mapper.Map<CollectionDTO>(mappedCollection);
            return collectionDto;
        }
        return null;
    }

    public async Task<IEnumerable<CollectionDTO>?> getAllfeatures()
    {
        var collections = await _collectionRepo.getCollectionFeatures();
        var mapped = _mapper.Map<List<CollectionDTO>>(collections);
        return mapped;
    }
}