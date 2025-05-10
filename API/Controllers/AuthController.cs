using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using API.ApiRoutes;
using Application.DTOs.Request;
using Application.DTOs.Response;
using AutoMapper;
using CorrelationId.Abstractions;
using Application.Common.Responses;

namespace API.Controllers
{
    [ApiController]
    public class AuthController : BaseController
    {
        private readonly IXenforoAuthService _authService;

        public AuthController(
            IXenforoAuthService authService,
            IMapper mapper,
            ICorrelationContextAccessor correlationContextAccessor
        ) : base(mapper, correlationContextAccessor)
        {
            _authService = authService;
        }

        [HttpPost(AuthRoutes.Login)]
        public async Task<ActionResult<ApiResponse<XenforoAuthResponseDTO>>> Login([FromBody] LoginRequestDTO request)
        {
            var result = await _authService.AuthenticateUserAsync(
                request.Username, request.Password, request.IPAddress);

            return HandleResponse(result);
        }

        [HttpPost(AuthRoutes.Logout)]
        public async Task<ActionResult<ApiResponse<object>>> Logout([FromBody] LogoutRequestDTO request)
        {
            var result = await _authService.LogoutAsync(request.SessionId);
            return HandleResponse(result);
        }

        [HttpPost(AuthRoutes.ValidateSession)]
        public async Task<ActionResult<ApiResponse<XenforoAuthResponseDTO>>> ValidateSession([FromBody] ValidateSessionRequestDTO request)
        {
            var result = await _authService.ValidateSessionAsync(request.SessionId, request.IPAddress);
            return HandleResponse(result);
        }
    }
}
