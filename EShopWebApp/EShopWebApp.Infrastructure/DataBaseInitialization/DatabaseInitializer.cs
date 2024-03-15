using EShopWebApp.Infrastructure.Data;
using EShopWebApp.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace EShopWebApp.Infrastructure.DataBaseInitialization
{
    public class DatabaseInitializer
    {
        private readonly IServiceProvider _serviceProvider;
        private static readonly Guid unknownCategoryId = Guid.Parse("82d5a7a3-0fa8-4c84-b088-679a09dc2a57");
        private static readonly Guid foodCategoryId = Guid.Parse("4c0af39e-c75a-4f39-be66-b52547750365");
        private static readonly Guid globalBrandId = Guid.Parse("f58c3c02-5b5e-444a-bd7f-6294f780eabf");
        private static readonly Guid noBrandId = Guid.Parse("54832a0a-8c66-46b0-97ec-293760c4153b");
        private static readonly Guid photoId1 = Guid.Parse("cc5e0372-e1cb-4af3-bf53-1be793c30784");
        private static readonly Guid photoId2 = Guid.Parse("9e160dad-49f8-4ee1-8b26-edb1238c0432");
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
        public async Task InitializeUsersAsync()
        {
            using var scope = _serviceProvider.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            await InitializeUsers(userManager);
        }

        public async Task InitializeProductsAsync()
        {
            using var scope = _serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>(); 
            await InitializeProducts(dbContext);
        }
        public async Task InitializeCategoriesAsync()
        {
            using var scope = _serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>(); 
            await InitializeCategories(dbContext);
        }
        public async Task InitializeBrandsAsync()
        {
            using var scope = _serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            await InitializeBrands(dbContext);
        }
        public async Task InitializePhotosAsync()
        {
            using var scope = _serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            await InitializePhotos(dbContext);
        }

        private static async Task InitializePhotos(ApplicationDbContext dbContext)
        {
            if (dbContext.Photos.Any())
                return; // Products already seeded
            
           using MemoryStream ms = new MemoryStream();
            {
                var imageFile1 = new FileStream("wwwroot\\pictures\\Banana-Single.jpg", FileMode.Open);
               
                imageFile1.CopyTo(ms);

                var photos = new[]
                {
                    new Photo {Id = photoId1, Name = "Banana-Single.jpg", Picture = ms.ToArray()},

                };

                await dbContext.Photos.AddRangeAsync(photos);
                await dbContext.SaveChangesAsync();

            }
            
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
        private async Task InitializeBrands(ApplicationDbContext dbContext)
        {
            // Seed products here
            if (dbContext.Brands.Any())
                return; // Products already seeded

            var brands = new[]
            {
                new Brand {Id = noBrandId, Name = "nobrand"},
                new Brand {Id= globalBrandId , Name = "global"},
                // Add more products as needed
            };

            await dbContext.Brands.AddRangeAsync(brands);
            await dbContext.SaveChangesAsync();
        }

        private static async Task InitializeCategories(ApplicationDbContext dbContext)
        {
            // Seed products here
            if (dbContext.Categories.Any())
                return; // Products already seeded
            
            var categories = new[]
            {
                new Category { Id = unknownCategoryId ,Name = "unknown" },
                new Category { Id= foodCategoryId ,Name = "food" },
               
            };

            await dbContext.Categories.AddRangeAsync(categories);
            await dbContext.SaveChangesAsync();
        }
        private static async Task InitializeUsers(UserManager<ApplicationUser> userManager)
        {
            // Seed users here
            if (userManager.Users.Any())
                return; 

            var adminUser = new ApplicationUser
            {
                FirstName = "New",
                LastName = "User",
                UserName = "admin@example.com",
                Email = "admin@example.com"
            };

            await userManager.CreateAsync(adminUser, "Admin@123"); 

            await userManager.AddToRoleAsync(adminUser, "Admin");

           
        }

        private static async Task InitializeProducts(ApplicationDbContext dbContext)
        {
            // Seed products here
            if (dbContext.Products.Any())
                return; 

            var products = new[]
            {
                new Product { Name = "Product 1", Price = 10.99m, Description = "Description of Product 1",CategoryId = unknownCategoryId,BrandId = globalBrandId,PhotoId= photoId1 },
                new Product { Name = "Product 2", Price = 20.49m, Description = "Description of Product 2",CategoryId = unknownCategoryId ,BrandId = globalBrandId,PhotoId = photoId1},
                new Product { Name = "Product 3", Price = 30.99m, Description = "Description of Product 3",CategoryId = unknownCategoryId,BrandId = globalBrandId,PhotoId = photoId1 },
                new Product { Name = "Product 4", Price = 40.49m, Description = "Description of Product 4",CategoryId = unknownCategoryId,BrandId = globalBrandId,PhotoId = photoId1 },
                new Product { Name = "Product 5", Price = 50.99m, Description = "Description of Product 5",CategoryId = unknownCategoryId,BrandId = globalBrandId,PhotoId = photoId1 },
                new Product { Name = "Product 6", Price = 60.49m, Description = "Description of Product 6",CategoryId = unknownCategoryId,BrandId = globalBrandId,PhotoId = photoId1 },
                new Product { Name = "Product 7", Price = 70.99m, Description = "Description of Product 7",CategoryId = unknownCategoryId,BrandId = globalBrandId,PhotoId = photoId1 },
                new Product { Name = "Product 8", Price = 80.49m, Description = "Description of Product 8",CategoryId = unknownCategoryId,BrandId = globalBrandId,PhotoId = photoId1 },
                new Product { Name = "Product 9", Price = 90.99m, Description = "Description of Product 9",CategoryId = unknownCategoryId,BrandId = globalBrandId,PhotoId = photoId1 }
                // Add more products as needed
            };

            await dbContext.Products.AddRangeAsync(products);
            await dbContext.SaveChangesAsync();
        }
    }


}

