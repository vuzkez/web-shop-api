using MyShop.WebApi.Domain.Entities;

namespace MyShop.WebApi.Applications.Interfaces
{
    public interface IJwtService
    {
        public string GenerateToken(User user,IList<string> roles);
    }
}
