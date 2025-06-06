using Backend.Model;
namespace Backend.Repository.Categorys
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetAllCategory();

    }
}
