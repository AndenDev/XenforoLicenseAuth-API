using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Application.Common.Responses
{
    /// <summary>
    /// A standard API response wrapper that unifies successful and error responses.
    /// </summary>
    /// <typeparam name="T">Type of the data being returned.</typeparam>
    public class ApiResponse<T>
    {
        /// <summary>
        /// Indicates if the response is successful.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// The actual data returned by the API (if successful).
        /// </summary>
        public T? Data { get; set; }

        /// <summary>
        /// The error details (if any).
        /// </summary>
        public ProblemDetails? Error { get; set; }

        /// <summary>
        /// The HTTP status code of the response.
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// A general-purpose message, e.g., "Operation completed successfully."
        /// </summary>
        public string? Message { get; set; }

        /// <summary>
        /// Factory method to create a successful response.
        /// </summary>
        public static ApiResponse<T> CreateSuccess(
            T? data,
            string message = "",
            HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            return new ApiResponse<T>
            {
                Success = true,
                Data = data,
                Message = message,
                StatusCode = (int)statusCode
            };
        }

        /// <summary>
        /// Factory method to create an error response.
        /// </summary>
        public static ApiResponse<T> CreateError(
            T? data = default,
            ProblemDetails? error = null,
            HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Data = data,
                Error = error,
                StatusCode = (int)statusCode
            };
        }
    }
}
