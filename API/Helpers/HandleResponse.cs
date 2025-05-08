using Application.Common.Results;
using Application.DTOs.Response;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Helpers
{
    public static class ResponseHandler
    {
        public static IActionResult HandleResponse<T>(Result<T> result)
        {
            var statusCode = result.Success
                ? StatusCodes.Status200OK
                : (int)(result.Error?.StatusCode ?? HttpStatusCode.BadRequest);

            return new ObjectResult(result) { StatusCode = statusCode };
        }
    }
}
