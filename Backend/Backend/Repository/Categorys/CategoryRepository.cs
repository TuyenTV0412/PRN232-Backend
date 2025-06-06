using Backend.Model;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repository.Categorys
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly Prn232Context _context;

        public CategoryRepository(Prn232Context context)
        {
            _context = context;
        }
        public async Task<List<Category>> GetAllCategory()
        {
           return await _context.Categories.ToListAsync();
        }
    }
}
