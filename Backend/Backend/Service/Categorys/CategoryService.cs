using Backend.Model;
using Backend.Repository.Categorys;
using System.Collections.Generic;
using System.Threading.Tasks;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public Task<List<Category>> GetAllCategory() => _categoryRepository.GetAllCategory();

    public Task<Category?> GetCategoryById(int id) => _categoryRepository.GetCategoryById(id);

    public Task<Category> AddCategory(Category category) => _categoryRepository.AddCategory(category);

    public Task<Category?> UpdateCategory(Category category) => _categoryRepository.UpdateCategory(category);

    public Task<bool> DeleteCategory(int id) => _categoryRepository.DeleteCategory(id);
}
