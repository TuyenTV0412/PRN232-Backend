using Backend.Model;
using Backend.Service.Publishers;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class PublisherController : ControllerBase
{
    private readonly IPublisherService _publisherService;
    public PublisherController(IPublisherService publisherService)
    {
        _publisherService = publisherService;
    }

    [HttpGet]
    public async Task<ActionResult<List<Publisher>>> GetAll()
    {
        var publishers = await _publisherService.GetAllPublisher();
        return Ok(publishers);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Publisher>> GetById(int id)
    {
        var publisher = await _publisherService.GetPublisherById(id);
        if (publisher == null) return NotFound();
        return Ok(publisher);
    }

    [HttpPost]
    public async Task<ActionResult<Publisher>> Create([FromBody] Publisher publisher)
    {
        var created = await _publisherService.AddPublisher(publisher);
        return CreatedAtAction(nameof(GetById), new { id = created.PublisherId }, created);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Publisher>> Update(int id, [FromBody] Publisher publisher)
    {
        if (id != publisher.PublisherId) return BadRequest();
        var updated = await _publisherService.UpdatePublisher(publisher);
        if (updated == null) return NotFound();
        return Ok(updated);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var deleted = await _publisherService.DeletePublisher(id);
        if (!deleted) return NotFound();
        return NoContent();
    }
}
