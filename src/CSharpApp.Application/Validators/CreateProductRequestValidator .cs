using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharpApp.Application.Constants;

namespace CSharpApp.Application.Validation
{
    public class CreateProductRequestValidator : AbstractValidator<CreateProductRequest>
    {
        public CreateProductRequestValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage(ValidationMessages.ProductTitleRequired)
                .Length(10, 100).WithMessage(ValidationMessages.ProductTitleMustBeGreaterThan10);

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage(ValidationMessages.ProductPriceMustBePositiveNumber);

            RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage(ValidationMessages.ProductCategoryIdMustBePositiveNumber);

            RuleFor(x => x.Images)
                .NotEmpty().WithMessage(ValidationMessages.ProductImageURLRequired);
        }
    }
}
