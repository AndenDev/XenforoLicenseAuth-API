
using Domain.Entities;

namespace Application.Interfaces.Xenforo
{
    public interface ILicensePlanRepository
    {
        Task<LicensePlan?> GetByIdAsync(uint id);
        Task<IEnumerable<LicensePlan>> GetAllAsync();
    }
}
