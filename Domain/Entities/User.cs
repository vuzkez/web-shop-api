using Microsoft.AspNetCore.Identity;

namespace MyShop.WebApi.Domain.Entities
{
    public class User : IdentityUser
    {
        public string? UserTelegram { get; set; }
    }
}
