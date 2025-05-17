using Application.Common.Responses;
using AutoMapper;
using CorrelationId.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using API.ApiRoutes;
using Application.Features.Auth.Login.Command;
using Application.Features.Auth.Logout.Command;
using Application.Features.Auth.ValidateSession.Command;

namespace API.Controllers
{
    [ApiController]
    public class AuthController : BaseController
    {
        private readonly IMediator _mediator;

        public AuthController(
            IMediator mediator,
            IMapper mapper,
            ICorrelationContextAccessor correlationContextAccessor
        ) : base(mapper, correlationContextAccessor)
        {
            _mediator = mediator;
        }

        [HttpPost(AuthRoutes.Login)]
        public async Task<ActionResult<ApiResponse<LoginViewModel>>> Login([FromBody] LoginCommand command)
        {
            var result = await _mediator.Send(command); 
            return HandleResponse(result);             
        }


        [HttpPost(AuthRoutes.Logout)]
        public async Task<ActionResult<ApiResponse<LogoutViewModel>>> Logout([FromBody] LogoutCommand command)
        {
            var result = await _mediator.Send(command); 
            return HandleResponse(result);              
        }


        [HttpPost(AuthRoutes.ValidateSession)]
        public async Task<ActionResult<ApiResponse<ValidateSessionViewModel>>> ValidateSession([FromBody] ValidateSessionCommand command)
        {
            var result = await _mediator.Send(command);
            return HandleResponse(result);
        }

    }
}
