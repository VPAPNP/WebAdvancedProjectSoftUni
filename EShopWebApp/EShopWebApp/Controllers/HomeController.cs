using EShopWebApp.Core.Contracts;
using EShopWebApp.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using static EShopWebApp.Core.DataConstants.GeneralApplicationConstants.Identity;
namespace EShopWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductService _productService;

        public HomeController(ILogger<HomeController> logger,
            IProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            if (User.IsInRole(AdministratorRoleName))
            {
              return RedirectToAction("Index", "Home", new { area = AdminAreaName });
            }

            var model = await _productService.GetLastThreeAddedAsync();
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Contacts()
        {
			return View();
		}   
        public IActionResult News()
        {
            return View();
        }

        
        public IActionResult Error(int statusCode)
        {
            if (statusCode == 404)
            {
                return View("NotFound404");
            }
            if (statusCode == 500)
            {
                return View("InternalServerError500");
            }
            if (statusCode == 403)
            {
                return View("AccessDenied403");
            }
            if (statusCode == 401)
            {
                return View("Unauthorized401");
            }
            if (statusCode == 400)
            {
                return View("BadRequest400");
            }
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
