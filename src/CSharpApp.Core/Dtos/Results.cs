using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpApp.Core.Dtos
{
    public class Result<T>
    {
        public T Data { get; private set; }
        public bool IsSuccess { get; private set; }
        public string ErrorMessage { get; private set; }
        public int StatusCode { get; private set; }

        
        private Result() { }

        // Success result constructor
        public static Result<T> Success(T data)
        {
            return new Result<T>
            {
                Data = data,
                IsSuccess = true,
                StatusCode = 200 // OK
            };
        }

        // Failure result constructor
        public static Result<T> Failure(string errorMessage, int statusCode = 500)
        {
            return new Result<T>
            {
                Data = default(T),
                IsSuccess = false,
                ErrorMessage = errorMessage,
                StatusCode = statusCode
            };
        }
    }
}
