using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces.Xenforo
{
    public interface IClientBuildRepository
    {
        Task<IEnumerable<ClientBuild>> GetAllAsync();
        Task<IEnumerable<ClientBuild>> GetByApplicationIdAsync(uint appId);
        Task<uint> AddAsync(ClientBuild build);
        Task<bool> UpdateAsync(ClientBuild build);
        Task<bool> DeleteAsync(uint id);
    }
}
