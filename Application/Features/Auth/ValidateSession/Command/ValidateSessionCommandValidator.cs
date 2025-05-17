using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Auth.ValidateSession.Command
{
    public class ValidateSessionCommandValidator : AbstractValidator<ValidateSessionCommand>
    {
        public ValidateSessionCommandValidator()
        {
        }
    }
}
