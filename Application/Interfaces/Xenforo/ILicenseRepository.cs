using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces.Xenforo
{
    public interface ILicenseRepository
    {
        Task<License?> GetByKeyAsync(string licenseKey);
        Task<IEnumerable<License>> GetByUserIdAsync(uint userId);
        Task<IEnumerable<License>> GetAllAsync();
        Task AddAsync(License license);
        Task UpdateAsync(License license);
        Task DeleteAsync(uint id);
    }
}
