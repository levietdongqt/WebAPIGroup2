using WebAPIGroup2.Models.POJO;

namespace WebAPIGroup2.Respository.Inteface
{
    public interface IImageRepo : IBaseRepository<Image, int>
    {
        List<Image> getByIdList(List<int> list);
    }
}
