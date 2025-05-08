using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Application.DTOs.Response
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }

        public ApiResponse(bool success, T? data, string? message = null)
        {
            Success = success;
            Data = data;
            Message = message;
        }

        public static IActionResult Ok(T data, string? message = null)
            => new OkObjectResult(new ApiResponse<T>(true, data, message));

        public static IActionResult Unauthorized(string message = "Unauthorized")
            => new UnauthorizedObjectResult(new ApiResponse<T>(false, default, message));

        public static IActionResult BadRequest(string message = "Bad request")
            => new BadRequestObjectResult(new ApiResponse<T>(false, default, message));

        public static IActionResult NotFound(string message = "Not found")
            => new NotFoundObjectResult(new ApiResponse<T>(false, default, message));

        public static IActionResult Forbidden(string message = "Forbidden")
            => new ObjectResult(new ApiResponse<T>(false, default, message)) { StatusCode = StatusCodes.Status403Forbidden };
    }
}
