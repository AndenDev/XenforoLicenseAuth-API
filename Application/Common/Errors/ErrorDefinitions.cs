using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Errors
{
    public static class ErrorDefinitions
    {
        public static readonly ErrorDetail Unauthorized = new(
             title: "Unauthorized",
             code: "Auth.Unauthorized",
             defaultMessage: "Unauthorized access. You must be logged in to access this resource."
         );

        public static readonly ErrorDetail Forbidden = new(
            title: "Forbidden",
            code: "Auth.Forbidden",
            defaultMessage: "Forbidden: your role '{0}' does not have access to this resource."
        );

        public static readonly ErrorDetail UserNotFound = new(
            title: "User Not Found",
            code: "Auth.UserNotFound",
            defaultMessage: "The user '{0}' was not found."
        );

        public static readonly ErrorDetail InvalidPassword = new(
            title: "Invalid Password",
            code: "Auth.InvalidPassword",
            defaultMessage: "The password is incorrect for user '{0}'."
        );

        public static readonly ErrorDetail UnauthorizedGroup = new(
            title: "Unauthorized Group",
            code: "Auth.UnauthorizedGroup",
            defaultMessage: "User group '{0}' is not authorized to log in."
        );

        public static readonly ErrorDetail SessionNotFound = new(
            title: "Session Not Found",
            code: "Auth.SessionNotFound",
            defaultMessage: "Session '{0}' was not found."
        );

        public static readonly ErrorDetail SessionExpired = new(
            title: "Session Expired",
            code: "Auth.SessionExpired",
            defaultMessage: "Session '{0}' has expired."
        );

        public static readonly ErrorDetail InvalidSession = new(
            title: "Invalid Session",
            code: "Auth.InvalidSession",
            defaultMessage: "Session '{0}' is invalid."
        );

        public static readonly ErrorDetail InternalServerError = new(
            title: "Internal Server Error",
            code: "General.InternalError",
            defaultMessage: "An internal server error occurred: {0}"
        );

    }

}
