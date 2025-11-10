using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eCommerce.Core.DTO;
using FluentValidation;

namespace eCommerce.Core.Validators
{
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            // add validations to email
            RuleFor(temp=>temp.Email).NotEmpty().WithMessage("email can't be empty")
                .EmailAddress().WithMessage("invalid email format");
            RuleFor(temp => temp.Password).NotEmpty().WithMessage("password can't be empty")
            .Length(min: 8, max: 15).WithMessage("minimum 8 characters required");
        }
    }
}
