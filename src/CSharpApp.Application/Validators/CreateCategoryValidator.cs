using FluentValidation;
using CSharpApp.Application.Constants;

namespace CSharpApp.Application.Validation
{
    public class CreateCategoryValidator : AbstractValidator<CreateCategory>
    {
        public CreateCategoryValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage(ValidationMessages.CategoryNameRequired)
                .Length(10, 100).WithMessage(ValidationMessages.CategoryNameMustBeGreaterThan10);

            RuleFor(x => x.Image)
                .NotEmpty().WithMessage(ValidationMessages.CategoryImageRequired);
        }
    }
}
