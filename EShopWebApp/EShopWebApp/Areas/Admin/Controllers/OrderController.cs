using Microsoft.AspNetCore.Mvc;

namespace EShopWebApp.Areas.Admin.Controllers
{
    public class OrderController : BaseAdminController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
