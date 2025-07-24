using Backend.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.Repository.Publishers
{
    public class PublisherRepository : IPublisherRepository
    {
        private readonly Prn232Context _context;

        public PublisherRepository(Prn232Context context)
        {
            _context = context;
        }

        public async Task<List<Publisher>> GetAllPublisher()
        {
            return await _context.Publishers.ToListAsync();
        }

        public async Task<Publisher?> GetPublisherById(int id)
        {
            return await _context.Publishers.FindAsync(id);
        }

        public async Task<Publisher> AddPublisher(Publisher publisher)
        {
            _context.Publishers.Add(publisher);
            await _context.SaveChangesAsync();
            return publisher;
        }

        public async Task<Publisher?> UpdatePublisher(Publisher publisher)
        {
            var existing = await _context.Publishers.FindAsync(publisher.PublisherId);
            if (existing == null) return null;
            existing.PublisherName = publisher.PublisherName;
            existing.Address = publisher.Address;
            existing.Website = publisher.Website;
            existing.Email = publisher.Email;
            existing.Phone = publisher.Phone;
            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeletePublisher(int id)
        {
            var publisher = await _context.Publishers.FindAsync(id);
            if (publisher == null) return false;
            _context.Publishers.Remove(publisher);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
