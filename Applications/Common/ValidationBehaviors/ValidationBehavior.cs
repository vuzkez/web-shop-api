using FluentValidation;
using MediatR;
using MyShop.WebApi.Applications.Common.Exceptions;

namespace MyShop.WebApi.Applications.Common.ValidationBehaviors
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request,RequestHandlerDelegate<TResponse> next,CancellationToken cancellationToken)
        {
            if (_validators.Any())
            {
                var context = new ValidationContext<TRequest>(request);

                // Запускаем все валидаторы, зарегистрированные для этого типа запроса
                var validationResults = await Task.WhenAll(
                    _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

                // Собираем все ошибки в один список
                var failures = validationResults
                    .SelectMany(r => r.Errors)
                    .Where(f => f != null)
                    .ToList();

                if (failures.Count != 0)
                {
                    var errors = failures
                        .GroupBy(f => f.PropertyName)
                        .ToDictionary(
                            g => g.Key,
                            g => string.Join("; ", g.Select(f => f.ErrorMessage))
                        );

                    throw new CustomValidationException(errors);
                }
            }

            // Если ошибок нет, передаем управление следующему звену или самому хендлеру
            return await next();
        }
    }
}
