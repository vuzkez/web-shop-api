using System.Diagnostics;
using MediatR;

namespace MyShop.WebApi.Applications.Common.ValidationBehaviors
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

        public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Запуск команды {RequestName} с данными: {@Request}", typeof(TRequest).Name, request);

            var stopwatch = Stopwatch.StartNew();

            try
            {
                var response = await next();

                stopwatch.Stop();

                _logger.LogInformation("Запрос {RequestName} выполнен за {ElapsedMilliseconds} мс", typeof(TRequest).Name, stopwatch.ElapsedMilliseconds);

                return response;
            }
            catch (Exception ex)
            {
                stopwatch.Stop();

                _logger.LogError(ex,"Ошибка при выполнении запроса {RequestName} (за {ElapsedMilliseconds} мс)", 
                    typeof(TRequest).Name,stopwatch.ElapsedMilliseconds);

                throw;
            }
        }
    }

}
