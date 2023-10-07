using AutoMapper;
using WebAPIGroup2.Models.DTO;
using WebAPIGroup2.Models.POJO;

namespace WebAPIGroup2.Models
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            //User
            CreateMap<User, UserDTO>()
                .ForMember(t => t.Password, option => option.Ignore()).ReverseMap();

            //Template
            CreateMap<Template,TemplateDTO>().ReverseMap();
            CreateMap<Template, AddTemplateDTO>().ReverseMap();


            //DescriptionTemplate
            CreateMap<DescriptionTemplate,DescriptionTemplateDTO>().ReverseMap();

            //Template Image
            CreateMap<TemplateImage,TemplateImageDTO>().ReverseMap();
            
            CreateMap<CollectionTemplate, CollectionTemplateDTO>().ReverseMap();
            CreateMap<PrintSize, SizeDTO>().ReverseMap();
            CreateMap<TemplateSize,TemplateSizeDTO>().ReverseMap();
            CreateMap<MaterialPage, MaterialPageDTO>().ReverseMap();
            CreateMap<DeliveryInfo, DeliveryInfoDTO>().ReverseMap();
            CreateMap<ContentEmail, ContentEmailDTO>().ReverseMap();
            CreateMap<Review, ReviewDTO>().ReverseMap();
            CreateMap<Review, AddReviewDTO>().ReverseMap();
            CreateMap<Collection,CollectionDTO>().ReverseMap(); 
            CreateMap<Category, CategoryDTO>().ReverseMap();
        }
    }
}
