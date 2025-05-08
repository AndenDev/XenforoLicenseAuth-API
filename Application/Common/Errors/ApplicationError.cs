using System.Net;

namespace Application.Common.Errors
{
    public class ApplicationError
    {
        public string Code { get; }
        public string Message { get; }
        public HttpStatusCode StatusCode { get; }

        private ApplicationError(string code, string message, HttpStatusCode statusCode)
        {
            Code = code;
            Message = message;
            StatusCode = statusCode;
        }

        public static readonly ApplicationError UserNotFound = new(
            "USER_NOT_FOUND",
            "The specified username does not exist.",
            HttpStatusCode.Unauthorized
        );

        public static readonly ApplicationError InvalidAuthData = new(
            "INVALID_AUTH_DATA",
            "Authentication data is invalid or corrupt.",
            HttpStatusCode.Unauthorized
        );

        public static readonly ApplicationError InvalidPassword = new(
            "INVALID_PASSWORD",
            "The password provided is incorrect.",
            HttpStatusCode.Unauthorized
        );

        public static readonly ApplicationError UnauthorizedGroup = new(
            "UNAUTHORIZED_GROUP",
            "User does not belong to an authorized group.",
            HttpStatusCode.Forbidden
        );
    }
}
