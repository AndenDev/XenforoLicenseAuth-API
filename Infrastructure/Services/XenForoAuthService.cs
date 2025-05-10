using System.Collections;
using System.Text;
using System.Net;
using System.Security.Cryptography;
using Application.Common.Errors;
using Application.Common.Results;
using Application.DTOs.Response;
using Application.Interfaces;
using CorrelationId.Abstractions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.Constant;
using Shared.Utils;

namespace Infrastructure.Services
{
    public class XenForoAuthService : IXenforoAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICorrelationContextAccessor _correlationContextAccessor;

        public XenForoAuthService(
            IUnitOfWork unitOfWork,
            ICorrelationContextAccessor correlationContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _correlationContextAccessor = correlationContextAccessor;
        }

        public async Task<ServiceResult<XenforoAuthResponseDTO>> AuthenticateUserAsync(string username, string password, string clientIp)
        {
            var correlationId = _correlationContextAccessor?.CorrelationContext?.CorrelationId ?? Guid.NewGuid().ToString();

            var user = await _unitOfWork.Repository<User>()
                .Queryable()
                .Include(u => u.UserGroup)
                .FirstOrDefaultAsync(u => u.Username == username);

            if (user == null)
            {
                var error = ApplicationError.CreateNotFoundError(
                    ErrorDefinitions.UserNotFound,
                    correlationId,
                    username
                );
                return ServiceResult<XenforoAuthResponseDTO>.NotFound(error);
            }

            var auth = await _unitOfWork.Repository<UserAuthenticate>()
                  .GetAsync(a => a.UserId == user.UserId);

            if (auth == null)
            {
                var error = ApplicationError.CreateNotFoundError(
                    ErrorDefinitions.UserNotFound,
                    correlationId,
                    username
                );
                return ServiceResult<XenforoAuthResponseDTO>.NotFound(error);
            }

            var authDataStr = Encoding.UTF8.GetString(auth.Data);
            if (!VerifyPassword(authDataStr, password))
            {
                var error = ApplicationError.CreateUserError(
                    ErrorDefinitions.InvalidPassword,
                    correlationId,
                    username
                );
                return ServiceResult<XenforoAuthResponseDTO>.UserError(error);
            }

            var allowedGroups = new[] { XenForoUserGroups.Admin };
            if (!allowedGroups.Contains(user.UserGroup.Title, StringComparer.OrdinalIgnoreCase))
            {
                var error = ApplicationError.CreateForbiddenError(
                    ErrorDefinitions.UnauthorizedGroup,
                    correlationId,
                    user.UserGroup.Title
                );
                return ServiceResult<XenforoAuthResponseDTO>.Forbidden(error);
            }

            // Check existing session
            var nowUnix = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var session = await _unitOfWork.Repository<Session>()
                .Queryable()
                .Where(s => s.ExpiryDate >= nowUnix)
                .ToListAsync();

            byte[]? sessionIdBytes = null;

            foreach (var s in session)
            {
                var sessionDataBlob = Encoding.UTF8.GetString(s.SessionData);
                if (new PHPSerializer().Deserialize(sessionDataBlob) is Hashtable sessionData
                    && sessionData.ContainsKey("userId")
                    && Convert.ToInt32(sessionData["userId"]) == user.UserId)
                {
                    sessionIdBytes = s.SessionId;
                    break;
                }
            }

            if (sessionIdBytes == null)
            {
                sessionIdBytes = GenerateSessionId();
            }

            var expiryUnix = DateTimeOffset.UtcNow.AddHours(1).ToUnixTimeSeconds();

            var sessionDataObj = CreateSessionData(user.UserId, user.Username, user.UserGroupId, user.UserGroup.Title, clientIp, nowUnix);
            var serializedSessionData = new PHPSerializer().Serialize(sessionDataObj);

            var sessionEntity = new Session
            {
                SessionId = sessionIdBytes,
                SessionData = Encoding.UTF8.GetBytes(serializedSessionData),
                ExpiryDate = (int)expiryUnix
            };

            await _unitOfWork.Repository<Session>().AddAsync(sessionEntity);

            var ipPacked = IPAddress.Parse(string.IsNullOrEmpty(clientIp) ? "0.0.0.0" : clientIp).GetAddressBytes();
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

            await _unitOfWork.Repository<SessionActivity>().AddAsync(sessionActivity);

            await _unitOfWork.SaveAsync();

            var sessionIdHex = BitConverter.ToString(sessionIdBytes).Replace("-", "").ToLowerInvariant();

            var authResponse = new XenforoAuthResponseDTO
            {
                Success = true,
                User = new XenforoAuthResponseDTO.XenforoUserDTO
                {
                    UserId = user.UserId,
                    UserName = user.Username,
                    UserGroupId = user.UserGroupId,
                },
                SessionId = sessionIdHex
            };

            return ServiceResult<XenforoAuthResponseDTO>.Success(authResponse);
        }
        public async Task<ServiceResult<object>> LogoutAsync(string sessionIdHex)
        {
            var correlationId = _correlationContextAccessor?.CorrelationContext?.CorrelationId ?? Guid.NewGuid().ToString();
            var sessionIdBytes = HexUtils.StringToByteArray(sessionIdHex);

            try
            {
                var session = await _unitOfWork.Repository<Domain.Entities.Session>()
                    .GetAsync(s => s.SessionId == sessionIdBytes);

                if (session == null)
                {
                    var error = ApplicationError.CreateNotFoundError(
                        ErrorDefinitions.SessionNotFound,
                        correlationId,
                        sessionIdHex
                    );
                    return ServiceResult<object>.NotFound(error);
                }
                var sessionDataBlob = Encoding.UTF8.GetString(session.SessionData);
                var serializer = new PHPSerializer();
                var sessionData = serializer.Deserialize(sessionDataBlob) as Hashtable;

                if (sessionData == null || !sessionData.ContainsKey("userId"))
                {
                    var error = ApplicationError.CreateUserError(
                        ErrorDefinitions.InvalidSession,
                        correlationId,
                        sessionIdHex
                    );
                    return ServiceResult<object>.UserError(error);
                }

                var userId = Convert.ToInt32(sessionData["userId"]);

                var sessionActivity = await _unitOfWork.Repository<Domain.Entities.SessionActivity>()
                    .GetAsync(sa => sa.UserId == userId);

                if (sessionActivity != null)
                {
                    _unitOfWork.Repository<Domain.Entities.SessionActivity>().Delete(sessionActivity);
                }

                _unitOfWork.Repository<Domain.Entities.Session>().Delete(session);
                await _unitOfWork.SaveAsync();

                return ServiceResult<object>.Success(null);
            }
            catch (Exception ex)
            {
                var error = ApplicationError.CreateExceptionError(
                    ErrorDefinitions.InternalServerError,
                    correlationId,
                    ex.Message
                );
                return ServiceResult<object>.Exception(error);
            }
        }
        public async Task<ServiceResult<XenforoAuthResponseDTO>> ValidateSessionAsync(string sessionIdHex, string clientIp)
        {
            var correlationId = _correlationContextAccessor?.CorrelationContext?.CorrelationId ?? Guid.NewGuid().ToString();
            var sessionIdBytes = HexUtils.StringToByteArray(sessionIdHex);
            var nowUnix = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            try
            {
                var session = await _unitOfWork.Repository<Domain.Entities.Session>()
                    .GetAsync(s => s.SessionId == sessionIdBytes);

                if (session == null)
                {
                    var error = ApplicationError.CreateNotFoundError(
                        ErrorDefinitions.SessionNotFound,
                        correlationId,
                        sessionIdHex
                    );
                    return ServiceResult<XenforoAuthResponseDTO>.NotFound(error);
                }

                if (session.ExpiryDate < nowUnix)
                {
                    var error = ApplicationError.CreateUserError(
                        ErrorDefinitions.SessionExpired,
                        correlationId,
                        sessionIdHex
                    );
                    return ServiceResult<XenforoAuthResponseDTO>.UserError(error);
                }

                var serializer = new PHPSerializer();
                var sessionDataBlob = Encoding.UTF8.GetString(session.SessionData);
                var sessionData = serializer.Deserialize(sessionDataBlob) as Hashtable;

                if (sessionData == null || !sessionData.ContainsKey("userId"))
                {
                    var error = ApplicationError.CreateUserError(
                        ErrorDefinitions.InvalidSession,
                        correlationId,
                        sessionIdHex
                    );
                    return ServiceResult<XenforoAuthResponseDTO>.UserError(error);
                }

                var userId = Convert.ToInt32(sessionData["userId"]);
                var username = sessionData["username"]?.ToString();
                var userGroupId = Convert.ToInt32(sessionData["userGroupId"]);

                var authResponse = new XenforoAuthResponseDTO
                {
                    Success = true,
                    User = new XenforoAuthResponseDTO.XenforoUserDTO
                    {
                        UserId = userId,
                        UserName = username,
                        UserGroupId = userGroupId,
                    },
                    SessionId = sessionIdHex
                };

                return ServiceResult<XenforoAuthResponseDTO>.Success(authResponse);
            }
            catch (Exception ex)
            {
                var error = ApplicationError.CreateExceptionError(
                    ErrorDefinitions.InternalServerError,
                    correlationId,
                    ex.Message
                );
                return ServiceResult<XenforoAuthResponseDTO>.Exception(error);
            }
        }

        // ===========================
        // 🔐 Helpers
        // ===========================

        private bool VerifyPassword(string authDataStr, string password)
        {
            var serializer = new PHPSerializer();
            var result = serializer.Deserialize(authDataStr);

            if (result is not Hashtable authData || !authData.ContainsKey("hash"))
                return false;

            var storedHash = authData["hash"].ToString()?.Replace("$2y$", "$2a$");
            return BCrypt.Net.BCrypt.Verify(password, storedHash);
        }

        private byte[] GenerateSessionId()
        {
            var sessionIdBytes = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(sessionIdBytes);
            return sessionIdBytes;
        }

        private Hashtable CreateSessionData(int userId, string username, int userGroupId, string userGroupName, string clientIp, long nowUnix)
        {
            var ipAddress = string.IsNullOrEmpty(clientIp) ? "0.0.0.0" : clientIp;
            var ipBytes = IPAddress.Parse(ipAddress).GetAddressBytes();
            var ipPackedString = Encoding.GetEncoding("ISO-8859-1").GetString(ipBytes);

            return new Hashtable
            {
                ["_ip"] = ipPackedString,
                ["userId"] = userId,
                ["username"] = username,
                ["userGroupId"] = userGroupId,
                ["userGroupName"] = userGroupName,
                ["passwordDate"] = nowUnix,
                ["dismissedNotices"] = new Hashtable(),
                ["lastNoticeUpdate"] = nowUnix,
                ["promotionChecked"] = true,
                ["trophyChecked"] = true,
                ["previousActivity"] = nowUnix
            };
        }
    }
}
