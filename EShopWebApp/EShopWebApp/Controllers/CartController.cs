using EShopWebApp.Core.Contracts;
using EShopWebApp.Core.ViewModels.CartViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EShopWebApp.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
       
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IBrandService _brandService;

        
        public CartController(ICartService cartService,  IProductService productService,ICategoryService categoryService,IBrandService brandService)
        {
            _cartService = cartService;
            
            _productService = productService;
            _categoryService = categoryService;
            _brandService = brandService;

            
        }
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var cartView = await _cartService.GetCartAsync(userId!);


            return View(cartView);
            


            
        }
        
        public async Task<IActionResult> AddToCart(Guid id)
        {
            if (!User.Identity!.IsAuthenticated)
            {
                return LocalRedirect("/Identity/Account/Login");



            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            
            
            var cartView = await _cartService.AddProductToCartAsync(id, userId!);
            return View("Index", cartView);
        }
        public async Task<IActionResult> RemoveFromCart(Guid id)
        {
            if (!User.Identity!.IsAuthenticated)
            {
                return LocalRedirect("/Identity/Account/Login");
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _cartService.RemoveProduct(id,  userId!);

            var cartView = await _cartService.GetCartAsync(userId!);
            return View("Index", cartView);
            
        }


    }
   
}
