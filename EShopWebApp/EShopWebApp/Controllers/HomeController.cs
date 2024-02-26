using EShopWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using EShopWebApp.Core.Contracts;

namespace EShopWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IImageService _imageService;


        public HomeController(ILogger<HomeController> logger,IImageService imageService)
        {
            _logger = logger;
            _imageService = imageService;

        }

        public IActionResult Index()
        {
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
        

        [HttpPost]
        public async Task<IActionResult> UploadFile(IEnumerable<IFormFile> file)
        {
            string fileName = file?.FirstOrDefault()?.FileName;
            
           await _imageService.UploadImageAsync(file,fileName);



            return Ok();
        }
    }
}
