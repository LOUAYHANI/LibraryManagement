using LibraryManagement.Application.Loans.Exceptions;
using LibraryManagement.Domain.Loans;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Api.Errors
{
    public class ApiExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<ApiExceptionHandler> _logger;

        public ApiExceptionHandler(ILogger<ApiExceptionHandler> logger)
        {
            _logger = logger;
        }
        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            var statusCode = exception switch
            {
                MemberNotFoundException => StatusCodes.Status404NotFound,
                BookNotFoundException => StatusCodes.Status404NotFound,
                NoAvailableCopyException => StatusCodes.Status409Conflict,
                LoanLimitReachedException => StatusCodes.Status409Conflict,
                LoanNotFoundException => StatusCodes.Status404NotFound,
                LoanAlreadyReturnedException => StatusCodes.Status409Conflict,
                ArgumentException => StatusCodes.Status400BadRequest,
                _ => StatusCodes.Status500InternalServerError
            };

            if (statusCode == StatusCodes.Status500InternalServerError)
            {
                _logger.LogError(exception, "An unexpected error occurred.");
            }


            var problem = new ProblemDetails
            {
                Status = statusCode,
                Title = exception.Message
            };

            httpContext.Response.StatusCode = statusCode;

            await httpContext.Response.WriteAsJsonAsync(problem, cancellationToken);

            return true;
        }
    }
}