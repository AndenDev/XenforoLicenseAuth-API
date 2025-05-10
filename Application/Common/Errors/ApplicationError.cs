using System.Diagnostics;
using System.Net;
using Domain.Enums;

namespace Application.Common.Errors
{
    /// <summary>
    /// Represents a structured application error with standard metadata.
    /// </summary>
    public partial class ApplicationError
    {
        public string Title { get; }

        public string ErrorCode { get; }

        public string ErrorDescription { get; }

        public string TraceId { get; }

        public string CorrelationId { get; }

        public ErrorType ErrorType { get; }

        private ApplicationError(string title, ErrorType errorType, string errorCode, string correlationId, string errorDescription)
        {
            Title = title;
            ErrorCode = errorCode;
            ErrorDescription = errorDescription;
            CorrelationId = correlationId;
            TraceId = Activity.Current?.TraceId.ToString() ?? "NoTraceId";
            ErrorType = errorType;
        }

        /// <summary>
        /// Creates a standardized error using <see cref="ErrorDetail"/>.
        /// </summary>
        public static ApplicationError CreateError(
            ErrorType errorType,
            ErrorDetail errorDetail,
            string correlationId,
            params object[] args
        )
        {
            var formattedMessage = string.Format(errorDetail.DefaultMessage, args);

            return new ApplicationError(
                errorDetail.Title,
                errorType,
                errorDetail.Code,
                correlationId,
                formattedMessage
            );
        }
        public static ApplicationError CreateUserError(
            ErrorDetail errorDetail,
            string correlationId,
            params object[] args
        ) => CreateError(ErrorType.User, errorDetail, correlationId, args);

        public static ApplicationError CreateNotFoundError(
            ErrorDetail errorDetail,
            string correlationId,
            params object[] args
        ) => CreateError(ErrorType.NotFound, errorDetail, correlationId, args);

        public static ApplicationError CreateExceptionError(
            ErrorDetail errorDetail,
            string correlationId,
            params object[] args
        ) => CreateError(ErrorType.Exception, errorDetail, correlationId, args);

        public static ApplicationError CreateConflictError(
            ErrorDetail errorDetail,
            string correlationId,
            params object[] args
        ) => CreateError(ErrorType.Conflict, errorDetail, correlationId, args);

        public static ApplicationError CreateForbiddenError(
            ErrorDetail errorDetail,
            string correlationId,
            params object[] args
        ) => CreateError(ErrorType.Forbidden, errorDetail, correlationId, args);

        public static ApplicationError CreateAuthenticationError(
            ErrorDetail errorDetail,
            string correlationId,
            params object[] args
        ) => CreateError(ErrorType.Authentication, errorDetail, correlationId, args);

        public static ApplicationError CreateSecurityError(
            ErrorDetail errorDetail,
            string correlationId,
            params object[] args
        ) => CreateError(ErrorType.Security, errorDetail, correlationId, args);
    }
}
