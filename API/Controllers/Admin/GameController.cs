using API.ApiRoutes;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Constant;
using Application.DTOs.Response;
using Application.Features.Xenforo.Game.Command;
using Application.Features.Xenforo.Game.Queries;

namespace API.Controllers.Admin
{
    [ApiController]
    [Authorize(Policy = UserGroupPolicies.AdminOnly)]
    public class GameController : ControllerBase
    {
        private readonly IMediator _mediator;

        public GameController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet(GameRoutes.Admin.GetAll)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<Domain.Entities.Game>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllGames()
        {
            var result = await _mediator.Send(new GetAllGamesQuery());
            return ApiResponse<object>.Ok(result, "All games retrieved.");
        }

        [HttpGet(GameRoutes.Admin.GetById)]
        public async Task<IActionResult> GetGameById(uint id)
        {
            var result = await _mediator.Send(new GetGameByIdQuery(id));
            if (result == null)
                return ApiResponse<object>.NotFound($"Game with ID '{id}' not found.");

            return ApiResponse<object>.Ok(result, "Game retrieved.");
        }

        [HttpPost(GameRoutes.Admin.Add)]
        public async Task<IActionResult> AddGame([FromBody] AddGameCommand command)
        {
            var createdGame = await _mediator.Send(command);
            return ApiResponse<object>.Ok(createdGame, "Game added successfully.");
        }

        [HttpPut(GameRoutes.Admin.Update)]
        public async Task<IActionResult> UpdateGame(uint id, [FromBody] UpdateGameCommand command)
        {
            command.Id = id;
            var updatedGame = await _mediator.Send(command);

            if (updatedGame == null)
                return ApiResponse<object>.NotFound($"Game with ID {id} not found.");

            return ApiResponse<object>.Ok(updatedGame, "Game updated successfully.");
        }

        [HttpDelete(GameRoutes.Admin.Delete)]
        public async Task<IActionResult> DeleteGame(uint id)
        {
            var success = await _mediator.Send(new DeleteGameCommand(id));
            if (success == null)
                return ApiResponse<object>.NotFound($"Game with ID {id} not found or could not be deleted.");

            return ApiResponse<object>.Ok(null, "Game deleted successfully.");
        }
    }
}
