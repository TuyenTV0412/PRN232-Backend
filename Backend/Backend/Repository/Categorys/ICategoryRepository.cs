using Backend.Model;
namespace Backend.Repository.Categorys
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetAllCategory();
    Task<Category?> GetCategoryById(int id);
    Task<Category> AddCategory(Category category);
    Task<Category?> UpdateCategory(Category category);
    Task<bool> DeleteCategory(int id);

    }
}
