using Backend.Model;
using Backend.Service.Authors;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
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
    }
}
