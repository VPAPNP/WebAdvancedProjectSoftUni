using EShopWebApp.Core.Contracts;
using EShopWebApp.Core.ViewModels.CartViewModels;
using EShopWebApp.Infrastructure.Data.Models;
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
        private readonly IHttpContextAccessor _httpContextAccessor;


        public CartController(
            ICartService cartService,  
            IProductService productService,
            ICategoryService categoryService,
            IBrandService brandService,
            IHttpContextAccessor httpContextAccessor)
        {
            _cartService = cartService;
            _productService = productService;
            _categoryService = categoryService;
            _brandService = brandService;
            _httpContextAccessor = httpContextAccessor;

            
        }
        public async Task<IActionResult> Index()
        {
            CartViewModel cartView = new CartViewModel();
            if (!User.Identity!.IsAuthenticated)
            {
                string curSessionId = _httpContextAccessor.HttpContext.Request.Cookies["ShoppingCartSessionId"];
                if (string.IsNullOrEmpty(curSessionId)) 
                {
                    curSessionId = await _cartService.CreateShoppingCartSession();
                   
                }
                cartView = await _cartService.GetGuestCartAsync(curSessionId);
            }
            else
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                cartView = await _cartService.GetCartAsync(userId!);
            }

            return View(cartView);
            
        }
        
        public async Task<IActionResult> AddToCart(Guid id)
        {
            if (!User.Identity!.IsAuthenticated)
            {
                 await _cartService.AddProductToGuestCartAsync(id);

            }
            else
            {
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                await _cartService.AddProductToCartAsync(id, userId!);
            }
            return RedirectToAction("All","Product");
        }
        public async Task<IActionResult> RemoveFromCart(Guid id)
        {
            CartViewModel cartView = new CartViewModel();
            if (!User.Identity!.IsAuthenticated)
            {
                await _cartService.RemoveGuestProduct(id);
                string sessionId = _httpContextAccessor.HttpContext.Request.Cookies["ShoppingCartSessionId"];
                cartView = await _cartService.GetGuestCartAsync(sessionId);
            }
            else 
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                await _cartService.RemoveProduct(id, userId!);
                cartView = await _cartService.GetCartAsync(userId!);
            }
            
            return View("Index", cartView);
            
        }
        public async Task<IActionResult> BuyItNow(Guid id)
        {
            CartViewModel cartView = new CartViewModel();
            if (!User.Identity!.IsAuthenticated)
            {
                cartView = await _cartService.AddProductToGuestCartAsync(id);
            }
            else
            {
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                cartView = await _cartService.AddProductToCartAsync(id, userId!);
            }
            return RedirectToAction("Index", "Cart");
        }


    }
   
}
