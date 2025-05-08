using Application.Common.Results;
using Application.DTOs.Response;


namespace Application.Interfaces
{
    public interface IXenforoAuthService
    {
      Task<Result<XenforoAuthResponseDTO>> AuthenticateUserAsync(string username, string password, string clientIp);
    }
}
