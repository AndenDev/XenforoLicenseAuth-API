
namespace Application.Interfaces.Xenforo
{
    public interface IApplicationRepository
    {
        Task<Domain.Entities.Application?> GetByIdAsync(uint id);
        Task<IEnumerable<Domain.Entities.Application>> GetAllAsync();
        Task<uint> AddAsync(Domain.Entities.Application app);
        Task<bool> UpdateAsync(Domain.Entities.Application app);
        Task<bool> DeleteAsync(uint id);
    }
}
