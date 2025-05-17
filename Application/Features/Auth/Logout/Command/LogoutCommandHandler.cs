using System.Collections;
using System.Text;
using MediatR;
using Application.Interfaces;
using CorrelationId.Abstractions;
using Application.Common.Errors;
using Shared.Utils;
using Domain.Entities;
using Application.Common.Results;

namespace Application.Features.Auth.Logout.Command
{
    public class LogoutCommandHandler : IRequestHandler<LogoutCommand, ServiceResult<LogoutViewModel>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICorrelationContextAccessor _correlationContextAccessor;

        public LogoutCommandHandler(
            IUnitOfWork unitOfWork,
            ICorrelationContextAccessor correlationContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _correlationContextAccessor = correlationContextAccessor;
        }

        public async Task<ServiceResult<LogoutViewModel>> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            var correlationId = _correlationContextAccessor?.CorrelationContext?.CorrelationId ?? Guid.NewGuid().ToString();
            var sessionIdBytes = HexUtils.StringToByteArray(request.SessionId);

            var session = await _unitOfWork.Repository<Session>()
                .GetAsync(s => s.SessionId == sessionIdBytes);

            if (session == null)
            {
                return ServiceResult<LogoutViewModel>.NotFound(
                    ApplicationError.CreateNotFoundError(
                        ErrorDefinitions.SessionNotFound,
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
                return ServiceResult<LogoutViewModel>.UserError(
                    ApplicationError.CreateUserError(
                        ErrorDefinitions.InvalidSession,
                        correlationId,
                        request.SessionId
                    )
                );
            }

            var userId = Convert.ToInt32(sessionData["userId"]);

            var sessionActivity = await _unitOfWork.Repository<SessionActivity>()
                .GetAsync(sa => sa.UserId == userId);

            if (sessionActivity != null)
            {
                _unitOfWork.Repository<SessionActivity>().Delete(sessionActivity);
            }

            _unitOfWork.Repository<Session>().Delete(session);
            await _unitOfWork.SaveAsync(cancellationToken);

            return ServiceResult<LogoutViewModel>.Success(new LogoutViewModel
            {
                Success = true
            });
        }
    }
}
