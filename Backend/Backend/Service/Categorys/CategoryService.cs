using Backend.Model;
using Backend.Repository.Categorys;

namespace Backend.Service.Categorys
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        public async Task<List<Category>> GetAllCategory()
        {
            return await _categoryRepository.GetAllCategory();
        }
    }
}
