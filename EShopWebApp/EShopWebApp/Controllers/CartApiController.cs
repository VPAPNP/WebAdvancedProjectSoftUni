using EShopWebApp.Core.Contracts;
using EShopWebApp.Core.ViewModels.CartViewModels;
using EShopWebApp.Core.ViewModels.ProductViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;
using System.Security.Claims;


namespace EShopWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartApiController : ControllerBase
    {
        private readonly ICartService _cartService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<CartApiController> _logger;

        public CartApiController(ICartService cartService, IHttpContextAccessor httpContextAccessor, ILogger<CartApiController> logger)
        {
            _cartService = cartService;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        // GET: api/<CartApiController>
        [HttpGet("getcart")]
        public async Task<IEnumerable<ProductAllViewModel>> Get()
        {
            CartViewModel cart = new CartViewModel();
            var list = new List<ProductAllViewModel>();
            if (User.Identity!.IsAuthenticated) 
            {
                
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                cart = await _cartService.GetCartAsync(userId!);
                list = cart.ShoppingCartItems.Select(x => new ProductAllViewModel
                {
                    Id = x.ProductId.ToString(),
                    Name = x.Product.Name,
                    Price = x.Product.Price,
                    StockQuantity = x.Quantity
                }).ToList();

               return list;
            }
            
            return list ;
        }

        

        // POST api/<CartApiController>
        [HttpPost("addtocart")]
        [ProducesResponseType<string>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task Post([FromBody] string id)
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    await _cartService.AddProductToCartAsync(Guid.Parse(id), userId);
                    
                }
                else
                {


                    await _cartService.AddProductToGuestCartAsync(Guid.Parse(id));
                    
                }
               
               
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the request. api/addtocart");
                throw; 
                
            }
           
        }

        

        
        [HttpDelete("removefromcart")]
        public async Task DeleteItem([FromBody]string id)
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
               await _cartService.RemoveProduct(Guid.Parse(id), userId);
            }
            else
            {
                
                await _cartService.RemoveGuestProduct(Guid.Parse(id));
            }
        }
        [HttpDelete("removecartitem")]
        public async Task DeleteCartItem([FromBody] string id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _cartService.RemoveShoppingCartItemsAsync(id,userId);

            await Console.Out.WriteLineAsync();

        }


    }
}
