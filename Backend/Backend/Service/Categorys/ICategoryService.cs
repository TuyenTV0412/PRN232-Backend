using Backend.Model;

namespace Backend.Service.Categorys
{
    public interface ICategoryService
    {
        Task<List<Category>> GetAllCategory();
    }
}
