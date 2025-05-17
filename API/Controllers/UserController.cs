using Application.Common.Responses;
using AutoMapper;
using CorrelationId.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using API.ApiRoutes;
using Application.Features.XenforoUser.GetUserProfile;
using API.Attributes;
using Shared.Constant;


namespace API.Controllers
{
    [ApiController]
    [XenForoAuthorize(XenForoUserGroups.Admin)]
    public class UserController : BaseController
    {
        private readonly IMediator _mediator;

        public UserController(
            IMediator mediator,
            IMapper mapper,
            ICorrelationContextAccessor correlationContextAccessor
        ) : base(mapper, correlationContextAccessor)
        {
            _mediator = mediator;
        }

        [HttpPost(UserRoutes.Profile)]
        public async Task<ActionResult<ApiResponse<GetUserProfileViewModel>>> GetUserProfile(int userId)
        {
            var result = await _mediator.Send(new GetUserProfileQuery { UserId = userId });
            return HandleResponse(result);
        }
    }
}
