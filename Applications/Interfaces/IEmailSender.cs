namespace MyShop.WebApi.Applications.Interfaces
{
    public interface IEmailSender
    {
        /// <summary>
        /// Метод для рассылки кода (сообщения) зарегестрированным пользователям.
        /// </summary>
        /// <param name="toEmail">Email пользователя.</param>
        /// <param name="subject">Тема сообщения.</param>
        /// <param name="htmlMessage">Само сообщение (html).</param>
        /// <returns></returns>
        Task SendEmailAsync(string toEmail, string subject, string htmlMessage);
    }
}
