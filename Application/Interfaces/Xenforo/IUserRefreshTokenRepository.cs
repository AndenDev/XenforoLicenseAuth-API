using Domain.Entities;
using System.Threading.Tasks;

namespace Application.Interfaces.Xenforo
{
    public interface IUserRefreshTokenRepository
    {
        Task<UserRefreshToken?> GetByTokenAsync(string refreshToken);
        Task AddAsync(UserRefreshToken token);
        Task DeleteAsync(string refreshToken);
    }
}
