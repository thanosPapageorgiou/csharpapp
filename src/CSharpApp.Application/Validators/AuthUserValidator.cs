using FluentValidation;
using CSharpApp.Application.Constants;

namespace CSharpApp.Application.Validation
{
    public class AuthUserValidator : AbstractValidator<AuthUser>
    {
        public AuthUserValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage(ValidationMessages.UserNameRequired);

            RuleFor(x => x.PassWord)
                 .NotEmpty().WithMessage(ValidationMessages.PassWordRequired);

        }
    }
}
