using Microsoft.AspNetCore.Mvc;

namespace EShopWebApp.Controllers
{
	public class NewsController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
		public IActionResult Details()
		{
			return View();
		}
	}
}
