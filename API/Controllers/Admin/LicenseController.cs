using API.ApiRoutes;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Constant;
using Application.DTOs.Response;
using Application.Features.Xenforo.License.Command;
using Application.Features.Xenforo.License.Queries;
using Application.Features.Xenforo.License.Query;

namespace API.Controllers.Admin
{
    [ApiController]
    [Authorize(Policy = UserGroupPolicies.AdminOnly)]
    public class LicenseController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LicenseController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet(LicenseRoutes.Admin.GetAll)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<Domain.Entities.License>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllLicenses()
        {
            var result = await _mediator.Send(new GetAllLicensesQuery());
            return ApiResponse<object>.Ok(result, "All licenses retrieved.");
        }

        [HttpGet(LicenseRoutes.Admin.GetByKey)]
        public async Task<IActionResult> GetLicenseByKey(string key)
        {
            var result = await _mediator.Send(new GetLicenseByKeyQuery(key));
            if (result == null)
                return ApiResponse<object>.NotFound($"License with key '{key}' not found.");

            return ApiResponse<object>.Ok(result, "License retrieved.");
        }

        [HttpGet(LicenseRoutes.Admin.GetByUserId)]
        public async Task<IActionResult> GetLicensesByUserId(uint userId)
        {
            var result = await _mediator.Send(new GetLicensesByUserIdQuery(userId));
            return ApiResponse<object>.Ok(result, $"Licenses for user ID {userId} retrieved.");
        }

        [HttpPost(LicenseRoutes.Admin.Add)]
        public async Task<IActionResult> AddLicense([FromBody] AddLicenseCommand command)
        {
            var createdLicense = await _mediator.Send(command);
            return ApiResponse<object>.Ok(createdLicense, "License added successfully.");
        }

        [HttpPut(LicenseRoutes.Admin.Update)]
        public async Task<IActionResult> UpdateLicense(uint id, [FromBody] UpdateLicenseCommand command)
        {
            command.License.Id = id;
            var updatedLicense = await _mediator.Send(command);

            if (updatedLicense == null)
                return ApiResponse<object>.NotFound($"License with ID {id} not found.");

            return ApiResponse<object>.Ok(updatedLicense, "License updated successfully.");
        }

        [HttpDelete(LicenseRoutes.Admin.Delete)]
        public async Task<IActionResult> DeleteLicense(uint id)
        {
            var success = await _mediator.Send(new DeleteLicenseCommand(id));
            if (success == null)
                return ApiResponse<object>.NotFound($"License with ID {id} not found or could not be deleted.");

            return ApiResponse<object>.Ok(null, "License deleted successfully.");
        }
    }
}
