using Microsoft.AspNetCore.Identity;
using MyShop.WebApi.Domain.Entities;

namespace MyShop.WebApi.Infrastructure.Data
{
    public static class DbInitializer
    {
        public static async Task InitializerAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();

            if (!await roleManager.RoleExistsAsync("Admin"))
                await roleManager.CreateAsync(new IdentityRole("Admin"));

            if (!await roleManager.RoleExistsAsync("User"))
                await roleManager.CreateAsync(new IdentityRole("User"));

            var adminUser = new User
            {
                UserName = "Admin",
                UserTelegram = "@vuzkez",
                Email = "Admin@gmail.com",
            };

            await userManager.CreateAsync(adminUser, "Admin123");
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }
    }
}
