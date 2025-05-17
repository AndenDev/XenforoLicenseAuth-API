
using Application.Common.Responses;
using Application.Common.Results;
using AutoMapper;
using CorrelationId.Abstractions;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers
{
    /// <summary>
    /// Base controller class for handling API responses.
    /// </summary>
    public class BaseController : ControllerBase
    {
        protected readonly IMapper _mapper;
        protected readonly ICorrelationContextAccessor _correlationContextAccessor;

        public BaseController(IMapper mapper, ICorrelationContextAccessor correlationContextAccessor)
        {
            _mapper = mapper;
            _correlationContextAccessor = correlationContextAccessor;
        }
        protected ActionResult<ApiResponse<TResponse>> HandleInlineError<TResponse>(
            string errorCode,
            string errorMessage,
            HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        {
            var problemDetails = new ProblemDetails
            {
                Title = errorCode,
                Detail = errorMessage,
                Status = (int)statusCode
            };

            return base.BadRequest(ApiResponse<TResponse>.CreateError(
                data: default,
                error: problemDetails,
                statusCode: statusCode));
        }

        public ActionResult<ApiResponse<TApiResponse>> HandleResponse<TServiceResult, TApiResponse>(ServiceResult<TServiceResult> result, bool isCreation = false)
        {
            if (result.IsSuccess)
            {
                return HandleActionResult<TServiceResult, TApiResponse>(result, isCreation, HttpStatusCode.OK);
            }

            return result.ErrorType switch
            {
                ErrorType.User => HandleErrorResponse<TServiceResult, TApiResponse>(result, HttpStatusCode.BadRequest),
                ErrorType.NotFound => HandleErrorResponse<TServiceResult, TApiResponse>(result, HttpStatusCode.NotFound),
                ErrorType.Conflict => HandleErrorResponse<TServiceResult, TApiResponse>(result, HttpStatusCode.Conflict),
                _ => HandleErrorResponse<TServiceResult, TApiResponse>(result, HttpStatusCode.InternalServerError),
            };
        }

        public ActionResult<ApiResponse<TApiResponse>> HandleResponse<TServiceResult, TApiResponse>(
            ServiceResult<TServiceResult> result,
            Action<IMappingOperationOptions> mappingOptions,
            bool isCreation = false)
        {
            if (result.IsSuccess)
            {
                return HandleActionResult<TServiceResult, TApiResponse>(result, mappingOptions, isCreation, HttpStatusCode.OK);
            }

            return result.ErrorType switch
            {
                ErrorType.User => HandleErrorResponse<TServiceResult, TApiResponse>(result, HttpStatusCode.BadRequest),
                ErrorType.NotFound => HandleErrorResponse<TServiceResult, TApiResponse>(result, HttpStatusCode.NotFound),
                ErrorType.Conflict => HandleErrorResponse<TServiceResult, TApiResponse>(result, HttpStatusCode.Conflict),
                _ => HandleErrorResponse<TServiceResult, TApiResponse>(result, HttpStatusCode.InternalServerError),
            };
        }

        public ActionResult<ApiResponse<TResponse>> HandleResponse<TResponse>(ServiceResult<TResponse> result, bool isCreation = false)
        {
            if (result.IsSuccess)
            {
                return HandleActionResult(result, isCreation, HttpStatusCode.OK);
            }

            return result.ErrorType switch
            {
                ErrorType.User => HandleErrorResponse(result, HttpStatusCode.BadRequest),
                ErrorType.NotFound => HandleErrorResponse(result, HttpStatusCode.NotFound),
                ErrorType.Conflict => HandleErrorResponse(result, HttpStatusCode.Conflict),
                _ => HandleErrorResponse(result, HttpStatusCode.InternalServerError),
            };
        }

        public ActionResult<ApiResponse<TResponse>> HandleResponse<TResponse>(
            Action<TResponse> action,
            ServiceResult<TResponse> result,
            bool isCreation)
        {
            if (result.IsSuccess)
            {
                action?.Invoke(result.Data);
                return HandleActionResult(result, isCreation, HttpStatusCode.OK);
            }

            return result.ErrorType switch
            {
                ErrorType.User => HandleErrorResponse(result, HttpStatusCode.BadRequest),
                ErrorType.NotFound => HandleErrorResponse(result, HttpStatusCode.NotFound),
                ErrorType.Conflict => HandleErrorResponse(result, HttpStatusCode.Conflict),
                _ => HandleErrorResponse(result, HttpStatusCode.InternalServerError),
            };
        }

        public ActionResult<ApiResponse<TApiResponse>> HandleResponse<TServiceResult, TApiResponse>(
            Func<TServiceResult, TApiResponse> action,
            ServiceResult<TServiceResult> result,
            bool isCreation)
        {
            if (result.IsSuccess)
            {
                var actionResponse = action.Invoke(result.Data);
                return HandleActionResult(ServiceResult<TApiResponse>.Success(actionResponse), isCreation, HttpStatusCode.OK);
            }

            return result.ErrorType switch
            {
                ErrorType.User => HandleErrorResponse<TServiceResult, TApiResponse>(result, HttpStatusCode.BadRequest),
                ErrorType.NotFound => HandleErrorResponse<TServiceResult, TApiResponse>(result, HttpStatusCode.NotFound),
                ErrorType.Conflict => HandleErrorResponse<TServiceResult, TApiResponse>(result, HttpStatusCode.Conflict),
                _ => HandleErrorResponse<TServiceResult, TApiResponse>(result, HttpStatusCode.InternalServerError),
            };
        }

        private ActionResult<ApiResponse<TResponse>> HandleActionResult<TResult, TResponse>(ServiceResult<TResult> result, bool isCreation, HttpStatusCode successStatusCode)
        {
            if (isCreation)
            {
                return new CreatedResult(
                    string.Empty,
                    ApiResponse<TResponse>.CreateSuccess(
                        _mapper.Map<TResponse>(result.Data),
                        result.GetSuccessMessage ?? string.Empty,
                        HttpStatusCode.Created));
            }

            return new OkObjectResult(ApiResponse<TResponse>.CreateSuccess(
                _mapper.Map<TResponse>(result.Data),
                result.GetSuccessMessage ?? string.Empty,
                successStatusCode));
        }

        private static ActionResult<ApiResponse<TResponse>> HandleActionResult<TResponse>(ServiceResult<TResponse> result, bool isCreation, HttpStatusCode successStatusCode)
        {
            if (isCreation)
            {
                return new CreatedResult(
                    string.Empty,
                    ApiResponse<TResponse>.CreateSuccess(
                        result.Data,
                        result.GetSuccessMessage ?? string.Empty,
                        HttpStatusCode.Created));
            }

            return new OkObjectResult(ApiResponse<TResponse>.CreateSuccess(
                result.Data,
                result.GetSuccessMessage ?? string.Empty,
                successStatusCode));
        }

        private ActionResult<ApiResponse<TResponse>> HandleErrorResponse<TResult, TResponse>(ServiceResult<TResult> result, HttpStatusCode errorStatusCode)
        {
            var errorDetails = _mapper.Map<ProblemDetails>(result.Error);

            if (result.ErrorType == ErrorType.User && result.Data != null)
            {
                return base.BadRequest(ApiResponse<TResponse>.CreateError(
                    data: _mapper.Map<TResponse>(result.Data),
                    error: errorDetails,
                    statusCode: errorStatusCode));
            }

            return base.StatusCode((int)errorStatusCode, ApiResponse<TResponse>.CreateError(
                data: _mapper.Map<TResponse>(result.Data),
                error: errorDetails,
                statusCode: errorStatusCode));
        }

        private ActionResult<ApiResponse<TResponse>> HandleErrorResponse<TResponse>(ServiceResult<TResponse> result, HttpStatusCode errorStatusCode)
        {
            var errorDetails = _mapper.Map<ProblemDetails>(result.Error);

            if (result.Error != null && result.ErrorType == ErrorType.User && result.Data != null)
            {
                return base.BadRequest(ApiResponse<TResponse>.CreateError(
                    result.Data,
                    error: errorDetails,
                    statusCode: errorStatusCode));
            }

            return base.StatusCode((int)errorStatusCode, ApiResponse<TResponse>.CreateError(
                result.Data,
                error: errorDetails,
                statusCode: errorStatusCode));
        }
        public ActionResult<ApiResponse<T>> HandleResponse<T>(T result, bool isCreation = false)
        {
            if (result == null)
            {
                var error = new ProblemDetails
                {
                    Title = "Not Found",
                    Detail = "Resource was not found.",
                    Status = (int)HttpStatusCode.NotFound
                };

                return NotFound(ApiResponse<T>.CreateError(default, error, HttpStatusCode.NotFound));
            }

            if (isCreation)
            {
                return new CreatedResult(
                    string.Empty,
                    ApiResponse<T>.CreateSuccess(result, "Created", HttpStatusCode.Created));
            }

            return Ok(ApiResponse<T>.CreateSuccess(result, "Success", HttpStatusCode.OK));
        }


        private ActionResult<ApiResponse<TApiResponse>> HandleActionResult<TServiceResult, TApiResponse>(
            ServiceResult<TServiceResult> result,
            Action<IMappingOperationOptions> mappingOptions,
            bool isCreation,
            HttpStatusCode successStatusCode)
        {
            if (isCreation)
            {
                return new CreatedResult(
                    string.Empty,
                    ApiResponse<TApiResponse>.CreateSuccess(
                        _mapper.Map<TApiResponse>(result.Data, mappingOptions),
                        result.GetSuccessMessage ?? string.Empty,
                        HttpStatusCode.Created));
            }

            return new OkObjectResult(ApiResponse<TApiResponse>.CreateSuccess(
                _mapper.Map<TApiResponse>(result.Data, mappingOptions),
                result.GetSuccessMessage ?? string.Empty,
                successStatusCode));
        }
    }
}
