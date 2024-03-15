using EShopWebApp.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using static EShopWebApp.Core.DataConstants.GeneralApplicationConstants.Identity;
namespace EShopWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            if (User.IsInRole(AdministratorRoleName))
            {
              return RedirectToAction("Index", "Home", new { area = AdminAreaName });
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
