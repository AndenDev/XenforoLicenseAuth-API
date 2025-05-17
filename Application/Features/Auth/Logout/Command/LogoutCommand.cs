using Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Auth.Logout.Command
{
    public class LogoutCommand : IRequest<ServiceResult<LogoutViewModel>>
    {
        public string SessionId { get; set; }
    }

}
