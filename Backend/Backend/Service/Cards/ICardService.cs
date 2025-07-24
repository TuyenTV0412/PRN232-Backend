using Backend.DTO;
using Backend.Model;
using System.Threading.Tasks;

public interface ICardService
{
    Task<bool> HasCardByUserId(string username);
    Task<Card?> GetCardByUserId(string username);
    Task<Card?> GetCardByPersonId(int personId);
    Task<Card> CreateCard(int personId);

   
}
