using EShopWebApp.Core.Services;
using EShopWebApp.Core.ViewModels.BrandViewModels;
using EShopWebApp.Infrastructure.Data;
using EShopWebApp.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace TestingEShopWebApp.UnitTests
{
	[TestFixture]
	public class BrandServiceTests
	{
		private ApplicationDbContext CreateDbContext()
		{
			var options = new DbContextOptionsBuilder<ApplicationDbContext>()
				.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
				.Options;

			return new ApplicationDbContext(options);
		}

		[Test]
		public async Task GetAllAsync_ReturnsAllBrands()
		{
			// Arrange
			using var context = CreateDbContext();
			var service = new BrandService(context);

			// Add test data to the in-memory database
			await context.Brands.AddRangeAsync(new[]
			{
				new Brand { Id = Guid.NewGuid(), Name = "Brand 1" },
				new Brand { Id = Guid.NewGuid(), Name = "Brand 2" },
				new Brand { Id = Guid.NewGuid(), Name = "Brand 3" }
			});
			await context.SaveChangesAsync();

			// Act
			var brands = await service.GetAllAsync();

			// Assert
			Assert.That(brands.Count, Is.EqualTo(3));
			Assert.IsTrue(brands.Any(b => b.Name == "Brand 1"));
			Assert.IsTrue(brands.Any(b => b.Name == "Brand 2"));
			Assert.IsTrue(brands.Any(b => b.Name == "Brand 3"));
		}

		[Test]
		public async Task GetByIdAsync_ReturnsBrand()
		{
			using var context = CreateDbContext();
			var service = new BrandService(context);

			var brandId = Guid.NewGuid();
			await context.Brands.AddAsync(new Brand { Id = brandId, Name = "Brand Test" });
			await context.SaveChangesAsync();

			var brand = await service.GetByIdAsync(brandId);

			Assert.IsNotNull(brand);
			Assert.That(brand.Id, Is.EqualTo(brandId.ToString()));
			Assert.That(brand.Name, Is.EqualTo("Brand Test"));
		}

		[Test]
		public async Task GetByNameAsync_ReturnsBrand()
		{
			using var context = CreateDbContext();
			var service = new BrandService(context);

			await context.Brands.AddAsync(new Brand { Id = Guid.NewGuid(), Name = "Brand Test" });
			await context.SaveChangesAsync();

			var brand = await service.GetByNameAsync("Brand Test");

			Assert.IsNotNull(brand);
			Assert.That(brand.Name, Is.EqualTo("Brand Test"));
		}

		[Test]
		public async Task CreateAsync_AddsBrand()
		{
			using var context = CreateDbContext();
			var service = new BrandService(context);

			var brandCreateViewModel = new BrandCreateViewModel { Name = "New Brand" };
			await service.CreateAsync(brandCreateViewModel);

			var createdBrand = await context.Brands.FirstOrDefaultAsync(b => b.Name == "New Brand");

			Assert.IsNotNull(createdBrand);
			Assert.That(createdBrand.Name, Is.EqualTo("New Brand"));
		}

		[Test]
		public async Task ExistsByNameAsync_ReturnsTrueIfBrandExists()
		{
			using var context = CreateDbContext();
			var service = new BrandService(context);

			await context.Brands.AddAsync(new Brand { Id = Guid.NewGuid(), Name = "Existing Brand" });
			await context.SaveChangesAsync();

			var exists = await service.ExistsByNameAsync("Existing Brand");

			Assert.IsTrue(exists);
		}

		[Test]
		public async Task DeleteAsync_SetsIsDeletedToTrue()
		{
			using var context = CreateDbContext();
			var service = new BrandService(context);

			var brandId = Guid.NewGuid();
			await context.Brands.AddAsync(new Brand { Id = brandId, Name = "Brand to Delete" });
			await context.SaveChangesAsync();

			await service.DeleteAsync(brandId.ToString());

			var deletedBrand = await context.Brands.FindAsync(brandId);

			Assert.IsNotNull(deletedBrand);
			Assert.IsTrue(deletedBrand.IsDeleted);
		}

		[Test]
		public async Task EditAsync_UpdatesBrandName()
		{
			using var context = CreateDbContext();
			var service = new BrandService(context);

			var brandId = Guid.NewGuid();
			await context.Brands.AddAsync(new Brand { Id = brandId, Name = "Initial Name" });
			await context.SaveChangesAsync();

			var brandEditViewModel = new BrandViewModel { Id = brandId.ToString(), Name = "Updated Name" };
			await service.EditAsync(brandEditViewModel);

			var updatedBrand = await context.Brands.FindAsync(brandId);

			Assert.IsNotNull(updatedBrand);
			Assert.That(updatedBrand.Name, Is.EqualTo("Updated Name"));
		}
		[Test]
		public async Task UndoDeleteAsync_SetsIsDeletedToFalse()
		{
			// Arrange
			using var context = CreateDbContext();
			var service = new BrandService(context);

			var brandId = Guid.NewGuid();
			await context.Brands.AddAsync(new Brand { Id = brandId, Name = "Deleted Brand", IsDeleted = true });
			await context.SaveChangesAsync();

			// Act
			await service.UndoDeleteAsync(brandId.ToString());

			// Assert
			var updatedBrand = await context.Brands.FindAsync(brandId);
			Assert.IsNotNull(updatedBrand);
			Assert.IsFalse(updatedBrand.IsDeleted);
		}
		[Test]
		public async Task GetAllNamesAsync_ReturnsAllBrandNames()
		{
			// Arrange
			using var context = CreateDbContext();
			var service = new BrandService(context);

			// Add some brands to the database
			await context.Brands.AddRangeAsync(new[]
			{
		new Brand { Id = Guid.NewGuid(), Name = "Brand 1" },
		new Brand { Id = Guid.NewGuid(), Name = "Brand 2" },
		new Brand { Id = Guid.NewGuid(), Name = "Brand 3" }
	});
			await context.SaveChangesAsync();

			// Act
			var brandNames = await service.GetAllNamesAsync();

			// Assert
			Assert.That(brandNames.Count, Is.EqualTo(3));
			Assert.Contains("Brand 1", brandNames);
			Assert.Contains("Brand 2", brandNames);
			Assert.Contains("Brand 3", brandNames);
		}

	}
}



