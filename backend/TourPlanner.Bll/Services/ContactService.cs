using TourPlanner.Bll.Interfaces;
using TourPlanner.Dal.Interfaces;
using TourPlanner.Models;

namespace TourPlanner.Bll.Services
{
    public class ContactService : IContactService
    {
        private static Dictionary<int, Contact> _contacts = new Dictionary<int, Contact>();
        private static int idCount = 0;

        public async Task<List<Contact>> GetAllAsync()
        {
            return _contacts.Values.ToList();
        }

        public async Task<Contact?> GetByIdAsync(int id)
        {
            _contacts.TryGetValue(id, out var contact);
            return contact;
        }

        public async Task<Contact?> CreateAsync(Contact contact)
        {
            contact.Id = idCount++;
            _contacts.TryAdd(contact.Id, contact);
            return contact;
        }

        public async Task<bool> UpdateByIdAsync(int id, Contact contact)
        {
            _contacts.TryGetValue(id, out var _contact);
            if (_contact == null)
            {
                return false;
            }

            _contacts[id] = contact;
            return true;
        }

        public async Task<bool> DeleteByIdAsync(int id)
        {
            return _contacts.Remove(id);
        }
    }
}
