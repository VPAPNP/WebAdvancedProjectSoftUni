using EShopWebApp.Core.Contracts;
using EShopWebApp.Core.ViewModels.CartViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


namespace EShopWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartApiController : ControllerBase
    {
        private readonly ICartService _cartService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartApiController(ICartService cartService, IHttpContextAccessor httpContextAccessor)
        {
            _cartService = cartService;
            _httpContextAccessor = httpContextAccessor;
           
        }

        // GET: api/<CartApiController>
        [HttpGet("getcart")]
        public async Task<CartViewModel> Get()
        {
            CartViewModel cart = new CartViewModel();
            if (User.Identity.IsAuthenticated) 
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                cart = await _cartService.GetCartAsync(userId);

               return cart;
            }
            
            return cart ;
        }

        

        // POST api/<CartApiController>
        [HttpPost("addtocart")]
        public async Task Post([FromBody] string id)
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                await _cartService.AddProductToCartAsync(Guid.Parse(id), userId);
            }
            else
            {
                
                
               await _cartService.AddProductToGuestCartAsync( Guid.Parse(id));
            }
            
        }

        

        
        [HttpDelete("removefromcart")]
        public async Task Delete([FromBody]string id)
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
       
    }
}
