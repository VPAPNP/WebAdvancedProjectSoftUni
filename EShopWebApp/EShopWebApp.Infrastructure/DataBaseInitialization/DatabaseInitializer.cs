using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace EShopWebApp.Infrastructure.DataBaseInitialization
{
    public class DatabaseInitializer
    {
        private readonly IServiceProvider _serviceProvider;

        public DatabaseInitializer(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task InitializeRolesAsync()
        {
            using var scope = _serviceProvider.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
            await InitializeRoles(roleManager);
        }
        private static async Task InitializeRoles(RoleManager<IdentityRole<Guid>> roleManager)
        {


            var roles = new[] { "Admin", "User","Employee" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole<Guid>(role));
                }
            }
        }

    }
}
