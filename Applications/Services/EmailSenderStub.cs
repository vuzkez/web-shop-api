using MyShop.WebApi.Applications.Interfaces;

namespace MyShop.WebApi.Applications.Services
{
    public class EmailSenderStub : IEmailSender
    {
        private readonly ILogger<EmailSenderStub> _logger;

        public EmailSenderStub(ILogger<EmailSenderStub> logger)
        {
            _logger = logger;
        }

        public Task SendEmailAsync(string toEmail, string subject, string htmlMessage)
        {
            _logger.LogInformation(
                "ПИСЬМО (ЗАГЛУШКА):\n" +
                "Кому: {Email}\n" +
                "Тема: {Subject}\n" +
                "Содержание: {Message}",
                toEmail, subject, htmlMessage);

            return Task.CompletedTask;
        }
    }
}
