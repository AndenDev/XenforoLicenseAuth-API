using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces.Xenforo
{
    public interface IHwidResetRequestRepository
    {
        Task AddRequestAsync(HwidResetRequest request);
        Task<IEnumerable<HwidResetRequest>> GetPendingAsync();
        Task ApproveAsync(uint id);
        Task DenyAsync(uint id);
    }
}
