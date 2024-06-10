using EShopWebApp.Core.Contracts;
using EShopWebApp.Core.ViewModels.CartViewModels;
using Microsoft.AspNetCore.Mvc;

using Stripe.Checkout;
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
        private readonly IConfiguration _configuration;
        private readonly IEcontService _econtService;
        


        public CartController(
            ICartService cartService,  
            IProductService productService,
            ICategoryService categoryService,
            IBrandService brandService,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,IEcontService econtService)
        {
            _cartService = cartService;
            _productService = productService;
            _categoryService = categoryService;
            _brandService = brandService;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _econtService = econtService;
            
        }
        public async Task<IActionResult> Index()
        {
            CartViewModel cartView = new CartViewModel();
            if (!User.Identity!.IsAuthenticated)
            {
                string curSessionId = _httpContextAccessor.HttpContext!.Request.Cookies["ShoppingCartSessionId"]!;
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
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
                await _cartService.AddProductToCartAsync(id, userId!);
            }
            return RedirectToAction("All","Product");
        }
        public async Task<IActionResult> RemoveFromCart(Guid id)
        {
            await _cartService.RemoveAllProductsFromCartAsync();
            CartViewModel cartView = new CartViewModel();
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!User.Identity!.IsAuthenticated)
            {
                await _cartService.RemoveProduct(id,userId!);
                string sessionId = _httpContextAccessor.HttpContext!.Request.Cookies["ShoppingCartSessionId"]!;
                cartView = await _cartService.GetGuestCartAsync(sessionId!);
            }
            else 
            {
                
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
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
                cartView = await _cartService.AddProductToCartAsync(id, userId!);
            }
            return RedirectToAction("Index", "Cart");
        }
        public async Task<IActionResult> Checkout()
        {
            CartViewModel cartView = new CartViewModel();
            if (!User.Identity!.IsAuthenticated)
            {
                string curSessionId = _httpContextAccessor.HttpContext!.Request.Cookies["ShoppingCartSessionId"]!;
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
            var products = cartView.ShoppingCartItems;

            var domain = "https://eshopwebappproject.azurewebsites.net";
            var options = new SessionCreateOptions
            {

                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                SuccessUrl = domain + "/Cart/CheckoutSuccess",
                CancelUrl = domain + "/Cart/CheckoutCancel",

            };
            if (User.Identity!.IsAuthenticated)
            {
                
                options.CustomerEmail = User.FindFirstValue(ClaimTypes.Email);
            }
           
            foreach (var product in products)
            {
                var productItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = product.Product.Name,
                        },
                        UnitAmount = (long)product.Product.Price*100,
                    },
                    Quantity = product.Quantity,
                };
                options.LineItems.Add(productItem);
            }
            var service = new SessionService();
            Session session = service.Create(options);
            Response.Headers.Append("Location", session.Url);

            return new StatusCodeResult(303);
		}

    }
   
}
