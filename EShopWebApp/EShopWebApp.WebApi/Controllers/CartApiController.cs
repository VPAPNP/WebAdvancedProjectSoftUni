using System.Security.Claims;
using EShopWebApp.Core.Contracts;
using EShopWebApp.Core.ViewModels.CartViewModels;
using EShopWebApp.Core.ViewModels.ProductViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EShopWebApp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartApiController : ControllerBase
    {
        
        private readonly IProductService _productService;
        private readonly ICartService _cartService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartApiController(IProductService productService,
            ICartService cartService,
            IHttpContextAccessor httpContextAccessor
            )
        {
           
            _productService = productService;
            _cartService = cartService;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(200, Type = typeof(int))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Get()
        {

            return Ok();

           
           
           
        }
       
    }
}

