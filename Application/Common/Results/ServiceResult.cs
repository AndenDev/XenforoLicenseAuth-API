using Application.Common.Errors;
using Domain.Enums;

namespace Application.Common.Results
{
    /// <summary>
    /// Encapsulates the result of a service operation, including success state, data, and error information.
    /// </summary>
    /// <typeparam name="T">The type of data returned by the service operation.</typeparam>
    public class ServiceResult<T>
    {
        /// <summary>
        /// Indicates whether the service operation was successful.
        /// </summary>
        public bool IsSuccess { get; private set; }

        /// <summary>
        /// The data returned by the service operation (if successful or partially successful).
        /// </summary>
        public T? Data { get; private set; }

        /// <summary>
        /// The type of error if the operation failed.
        /// </summary>
        public ErrorType ErrorType { get; private set; } = ErrorType.None;

        /// <summary>
        /// Detailed error information (if any).
        /// </summary>
        public ApplicationError? Error { get; private set; }

        /// <summary>
        /// Optional success message that can be returned alongside successful results.
        /// </summary>
        public string? GetSuccessMessage { get; private set; }

        // ---- Factory Methods ---- //

        /// <summary>
        /// Creates a successful result.
        /// </summary>
        public static ServiceResult<T> Success(T data, string? successMessage = null)
        {
            return new ServiceResult<T>
            {
                IsSuccess = true,
                Data = data,
                GetSuccessMessage = successMessage
            };
        }

        /// <summary>
        /// Creates a failed result with a specified error type and detailed error.
        /// </summary>
        public static ServiceResult<T> Failure(ErrorType errorType, ApplicationError error, T? data = default)
        {
            return new ServiceResult<T>
            {
                IsSuccess = false,
                ErrorType = errorType,
                Error = error,
                Data = data
            };
        }

        /// <summary>
        /// Creates a user error result (helper method).
        /// </summary>
        public static ServiceResult<T> UserError(ApplicationError error, T? data = default)
            => Failure(ErrorType.User, error, data);

        /// <summary>
        /// Creates a not-found error result (helper method).
        /// </summary>
        public static ServiceResult<T> NotFound(ApplicationError error)
            => Failure(ErrorType.NotFound, error);

        /// <summary>
        /// Creates a conflict error result (helper method).
        /// </summary>
        public static ServiceResult<T> Conflict(ApplicationError error)
            => Failure(ErrorType.Conflict, error);

        /// <summary>
        /// Creates an exception error result (helper method).
        /// </summary>
        public static ServiceResult<T> Exception(ApplicationError error)
            => Failure(ErrorType.Exception, error);

        /// <summary>
        /// Creates a forbidden error result (helper method).
        /// </summary>
        public static ServiceResult<T> Forbidden(ApplicationError error)
            => Failure(ErrorType.Forbidden, error);
    }
}
