using EShopWebApp.Core.Contracts;
using EShopWebApp.Core.ViewModels.PackageViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EShopWebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PackageController : Controller
    {
        private readonly IPackageService _packageService;

        public PackageController(IPackageService packageService)
        {
            _packageService = packageService;
        }

        public IActionResult Create(string returnUrl)
        {
            
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(PackageViewModel model, string returnUrl1)
        {
            if (!ModelState.IsValid)
            {
                return Redirect(returnUrl1);
            }
            
            await _packageService.CreateAsync(model);

            return Redirect(returnUrl1);
        }


    }
}
