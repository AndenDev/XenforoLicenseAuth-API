using Application.Common.Results;
using Application.DTOs.Response;


namespace Application.Interfaces
{
    public interface IXenforoAuthService
    {
        Task<ServiceResult<XenforoAuthResponseDTO>> AuthenticateUserAsync(string username, string password, string clientIp);

        Task<ServiceResult<Object>> LogoutAsync(string sessionIdHex);

        Task<ServiceResult<XenforoAuthResponseDTO>> ValidateSessionAsync(string sessionIdHex, string clientIp);
    }
}
