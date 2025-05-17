using Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Auth.ValidateSession.Command
{
    public class ValidateSessionCommand : IRequest<ServiceResult<ValidateSessionViewModel>>
    {
        public string SessionId { get; set; }
        public string ClientIp { get; set; }
    }

}
