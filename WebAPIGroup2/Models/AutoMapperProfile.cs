using AutoMapper;
using WebAPIGroup2.Models.DTO;
using WebAPIGroup2.Models.POJO;

namespace WebAPIGroup2.Models
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserDTO>()
                .ForMember(t => t.Password, option => option.Ignore()).ReverseMap();
            CreateMap<Category, CategoryDTO>()
                .ForMember(t => t.Id, opt => opt.MapFrom(src => src.Id)) 
                .ForMember(t => t.Name, opt => opt.MapFrom(src => src.Name))
                .ReverseMap();
            CreateMap<Template, TemplateDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.PricePlus, opt => opt.MapFrom(src => src.PricePlus))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.QuantitySold, opt => opt.MapFrom(src => src.QuantitySold))
                .ForMember(dest => dest.CreateDate, opt => opt.MapFrom(src => src.CreateDate))
                .ForMember(dest => dest.CategoryIds, opt => opt.MapFrom(src => src.CategoryTemplates.Select(ct => ct.CategoryId).ToList()))
                // Các thuộc tính khác của TemplateDTO
                .ReverseMap();
            CreateMap<TemplateDTO,TemplateWithCategoryDTO>()
                .ReverseMap();
            CreateMap<CategoryTemplate, CategoryDTO>().ReverseMap();
            CreateMap<PrintSize,SizeDTO>().ReverseMap();
        }
    }
}
