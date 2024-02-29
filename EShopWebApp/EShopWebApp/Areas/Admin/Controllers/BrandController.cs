using EShopWebApp.Core.Contracts;
using EShopWebApp.Core.Services;
using EShopWebApp.Core.ViewModels.BrandViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EShopWebApp.Areas.Admin.Controllers
{
    public class BrandController : BaseAdminController
    {
        private readonly IBrandService _brandService;
        public BrandController(IBrandService brandService) => _brandService = brandService;
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(BrandCreateViewModel brandView)
        {
            if (!ModelState.IsValid)
            {
                return View(brandView);
            }
            var exist = await _brandService.ExistsByNameAsync(brandView.Name);
            if (exist)
            {
                var brand = await _brandService.GetByNameAsync(brandView.Name);
                await _brandService.UndoDeleteAsync(brand.Id);
                return RedirectToAction("All", "Brand");
            }

            await _brandService.CreateAsync(brandView);

            return RedirectToAction("All", "Brand");
        }
        public async Task<IActionResult> All()
        {
            var brands = await _brandService.GetAllAsync();
            return View(brands);
        }


        public async Task<IActionResult> Delete(string id)
        {
            await _brandService.DeleteAsync(id);
            return RedirectToAction("All", "Brand");
        }
    }
}
