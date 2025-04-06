using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharpApp.Application.Constants;

namespace CSharpApp.Application.Validation
{
    public class CreateCategoryRequestValidator : AbstractValidator<CreateCategoryRequest>
    {
        public CreateCategoryRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage(ValidationMessages.CategoryNameRequired)
                .Length(10, 100).WithMessage(ValidationMessages.CategoryNameMustBeGreaterThan10);

            RuleFor(x => x.Image)
                .NotEmpty().WithMessage(ValidationMessages.CategoryImageRequired);
        }
    }
}
