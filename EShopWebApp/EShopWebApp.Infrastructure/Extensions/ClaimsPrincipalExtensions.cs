using System.Security.Claims;

namespace EShopWebApp.Infrastructure.Extensions
{
     
    public static class ClaimsPrincipalExtensions
    {
        public static string? GetId(this ClaimsPrincipal user)
            => user.FindFirstValue(ClaimTypes.NameIdentifier);

        public async static Task<string?> GetIdAsync(this ClaimsPrincipal user)
            => await Task.FromResult(user.FindFirstValue(ClaimTypes.NameIdentifier));
        
    }
}
