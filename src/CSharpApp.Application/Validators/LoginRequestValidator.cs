using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharpApp.Application.Constants;

namespace CSharpApp.Application.Validation
{
    public class LoginRequestValidator : AbstractValidator<User>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage(ValidationMessages.UserNameRequired);
                //.Length(10, 100).WithMessage(ValidationMessages.ProductTitleMustBeGreaterThan10);

            RuleFor(x => x.PassWord)
                 .NotEmpty().WithMessage(ValidationMessages.PassWordRequired);

        }
    }
}
