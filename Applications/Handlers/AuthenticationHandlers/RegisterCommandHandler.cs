using MediatR;
using MyShop.WebApi.Applications.Commands.AuthenticationCommands;
using MyShop.WebApi.Applications.Common.Exceptions;
using MyShop.WebApi.Infrastructure.Data;
using reCAPTCHA.AspNetCore;
using Microsoft.AspNetCore.Identity;
using MyShop.WebApi.Domain.Entities;
using MyShop.WebApi.Applications.Interfaces;

namespace MyShop.WebApi.Applications.Handlers.AuthenticationHandlers
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, bool>
    {
        private readonly AppDbContext _context;
        private readonly IRecaptchaService _recaptchaService;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<RegisterCommandHandler> _logger;

        public RegisterCommandHandler(AppDbContext context,IRecaptchaService recaptchaService, UserManager<User> userManager
            ,IConfiguration configManager,IEmailSender emailSender, ILogger<RegisterCommandHandler> logger)
        {
            _context = context;
            _recaptchaService = recaptchaService;
            _userManager = userManager;
            _configManager = configManager;
            _emailSender = emailSender;
            _logger = logger;
        }
        public async Task<bool> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var recaptchaResult = await _recaptchaService.Validate(request.CaptchaToken);
            if (!recaptchaResult.success)
                throw new CustomValidationException("CAPTCHA не пройдена. Попробуйте ещё раз.");

            var exitingUser = await _userManager.FindByEmailAsync(request.Email);
            if (exitingUser != null)
                throw new CustomValidationException("Пользователь с таким email уже существует.");

            var user = new User
            {
                UserName = request.Username,
                Email = request.Email,
                UserTelegram = request.Telegram
            };

            var createResult = await _userManager.CreateAsync(user, request.Password);
            if (!createResult.Succeeded)
            {
                var errors = new Dictionary<string,string>();
                foreach (var error in createResult.Errors)
                    errors.Add(error.Code, error.Description);

                throw new CustomValidationException(errors);
            }

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodedToken = Uri.EscapeDataString(token);
            var encodedUserId = Uri.EscapeDataString(user.Id);

            var baseUrl = _configManager["App:BaseUrl"];
            var confirmationLink = $"{baseUrl}/api/auth/confirm-email?userId={encodedUserId}&token={encodedToken}";

            await _emailSender.SendEmailAsync(user.Email, "Подтверждение email",
               $@"
                    <h2>Добро пожаловать в MyShop!</h2>
                    <p>Подтвердите ваш email, перейдя по ссылке:</p>
                    <a href='{confirmationLink}'>Подтвердить email</a>
                    <p>Ссылка действительна 24 часа.</p>
               ");

            await _userManager.AddToRoleAsync(user, "User");
            _logger.LogInformation("Пользователь с id:{user.Id} зарегестрировался.",user.Id);

            return true;
        }
    }
}
