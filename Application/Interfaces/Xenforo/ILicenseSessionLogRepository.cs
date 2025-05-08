using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces.Xenforo
{
    public interface ILicenseSessionLogRepository
    {
        Task AddAsync(LicenseSessionLog log);
        Task EndSessionAsync(string sessionId, string endedReason);
        Task<IEnumerable<LicenseSessionLog>> GetByLicenseIdAsync(uint licenseId);
        Task<IEnumerable<LicenseSessionLog>> GetActiveSessionsAsync();
    }
}
