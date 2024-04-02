using EShopWebApp.Core.Contracts;
using EShopWebApp.Core.ViewModels.CategoryViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EShopWebApp.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CategoryController : BaseAdminController
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService) => _categoryService = categoryService;
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryCreateViewModel categoryView)
        {
            if (!ModelState.IsValid)
            {
                return View(categoryView);
            }
            var exist = await _categoryService.ExistsByNameAsync(categoryView.Name);
            if (exist)
            {
                var category = await _categoryService.GetByNameAsync(categoryView.Name);
                await _categoryService.UndoDeleteAsync(category.Id);
                return RedirectToAction("All", "Category");
            }

            await _categoryService.CreateAsync(categoryView);

            return RedirectToAction("All", "Category");
        }

        public async Task<IActionResult> All()
        {
            var categories = await _categoryService.GetAllAsync();
            return View(categories);
        }
        public async Task<IActionResult> Edit(string id)
        {
            var category = await _categoryService.GetByIdAsync(Guid.Parse(id));
            return View(category);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(CategoryViewModel categoryView)
        {
            if (!ModelState.IsValid)
            {
                return View(categoryView);
            }

            await _categoryService.EditAsync(categoryView);
           
            return RedirectToAction("All", "Category");
        }


        public async Task<IActionResult> Delete(string id)
        {
            await _categoryService.DeleteAsync(id);
            return RedirectToAction("All", "Category");
        }
    }
}
