
using MediatR;
using System.Text;
using Application.Interfaces;
using CorrelationId.Abstractions;
using Application.Common.Errors;
using Domain.Entities;
using System.Collections;
using System.Net;
using Shared.Utils;
using Shared.Constant;
using Microsoft.EntityFrameworkCore;
using Application.Common.Results;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Application.Features.Auth.Login.Command
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, ServiceResult<LoginViewModel>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICorrelationContextAccessor _correlationContextAccessor;

        public LoginCommandHandler(
          IUnitOfWork unitOfWork,
          ICorrelationContextAccessor correlationContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _correlationContextAccessor = correlationContextAccessor;
        }

        public async Task<ServiceResult<LoginViewModel>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var correlationId = _correlationContextAccessor?.CorrelationContext?.CorrelationId ?? Guid.NewGuid().ToString();

            var user = await _unitOfWork.Repository<User>()
                .Queryable()
                .Include(u => u.UserGroup)
                .FirstOrDefaultAsync(u => u.Username == request.Username, cancellationToken);

            if (user == null)
            {
                return ServiceResult<LoginViewModel>.NotFound(
                    ApplicationError.CreateNotFoundError(
                        ErrorDefinitions.UserNotFound,
                        correlationId,
                        request.Username
                    )
                );
            }

            var auth = await _unitOfWork.Repository<UserAuthenticate>()
                .GetAsync(a => a.UserId == user.UserId);

            if (auth == null)
            {
                return ServiceResult<LoginViewModel>.NotFound(
                    ApplicationError.CreateNotFoundError(
                        ErrorDefinitions.UserNotFound,
                        correlationId,
                        request.Username
                    )
                );
            }

            var authDataStr = Encoding.UTF8.GetString(auth.Data);
            if (!SessionUtils.VerifyPassword(authDataStr, request.Password))
            {
                return ServiceResult<LoginViewModel>.UserError(
                    ApplicationError.CreateUserError(
                        ErrorDefinitions.InvalidPassword,
                        correlationId,
                        request.Username
                    )
                );
            }

            var allowedGroups = new[] { XenForoUserGroups.Admin };
            if (!allowedGroups.Contains(user.UserGroup.Title, StringComparer.OrdinalIgnoreCase))
            {
                return ServiceResult<LoginViewModel>.Forbidden(
                    ApplicationError.CreateForbiddenError(
                        ErrorDefinitions.UnauthorizedGroup,
                        correlationId,
                        user.UserGroup.Title
                    )
                );
            }

            var nowUnix = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            var existingSessions = await _unitOfWork.Repository<Session>()
                .Queryable()
                .ToListAsync(cancellationToken);

            foreach (var session in existingSessions)
            {
                var sessionDataBlob = Encoding.UTF8.GetString(session.SessionData);
                if (new PHPSerializer().Deserialize(sessionDataBlob) is Hashtable sessionData &&
                    sessionData.ContainsKey("userId") &&
                    Convert.ToInt32(sessionData["userId"]) == user.UserId)
                {
                    _unitOfWork.Repository<Session>().Delete(session);
                }
            }

            var existingActivities = await _unitOfWork.Repository<SessionActivity>()
                .Queryable()
                .Where(a => a.UserId == user.UserId)
                .ToListAsync(cancellationToken);

            foreach (var activity in existingActivities)
            {
                _unitOfWork.Repository<SessionActivity>().Delete(activity);
            }

            var sessionIdBytes = SessionUtils.GenerateSessionId();
            var expiryUnix = DateTimeOffset.UtcNow.AddHours(1).ToUnixTimeSeconds();

            var sessionDataObj = SessionUtils.CreateSessionData(user.UserId, user.Username, user.UserGroupId, user.UserGroup.Title, request.ClientIp, nowUnix);
            var serializedSessionData = new PHPSerializer().Serialize(sessionDataObj);

            var sessionEntity = new Session
            {
                SessionId = sessionIdBytes,
                SessionData = Encoding.UTF8.GetBytes(serializedSessionData),
                ExpiryDate = (int)expiryUnix
            };

            await _unitOfWork.Repository<Session>().AddAsync(sessionEntity, cancellationToken);

            var ipPacked = IPAddress.Parse(string.IsNullOrEmpty(request.ClientIp) ? "0.0.0.0" : request.ClientIp).GetAddressBytes();
            var sessionActivity = new SessionActivity
            {
                UserId = user.UserId,
                UniqueKey = ipPacked,
                Ip = ipPacked,
                ControllerName = "XF\\Pub\\Controller\\ForumController",
                ControllerAction = "List",
                ViewState = "valid",
                RobotKey = Array.Empty<byte>(),
                Params = Array.Empty<byte>(),
                ViewDate = (int)nowUnix
            };

            await _unitOfWork.Repository<SessionActivity>().AddAsync(sessionActivity, cancellationToken);
            await _unitOfWork.SaveAsync(cancellationToken);

            var sessionIdHex = BitConverter.ToString(sessionIdBytes).Replace("-", "").ToLowerInvariant();

            var viewModel = new LoginViewModel
            {
                Success = true,
                SessionId = sessionIdHex,
                User = new LoginViewModel.LoginUserViewModel
                {
                    UserId = user.UserId,
                    UserName = user.Username,
                    UserGroupId = user.UserGroupId,
                    UserGroupName = user.UserGroup.Title
                }
            };

            return ServiceResult<LoginViewModel>.Success(viewModel);
        }

    }
}
