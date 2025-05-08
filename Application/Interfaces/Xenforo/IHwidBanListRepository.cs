using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces.Xenforo
{
    public interface IHwidBanListRepository
    {
        Task<bool> IsBannedAsync(string hwid);
        Task AddBanAsync(string hwid, string? reason, string? bannedByUsername);
        Task RemoveBanAsync(string hwid);
        Task<IEnumerable<HwidBanEntry>> GetAllBansAsync();
    }
}
