using System;
using System.Collections.Generic;
using System.Text;
using TourPlanner.Dal.Interfaces;
using TourPlanner.Models;

namespace TourPlanner.Bll.Interfaces
{
    public interface IContactService
    {
        Task<List<Contact>> GetAllAsync();

        Task<Contact?> GetByIdAsync(int id);

        Task<Contact?> CreateAsync(Contact contact);

        Task<bool> UpdateByIdAsync(int id, Contact contact);

        Task<bool> DeleteByIdAsync(int id);
    }
}
