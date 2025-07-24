using Backend.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface ICategoryService
{
    Task<List<Category>> GetAllCategory();
    Task<Category?> GetCategoryById(int id);
    Task<Category> AddCategory(Category category);
    Task<Category?> UpdateCategory(Category category);
    Task<bool> DeleteCategory(int id);
}
