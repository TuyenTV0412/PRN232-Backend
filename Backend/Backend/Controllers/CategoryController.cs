using Backend.Model;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    public async Task<ActionResult<List<Category>>> GetAll()
    {
        var categories = await _categoryService.GetAllCategory();
        return Ok(categories);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Category>> GetById(int id)
    {
        var category = await _categoryService.GetCategoryById(id);
        if (category == null) return NotFound();
        return Ok(category);
    }

    [HttpPost]
    public async Task<ActionResult<Category>> Create([FromBody] Category category)
    {
        var created = await _categoryService.AddCategory(category);
        return CreatedAtAction(nameof(GetById), new { id = created.CategoryId }, created);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Category>> Update(int id, [FromBody] Category category)
    {
        if (id != category.CategoryId) return BadRequest();
        var updated = await _categoryService.UpdateCategory(category);
        if (updated == null) return NotFound();
        return Ok(updated);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var deleted = await _categoryService.DeleteCategory(id);
        if (!deleted) return NotFound();
        return NoContent();
    }
}
