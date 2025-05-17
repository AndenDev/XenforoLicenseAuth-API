using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Auth.Login.Command
{
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(x => x.Username)
               .NotEmpty().WithMessage("Username is required.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.");


            RuleFor(x => x.ClientIp)
                .NotEmpty().WithMessage("Client IP address is required.")
                .Must(BeAValidIpAddress).WithMessage("Invalid IP address format.");
        }
        private bool BeAValidIpAddress(string ip)
        {
            return IPAddress.TryParse(ip, out _);
        }
    }
}
