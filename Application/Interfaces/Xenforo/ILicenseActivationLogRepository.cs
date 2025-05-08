using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces.Xenforo
{
    public interface ILicenseActivationLogRepository
    {
        Task AddAsync(LicenseActivationLog log);
        Task<IEnumerable<LicenseActivationLog>> GetByLicenseIdAsync(uint licenseId);
        Task<IEnumerable<LicenseActivationLog>> GetByUserIdAsync(uint userId);
    }
}
