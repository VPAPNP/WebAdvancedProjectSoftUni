using EShopWebApp.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
namespace EShopWebApp.Areas.Admin.Controllers
{
    public class HomeController : BaseAdminController
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            

            return View();
        }

        public IActionResult Privacy()
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
