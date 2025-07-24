using Backend.Model;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class CardController : ControllerBase
{
    private readonly ICardService _cardService;
    public CardController(ICardService cardService)
    {
        _cardService = cardService;
    }

    // Kiểm tra user đã có thẻ chưa (theo username)
    [HttpGet("HasCard/{username}")]
    public async Task<ActionResult<bool>> HasCard(string username)
    {
        var hasCard = await _cardService.HasCardByUserId(username);
        return Ok(hasCard);
    }

    // Lấy thông tin thẻ theo username
    [HttpGet("User/{username}")]
    public async Task<ActionResult<Card>> GetCardByUserId(string username)
    {
        var card = await _cardService.GetCardByUserId(username);
        if (card == null)
            return NotFound();
        return Ok(card);
    }

    // Lấy thông tin thẻ theo personId
    [HttpGet("Person/{personId}")]
    public async Task<ActionResult<Card>> GetCardByPersonId(int personId)
    {
        var card = await _cardService.GetCardByPersonId(personId);
        if (card == null)
            return NotFound();
        return Ok(card);
    }

    // Tạo thẻ mới theo personId
    [HttpPost("CreateByPersonId")]
    public async Task<ActionResult<Card>> CreateCard([FromBody] int personId)
    {
        try
        {
            var card = await _cardService.CreateCard(personId);
            return CreatedAtAction(nameof(GetCardByPersonId), new { personId = personId }, card);
        }
        catch (System.Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

}
