using Backend.Model;
using Backend.Repository.Publishers;
using Backend.Service.Publishers;
using System.Collections.Generic;
using System.Threading.Tasks;

public class PublisherService : IPublisherService
{
    private readonly IPublisherRepository _publisherRepository;

    public PublisherService(IPublisherRepository publisherRepository)
    {
        _publisherRepository = publisherRepository;
    }

    public Task<List<Publisher>> GetAllPublisher() => _publisherRepository.GetAllPublisher();

    public Task<Publisher?> GetPublisherById(int id) => _publisherRepository.GetPublisherById(id);

    public Task<Publisher> AddPublisher(Publisher publisher) => _publisherRepository.AddPublisher(publisher);

    public Task<Publisher?> UpdatePublisher(Publisher publisher) => _publisherRepository.UpdatePublisher(publisher);

    public Task<bool> DeletePublisher(int id) => _publisherRepository.DeletePublisher(id);
}
