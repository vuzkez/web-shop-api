using MyShop.WebApi.Domain.Entities;

namespace MyShop.WebApi.Applications.Interfaces
{
    public interface IJwtService
    {
        /// <summary>
        /// Метод для генерации Jwt токена.
        /// </summary>
        /// <param name="user">Пользователь.</param>
        /// <param name="roles">Все роли.</param>
        /// <returns>Jwt токен типа string</returns>
        public string GenerateToken(User user,IList<string> roles);
    }
}
