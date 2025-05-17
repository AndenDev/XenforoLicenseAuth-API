using Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Auth.Login.Command
{
    public class LoginCommand : IRequest<ServiceResult<LoginViewModel>>
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string ClientIp { get; set; }
    }
}
