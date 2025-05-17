
using System.Collections;
using System.Text;
using Application.Common.Errors;
using Application.Common.Results;
using Application.Interfaces;
using CorrelationId.Abstractions;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Constant;
using Shared.Utils;

namespace Application.Features.Auth.ValidateSession.Command
{
    public class ValidateSessionCommandHandler : IRequestHandler<ValidateSessionCommand, ServiceResult<ValidateSessionViewModel>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICorrelationContextAccessor _correlationContextAccessor;

        public ValidateSessionCommandHandler(
            IUnitOfWork unitOfWork,
            ICorrelationContextAccessor correlationContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _correlationContextAccessor = correlationContextAccessor;
        }

        public async Task<ServiceResult<ValidateSessionViewModel>> Handle(ValidateSessionCommand request, CancellationToken cancellationToken)
        {
            var correlationId = _correlationContextAccessor?.CorrelationContext?.CorrelationId ?? Guid.NewGuid().ToString();
            var sessionIdBytes = HexUtils.StringToByteArray(request.SessionId);
            var nowUnix = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            var session = await _unitOfWork.Repository<Session>()
                .GetAsync(s => s.SessionId == sessionIdBytes);

            if (session == null)
            {
                return ServiceResult<ValidateSessionViewModel>.NotFound(
                    ApplicationError.CreateNotFoundError(
                        ErrorDefinitions.SessionNotFound,
                        correlationId,
                        request.SessionId
                    )
                );
            }

            if (session.ExpiryDate < nowUnix)
            {
                return ServiceResult<ValidateSessionViewModel>.UserError(
                    ApplicationError.CreateUserError(
                        ErrorDefinitions.SessionExpired,
                        correlationId,
                        request.SessionId
                    )
                );
            }

            var sessionDataBlob = Encoding.UTF8.GetString(session.SessionData);
            var serializer = new PHPSerializer();
            var sessionData = serializer.Deserialize(sessionDataBlob) as Hashtable;

            if (sessionData == null || !sessionData.ContainsKey("userId"))
            {
                return ServiceResult<ValidateSessionViewModel>.UserError(
                    ApplicationError.CreateUserError(
                        ErrorDefinitions.InvalidSession,
                        correlationId,
                        request.SessionId
                    )
                );
            }

            var userId = Convert.ToInt32(sessionData["userId"]);

            var user = await _unitOfWork.Repository<User>()
                .Queryable()
                .Include(u => u.UserGroup)
                .FirstOrDefaultAsync(u => u.UserId == userId, cancellationToken);

            if (user == null)
            {
                return ServiceResult<ValidateSessionViewModel>.NotFound(
                    ApplicationError.CreateNotFoundError(
                        ErrorDefinitions.UserNotFound,
                        correlationId,
                        userId.ToString()
                    )
                );
            }

            var allowedGroups = new[] { XenForoUserGroups.Admin };
            if (!allowedGroups.Contains(user.UserGroup.Title, StringComparer.OrdinalIgnoreCase))
            {
                return ServiceResult<ValidateSessionViewModel>.Forbidden(
                    ApplicationError.CreateForbiddenError(
                        ErrorDefinitions.UnauthorizedGroup,
                        correlationId,
                        user.UserGroup.Title
                    )
                );
            }

            var response = new ValidateSessionViewModel
            {
                Success = true,
                SessionId = request.SessionId,
                User = new ValidateSessionViewModel.SessionUserViewModel
                {
                    UserId = user.UserId,
                    UserName = user.Username,
                    UserGroupId = user.UserGroupId,
                    UserGroupName = user.UserGroup.Title
                }
            };

            return ServiceResult<ValidateSessionViewModel>.Success(response);
        }

    }
}
