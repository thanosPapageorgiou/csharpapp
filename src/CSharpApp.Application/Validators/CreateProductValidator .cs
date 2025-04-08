using FluentValidation;
using CSharpApp.Application.Constants;

namespace CSharpApp.Application.Validation
{
    public class CreateProductValidator : AbstractValidator<CreateProduct>
    {
        public CreateProductValidator()
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
