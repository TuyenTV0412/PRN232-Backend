using Backend.Model;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Backend.Repository.Cards
{
    public class CardRepository : ICardRepository
    {
        private readonly Prn232Context _context;
        public CardRepository(Prn232Context context)
        {
            _context = context;
        }

        public async Task<bool> HasCardByUserId(string username)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null) return false;
            return await _context.Cards.AnyAsync(c => c.PersonId == user.PersonId);
        }

        public async Task<Card?> GetCardByUserId(string username)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null) return null;
            return await _context.Cards
                .Include(c => c.Person)
                .FirstOrDefaultAsync(c => c.PersonId == user.PersonId);
        }

        public async Task<Card?> GetCardByPersonId(int personId)
        {
            return await _context.Cards
                .Include(c => c.Person)
                .FirstOrDefaultAsync(c => c.PersonId == personId);
        }

        public async Task<Card> CreateCard(Card card)
        {
            _context.Cards.Add(card);
            await _context.SaveChangesAsync();
            return card;
        }
    }
}
