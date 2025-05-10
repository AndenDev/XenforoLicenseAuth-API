using Application.Common.Errors;
using AutoMapper;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Mapping
{
    public class ApplicationMappingProfile : Profile
    {
        public ApplicationMappingProfile()
        {
            CreateMap<ApplicationError, ProblemDetails>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => GetStatusCode(src.ErrorType)))
                .ForMember(dest => dest.Detail, opt => opt.MapFrom(src => src.ErrorDescription))
                .ForMember(dest => dest.Extensions, opt => opt.MapFrom(src => new Dictionary<string, object?>
                {
                    { "correlationId", src.CorrelationId },
                    { "traceId", src.TraceId },
                    { "errorCode", src.ErrorCode }
                }));
        }

        private static int GetStatusCode(ErrorType errorType)
        {
            return errorType switch
            {
                ErrorType.User => (int)HttpStatusCode.BadRequest,
                ErrorType.NotFound => (int)HttpStatusCode.NotFound,
                ErrorType.Conflict => (int)HttpStatusCode.Conflict,
                ErrorType.Forbidden => (int)HttpStatusCode.Forbidden,
                ErrorType.Authentication => (int)HttpStatusCode.Unauthorized,
                ErrorType.Exception => (int)HttpStatusCode.InternalServerError,
                _ => (int)HttpStatusCode.InternalServerError
            };
        }
    }
}
