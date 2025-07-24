using Backend.Model;
using Backend.Repository.Cards;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

public class CardService : ICardService
{
    private readonly ICardRepository _cardRepository;
    private readonly Prn232Context _context;

    public CardService(ICardRepository cardRepository, Prn232Context context)
    {
        _cardRepository = cardRepository;
        _context = context;
    }

    public Task<bool> HasCardByUserId(string username)
        => _cardRepository.HasCardByUserId(username);

    public Task<Card?> GetCardByUserId(string username)
        => _cardRepository.GetCardByUserId(username);

    public Task<Card?> GetCardByPersonId(int personId)
        => _cardRepository.GetCardByPersonId(personId);

    public async Task<Card> CreateCard(int personId)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.PersonId == personId);
        if (user == null)
            throw new Exception("Người dùng không tồn tại, không thể tạo thẻ!");

        var existingCard = await _cardRepository.GetCardByPersonId(personId);
        if (existingCard != null)
            throw new Exception("Người dùng này đã có thẻ, không thể tạo thêm!");

        var today = DateOnly.FromDateTime(DateTime.Today);
        var validTo = today.AddYears(4);

        var card = new Card
        {
            PersonId = user.PersonId,
            ValidFrom = today,
            ValidThru = validTo
        };

        return await _cardRepository.CreateCard(card);
    }


}
