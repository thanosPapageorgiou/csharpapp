using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CSharpApp.Application.Constants
{
    public static class ValidationMessages
    {
        public const string CategoryNameRequired = "Category Name is required";
        public const string CategoryNameMustBeGreaterThan10 = "Name must be between 10 and 100 characters.";
        public const string CategoryImageRequired = "Image URL is required";

        public const string ProductPriceMustBePositiveNumber = "Price must be greater than zero.";
        public const string ProductTitleRequired = "Title is required.";
        public const string ProductTitleMustBeGreaterThan10 = "Title must be between 10 and 100 characters.";
        public const string ProductCategoryIdMustBePositiveNumber = "CategoryId must be a valid positive number.";
        public const string ProductImageURLRequired = "At least one image URL is required.";

        public const string UserNameRequired = "UserName is required";
        public const string PassWordRequired = "Password is required";
    }
    public static class LoggerMessages
    {
        public const string LoggerValidationFail = "Validation failed:";
    }

    public static class ExceptionMessages
    {
        public const string GeneralArgumentException = "You passed in an invalid parameter!";
    }
    public static class Properties
    {
        public const string Title = "title";
    }
}
