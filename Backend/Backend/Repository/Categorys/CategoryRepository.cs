using Backend.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        public async Task<Category?> GetCategoryById(int id)
        {
            return await _context.Categories.FindAsync(id);
        }

        public async Task<Category> AddCategory(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<Category?> UpdateCategory(Category category)
        {
            var existing = await _context.Categories.FindAsync(category.CategoryId);
            if (existing == null) return null;
            existing.CategoryName = category.CategoryName;
            // Cập nhật các trường khác nếu có
            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return false;
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
