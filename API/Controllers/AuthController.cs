using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using API.ApiRoutes;
using Application.DTOs.Request;
using Application.DTOs.Response;
using API.Attributes;
using API.Helpers;

namespace API.Controllers
{
    [ApiController]

    public class AuthController : ControllerBase
    {
        private readonly IXenforoAuthService _authService;

        public AuthController(IXenforoAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost(AuthRoutes.Login)]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO request)
        {
            var result = await _authService.AuthenticateUserAsync(request.Username, request.Password, request.IPAddress);
            return ResponseHandler.HandleResponse(result);
        }
    }
}
