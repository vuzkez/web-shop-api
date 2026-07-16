using MediatR;
using Microsoft.AspNetCore.Identity;
using MyShop.WebApi.Applications.Common.Exceptions;
using MyShop.WebApi.Applications.Queries.AuthenticationQueries;
using MyShop.WebApi.Domain.Entities;

namespace MyShop.WebApi.Applications.Handlers.AuthenticationHandlers
{
    public class ConfirmEmailQueryHandler : IRequestHandler<ConfirmEmailQuery,bool>
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger<ConfirmEmailQueryHandler> _logger;

        public ConfirmEmailQueryHandler(UserManager<User> userManager,ILogger<ConfirmEmailQueryHandler> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<bool> Handle(ConfirmEmailQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
                throw new NotFoundException(nameof(User), request.UserId);

            var result = await _userManager.ConfirmEmailAsync(user, request.Token);
            if (!result.Succeeded)
            {
                Dictionary<string,string> errors = new Dictionary<string,string>();
                foreach (var error in result.Errors)
                    errors.Add(error.Code, error.Description);

                throw new CustomValidationException(errors);
            }
            _logger.LogInformation("Пользователь с id:{user.Id} подтвердил Email.", user.Id);
            return true;
        }
    }
}
