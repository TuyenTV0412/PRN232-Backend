using Backend.Model;

namespace Backend.Service.Publishers
{
    public interface IPublisherService
    {
        Task<List<Publisher>> GetAllPublisher();
        Task<Publisher?> GetPublisherById(int id);
        Task<Publisher> AddPublisher(Publisher publisher);
        Task<Publisher?> UpdatePublisher(Publisher publisher);
        Task<bool> DeletePublisher(int id);
    }
}
