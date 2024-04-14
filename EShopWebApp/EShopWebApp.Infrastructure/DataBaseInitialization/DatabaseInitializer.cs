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
        private static readonly Guid photoId3 = Guid.Parse("db684f9f-3f0f-40b9-b336-f62175e1781b");
        private static readonly Guid photoId4 = Guid.Parse("232d27f9-a63b-4085-9318-43fb0f2011f4");
        private static readonly Guid photoId5 = Guid.Parse("a8aaa995-bb23-45eb-8a76-6377ba8b07a3");
        private static readonly Guid photoId6 = Guid.Parse("85a03c6c-9fae-4257-beb4-895c712b4177");
        private static readonly Guid photoId7 = Guid.Parse("2de83d4b-86fe-474c-8c15-95aae87573d1");
        private static readonly Guid photoId8 = Guid.Parse("14c0df6a-d34e-4b7a-9915-a7b0eabf160f");
        private static readonly Guid photoId9 = Guid.Parse("ca4dccc8-5c70-4d83-b39d-f305639ae7ef");
            
        private static readonly Guid productId1 = Guid.Parse("906b382c-e4fd-4c16-8747-4f755c973533");
        private static readonly Guid productId2 = Guid.Parse("15d68fb8-55d3-4b14-a99b-31a0370d8fc8");
        private static readonly Guid productId3 = Guid.Parse("32b83edc-168a-4d6e-968c-0df320d77084");
        private static readonly Guid productId4 = Guid.Parse("87b7fc16-836b-46f3-95cf-8035461e9720");
        private static readonly Guid productId5 = Guid.Parse("71d6cf8b-2f04-4bd2-bdd5-d26cddfa8b29");
        private static readonly Guid productId6 = Guid.Parse("5aef69f2-e43a-4fc4-92ba-edf9a8dd1cc0");
        private static readonly Guid productId7 = Guid.Parse("29ecbff0-8a74-4dea-87fa-5fcb96794977");
        private static readonly Guid productId8 = Guid.Parse("fa5aaf2b-3405-47e8-a512-a11c80f96bc2");
        private static readonly Guid productId9 = Guid.Parse("fd4d0f55-b863-456b-a153-7a1326cc0efb");
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
                new Product {Id = productId1, Name = "Product 1", Price = 10.99m, Description = "Description of Product 1",CategoryId = unknownCategoryId,BrandId = globalBrandId,FrontPhotoId= photoId1 },
                new Product { Id = productId2, Name = "Product 2", Price = 20.49m, Description = "Description of Product 2",CategoryId = unknownCategoryId ,BrandId = globalBrandId,FrontPhotoId = photoId2},
                new Product {Id= productId3, Name = "Product 3", Price = 30.99m, Description = "Description of Product 3",CategoryId = unknownCategoryId,BrandId = globalBrandId,FrontPhotoId = photoId3 },
                new Product {Id= productId4, Name = "Product 4", Price = 40.49m, Description = "Description of Product 4",CategoryId = unknownCategoryId,BrandId = globalBrandId,FrontPhotoId = photoId4 },
                new Product {Id= productId5, Name = "Product 5", Price = 50.99m, Description = "Description of Product 5",CategoryId = unknownCategoryId,BrandId = globalBrandId,FrontPhotoId = photoId5 },
                new Product {Id= productId6, Name = "Product 6", Price = 60.49m, Description = "Description of Product 6",CategoryId = unknownCategoryId,BrandId = globalBrandId,FrontPhotoId = photoId6 },
                new Product {Id= productId7, Name = "Product 7", Price = 70.99m, Description = "Description of Product 7",CategoryId = unknownCategoryId,BrandId = globalBrandId,FrontPhotoId = photoId7 },
                new Product {Id= productId8, Name = "Product 8", Price = 80.49m, Description = "Description of Product 8",CategoryId = unknownCategoryId,BrandId = globalBrandId,FrontPhotoId = photoId8 },
                new Product {Id= productId9, Name = "Product 9", Price = 90.99m, Description = "Description of Product 9",CategoryId = unknownCategoryId,BrandId = globalBrandId,FrontPhotoId = photoId9 }
                // Add more products as needed
            };

            await dbContext.Products.AddRangeAsync(products);
           
            await dbContext.SaveChangesAsync();
            var photos = dbContext.Photos.ToList();
            foreach (var product in products)
            {
                product.FrontPhoto.ProductId = product.Id;
            }
            await dbContext.SaveChangesAsync();

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
                    new Photo {Id = photoId2, Name = "Banana-Single.jpg", Picture = ms.ToArray()},
                    new Photo {Id = photoId3, Name = "Banana-Single.jpg", Picture = ms.ToArray()},
                    new Photo {Id = photoId4, Name = "Banana-Single.jpg", Picture = ms.ToArray()},
                    new Photo {Id = photoId5, Name = "Banana-Single.jpg", Picture = ms.ToArray()},
                    new Photo {Id = photoId6, Name = "Banana-Single.jpg", Picture = ms.ToArray()},
                    new Photo {Id = photoId7, Name = "Banana-Single.jpg", Picture = ms.ToArray()},
                    new Photo {Id = photoId8, Name = "Banana-Single.jpg", Picture = ms.ToArray()},
                    new Photo {Id = photoId9, Name = "Banana-Single.jpg", Picture = ms.ToArray()}

                };

                await dbContext.Photos.AddRangeAsync(photos);
                await dbContext.SaveChangesAsync();

                

            }
            
        }
    }


}

