using FluentValidation;

namespace MyShop.WebApi.Applications.Commands.ProductCommands
{
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Название обязательно")
                .MinimumLength(1).WithMessage("Минимум 1 символ");

            RuleFor(x => x.Price).NotNull().GreaterThan(0)
                .WithMessage("Цена должна быть больше 0");
        }
    }
}
