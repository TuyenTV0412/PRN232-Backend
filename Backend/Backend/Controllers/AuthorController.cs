using Backend.Model;
using Backend.Service.Authors;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class AuthorController : Controller
{
    private readonly IAuthorService _authorService;
    public AuthorController(IAuthorService authorService)
    {
        _authorService = authorService;
    }

    [HttpGet]
    public async Task<ActionResult<List<Author>>> GetAllAuthors()
    {
        var authors = await _authorService.GetAllAuthor();
        return Ok(authors);
    }

    [HttpGet("Top4Author")]
    public async Task<ActionResult<List<Author>>> GetTop4Author()
    {
        var authors = await _authorService.GetTop4Author();
        return Ok(authors);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Author>> GetAuthorById(int id)
    {
        var author = await _authorService.GetAuthorById(id);
        if (author == null) return NotFound();
        return Ok(author);
    }

    [HttpPost]
    public async Task<ActionResult<Author>> AddAuthor([FromBody] Author author)
    {
        var created = await _authorService.AddAuthor(author);
        return CreatedAtAction(nameof(GetAuthorById), new { id = created.AuthorId }, created);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Author>> UpdateAuthor(int id, [FromBody] Author author)
    {
        if (id != author.AuthorId) return BadRequest();
        var updated = await _authorService.UpdateAuthor(author);
        if (updated == null) return NotFound();
        return Ok(updated);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAuthor(int id)
    {
        var deleted = await _authorService.DeleteAuthor(id);
        if (!deleted) return NotFound();
        return NoContent();
    }


    [HttpGet("Index")]
    public async Task<IActionResult> GetAuthors([FromQuery] int page = 1, [FromQuery] int pageSize = 8, [FromQuery] string? search = null)
    {
        if (page <= 0) page = 1;
        if (pageSize <= 0) pageSize = 8;
        var result = await _authorService.GetAuthorsAsync(page, pageSize, search);
        return Ok(result);
    }
}
