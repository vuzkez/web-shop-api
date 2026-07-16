using MediatR;
using Microsoft.AspNetCore.Identity;
using MyShop.WebApi.Applications.Interfaces;
using MyShop.WebApi.Applications.Queries.AuthenticationQueries;
using MyShop.WebApi.Domain.Entities;
using MyShop.WebApi.Applications.Common.Exceptions;
using reCAPTCHA.AspNetCore;

namespace MyShop.WebApi.Applications.Handlers.AuthenticationHandlers
{
    public class LoginQueryHandler : IRequestHandler<LoginQuery, string>
    {
        private readonly UserManager<User> _userManager;
        private readonly IJwtService _jwtService;
        private readonly IRecaptchaService _recaptchaService;
        private readonly ILogger<LoginQueryHandler> _logger;

        public LoginQueryHandler(UserManager<User> userManager, IJwtService jwtService,IRecaptchaService recaptchaService, ILogger<LoginQueryHandler> logger)
        {
            _userManager = userManager;
            _jwtService = jwtService;
            _recaptchaService = recaptchaService;
            _logger = logger;
        }
        public async Task<string> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            var recaptchaResult = await _recaptchaService.Validate(request.CaptchaToken);
            if (!recaptchaResult.success)
                throw new CustomValidationException("CAPTCHA не пройдена. Попробуйте ещё раз.");

            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                throw new CustomValidationException("Неверный email или пароль");

            if (!await _userManager.CheckPasswordAsync(user, request.Password))
                throw new CustomValidationException("Неверный email или пароль");

            if (!await _userManager.IsEmailConfirmedAsync(user))
                throw new CustomValidationException("Email не подтверждён. Проверьте почту.");

            var roles = await _userManager.GetRolesAsync(user);
            _logger.LogInformation("Пользователь с id:{user.Id} вошел в систему.", user.Id);

            return _jwtService.GenerateToken(user, roles);
        }
    }
}
