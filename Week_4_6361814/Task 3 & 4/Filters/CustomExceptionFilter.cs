using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.IO;

namespace Task3_4.Filters
{
    public class CustomExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var exception = context.Exception;
            File.AppendAllText("exceptions.txt", $"{DateTime.Now}: {exception.Message}\n{exception.StackTrace}\n");
            context.Result = new ObjectResult("An error occurred.")
            {
                StatusCode = 500
            };
            context.ExceptionHandled = true;
        }
    }
}
