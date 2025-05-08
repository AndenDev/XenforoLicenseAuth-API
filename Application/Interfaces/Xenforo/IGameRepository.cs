
using Domain.Entities;

namespace Application.Interfaces.Xenforo
{
    public interface IGameRepository
    {
        Task<Game?> GetByIdAsync(uint id);
        Task<IEnumerable<Game>> GetAllAsync();
        Task<uint> Addsync(Game game);
        Task<bool> UpdateAsync(Game game);
        Task<bool> DeleteAsync(uint id);
    }
}
