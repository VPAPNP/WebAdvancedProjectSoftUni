using EShopWebApp.Core.Services;
using EShopWebApp.Core.ViewModels.CategoryViewModels;
using EShopWebApp.Infrastructure.Data;
using EShopWebApp.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace TestingEShopWebApp.UnitTests
{
	[TestFixture]
	public class CategoryServiceTests
	{
		private ApplicationDbContext CreateDbContext()
		{
			var options = new DbContextOptionsBuilder<ApplicationDbContext>()
				.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
				.Options;

			return new ApplicationDbContext(options);
		}

		[Test]
		public async Task GetAllAsync_ReturnsAllCategories()
		{
			// Arrange
			using var context = CreateDbContext();
			var service = new CategoryService(context);

			// Add test data to the in-memory database
			await context.Categories.AddRangeAsync(new[]
			{
				new Category { Id = Guid.NewGuid(), Name = "Category 1" },
				new Category { Id = Guid.NewGuid(), Name = "Category 2" },
				new Category { Id = Guid.NewGuid(), Name = "Category 3" }
			});
			await context.SaveChangesAsync();

			// Act
			var categories = await service.GetAllAsync();

			// Assert
			Assert.That(categories.Count, Is.EqualTo(3));
			Assert.IsTrue(categories.Any(c => c.Name == "Category 1"));
			Assert.IsTrue(categories.Any(c => c.Name == "Category 2"));
			Assert.IsTrue(categories.Any(c => c.Name == "Category 3"));
		}
		[Test]
		public async Task GetByIdAsync_ReturnsCategory()
		{
			using var context = CreateDbContext();
			var service = new CategoryService(context);

			var categoryId = Guid.NewGuid();
			await context.Categories.AddAsync(new Category { Id = categoryId, Name = "Category Test" });
			await context.SaveChangesAsync();

			var category = await service.GetByIdAsync(categoryId);

			Assert.IsNotNull(category);
			Assert.That(category.Id, Is.EqualTo(categoryId.ToString()));
		}
		[Test]
		public async Task GetByNameAsync_ReturnsCategory()
		{
			using var context = CreateDbContext();
			var service = new CategoryService(context);

			var categoryName = "Category Test";
			await context.Categories.AddAsync(new Category { Id = Guid.NewGuid(), Name = categoryName });
			await context.SaveChangesAsync();

			var category = await service.GetByNameAsync(categoryName);

			Assert.IsNotNull(category);
			Assert.That(category.Name, Is.EqualTo(categoryName));
		}
		[Test]
		public async Task CreateAsync_AddsCategory()
		{
			using var context = CreateDbContext();
			var service = new CategoryService(context);

			var categoryName = "Category Test";
			await service.CreateAsync(new CategoryCreateViewModel { Name = categoryName });

			var category = await context.Categories.FirstOrDefaultAsync(c => c.Name == categoryName);

			Assert.IsNotNull(category);
			Assert.That(category.Name, Is.EqualTo(categoryName));
		}
		[Test]
		public async Task ExistsByNameAsync_ReturnsTrueIfCategoryExists()
		{
			using var context = CreateDbContext();
			var service = new CategoryService(context);

			var categoryName = "Category Test";
			await context.Categories.AddAsync(new Category { Id = Guid.NewGuid(), Name = categoryName });
			await context.SaveChangesAsync();

			var exists = await service.ExistsByNameAsync(categoryName);

			Assert.IsTrue(exists);
		}
		[Test]
		public async Task DeleteAsync_SetsIsDeletedToTrue()
		{
			using var context = CreateDbContext();
			var service = new CategoryService(context);

			var categoryId = Guid.NewGuid();
			await context.Categories.AddAsync(new Category { Id = categoryId, Name = "Category Test" });
			await context.SaveChangesAsync();

			await service.DeleteAsync(categoryId.ToString());

			var category = await context.Categories.FirstAsync(c => c.Id == categoryId);

			Assert.IsTrue(category.IsDeleted);
		}
		[Test]
		public async Task GetAllNamesAsync()
		{
			using var context = CreateDbContext();
			var service = new CategoryService(context);

			await context.Categories.AddRangeAsync(new[]
			{
				new Category { Id = Guid.NewGuid(), Name = "Category 1" },
				new Category { Id = Guid.NewGuid(), Name = "Category 2" },
				new Category { Id = Guid.NewGuid(), Name = "Category 3" }
			});
			await context.SaveChangesAsync();

			var categories = await service.GetAllNamesAsync();

			Assert.That(categories.Count, Is.EqualTo(3));
			Assert.IsTrue(categories.Contains("Category 1"));
			Assert.IsTrue(categories.Contains("Category 2"));
			Assert.IsTrue(categories.Contains("Category 3"));
		}
		[Test]
		public async Task EditAsync()
		{
			using var context = CreateDbContext();
			var service = new CategoryService(context);

			var categoryId = Guid.NewGuid();
			await context.Categories.AddAsync(new Category { Id = categoryId, Name = "Category Test" });
			await context.SaveChangesAsync();

			var category = await service.GetByIdAsync(categoryId);
			category.Name = "Category Updated";
			await service.EditAsync(category);

			var updatedCategory = await context.Categories.FirstAsync(c => c.Id == categoryId);

			Assert.That(updatedCategory.Name, Is.EqualTo("Category Updated"));
		}
		[Test]
		public async Task UndoDeleteAsync_SetsIsDeletedToFalse()
		{
			using var context = CreateDbContext();
			var service = new CategoryService(context);

			var categoryId = Guid.NewGuid();
			await context.Categories.AddAsync(new Category { Id = categoryId, Name = "Deleted Category", IsDeleted = true });
			await context.SaveChangesAsync();

			await service.UndoDeleteAsync(categoryId.ToString());

			var updatedCategory = await context.Categories.FirstAsync(c => c.Id == categoryId);

			Assert.IsFalse(updatedCategory.IsDeleted);
		}



	}
}
