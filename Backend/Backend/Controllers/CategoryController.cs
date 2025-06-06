using Backend.Model;
using Backend.Repository.Categorys;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : Controller
    {

        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        [HttpGet]
        public async Task<ActionResult<List<Category>>> GetAllCategory()
        {
            var category = await _categoryRepository.GetAllCategory();
            return Ok(category);
        }
    }
}
