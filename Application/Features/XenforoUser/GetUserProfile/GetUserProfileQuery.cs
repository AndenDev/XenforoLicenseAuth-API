using Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.XenforoUser.GetUserProfile
{
    public class GetUserProfileQuery : IRequest<ServiceResult<GetUserProfileViewModel>>
    {
        public int UserId { get; set; }
    }
}
