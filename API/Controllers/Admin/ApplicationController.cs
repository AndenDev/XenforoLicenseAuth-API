using API.ApiRoutes;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Constant;
using Application.DTOs.Response;
using Application.Features.Xenforo.Application.Command;
using Application.Features.Xenforo.Application.Queries;

namespace API.Controllers.Admin
{
    [ApiController]
    [Authorize(Policy = UserGroupPolicies.AdminOnly)]
    public class ApplicationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ApplicationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet(ApplicationRoutes.Admin.GetAll)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<Domain.Entities.Application>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllApplications()
        {
            var result = await _mediator.Send(new GetAllApplicationsQuery());
            return ApiResponse<object>.Ok(result, "All applications retrieved.");
        }

        [HttpGet(ApplicationRoutes.Admin.GetById)]
        public async Task<IActionResult> GetApplicationById(uint id)
        {
            var result = await _mediator.Send(new GetApplicationByIdQuery(id));
            if (result == null)
                return ApiResponse<object>.NotFound($"Application with ID '{id}' not found.");

            return ApiResponse<object>.Ok(result, "Application retrieved.");
        }

        [HttpPost(ApplicationRoutes.Admin.Add)]
        public async Task<IActionResult> AddApplication([FromBody] AddApplicationCommand command)
        {
            var createdApp = await _mediator.Send(command);
            return ApiResponse<object>.Ok(createdApp, "Application added successfully.");
        }

        [HttpPut(ApplicationRoutes.Admin.Update)]
        public async Task<IActionResult> UpdateApplication(uint id, [FromBody] UpdateApplicationCommand command)
        {
            command.Id = id;
            var updatedApp = await _mediator.Send(command);

            if (updatedApp == null)
                return ApiResponse<object>.NotFound($"Application with ID {id} not found.");

            return ApiResponse<object>.Ok(updatedApp, "Application updated successfully.");
        }

        [HttpDelete(ApplicationRoutes.Admin.Delete)]
        public async Task<IActionResult> DeleteApplication(uint id)
        {
            var success = await _mediator.Send(new DeleteApplicationCommand(id));
            if (success == null)
                return ApiResponse<object>.NotFound($"Application with ID {id} not found or could not be deleted.");

            return ApiResponse<object>.Ok(null, "Application deleted successfully.");
        }
    }
}
