using Application.Common.Errors;
using Application.Common.Results;
using Application.Features.XenforoUser.GetUserProfile;
using Application.Interfaces;
using CorrelationId.Abstractions;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.XenforoUser.GetUserProfile
{
    public class GetUserProfileQueryHandler : IRequestHandler<GetUserProfileQuery, ServiceResult<GetUserProfileViewModel>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICorrelationContextAccessor _correlationContextAccessor;

        public GetUserProfileQueryHandler(
            IUnitOfWork unitOfWork,
            ICorrelationContextAccessor correlationContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _correlationContextAccessor = correlationContextAccessor;
        }

        public async Task<ServiceResult<GetUserProfileViewModel>> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
        {
            var correlationId = _correlationContextAccessor?.CorrelationContext?.CorrelationId ?? Guid.NewGuid().ToString();

            var user = await _unitOfWork.Repository<User>()
                .Queryable()
                .Include(u => u.UserGroup)
                .FirstOrDefaultAsync(u => u.UserId == request.UserId, cancellationToken);

            if (user == null)
            {
                return ServiceResult<GetUserProfileViewModel>.NotFound(
                    ApplicationError.CreateNotFoundError(
                        ErrorDefinitions.UserNotFound,
                        correlationId,
                        request.UserId.ToString()
                    )
                );
            }

            return ServiceResult<GetUserProfileViewModel>.Success(new GetUserProfileViewModel
            {
                Username = user.Username,
                Email = user.Email,
                GroupName = user.UserGroup?.Title
            });
        }
    }
}
