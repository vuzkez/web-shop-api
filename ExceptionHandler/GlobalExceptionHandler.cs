using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MyShop.WebApi.Applications.Common.Exceptions;

namespace MyShop.WebApi.GlobalException
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;
        private readonly IWebHostEnvironment _env;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger, IWebHostEnvironment env)
        {
            _logger = logger;
            _env = env;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            _logger.LogError(exception, "Ошибка при выполнении запроса");

            var statusCode = StatusCodes.Status500InternalServerError;
            var title = "Внутренняя ошибка сервера";
            var detailText = "Произошла ошибка на сервере. Попробуйте позже.";

            IDictionary<string, string[]> errorsDictionary = new Dictionary<string, string[]>();

            if (exception is NotFoundException)
            {
                statusCode = StatusCodes.Status404NotFound;
                title = "Ресурс не найден";
                detailText = exception.Message;
            }
            else if (exception is CustomValidationException validationException)
            {
                title = "Ошибка валидации.";
                statusCode = StatusCodes.Status400BadRequest;
                detailText = validationException.Message;

                if (validationException.Errors != null)
                {
                    errorsDictionary = validationException.Errors.ToDictionary(
                        kvp => kvp.Key,
                        kvp => new[] { kvp.Value }
                    );
                }
            }
            else
            {
                detailText = _env.IsDevelopment() ? exception.ToString() : "Произошла ошибка на сервере. Попробуйте позже.";
            }

            var problem = new ValidationProblemDetails(errorsDictionary)
            {
                Status = statusCode,
                Title = title,
                Detail = detailText,
                Instance = httpContext.Request.Path
            };

            httpContext.Response.StatusCode = statusCode;
            httpContext.Response.ContentType = "application/problem+json";
            await httpContext.Response.WriteAsJsonAsync(problem, cancellationToken);

            return true;
        }
    }
}
