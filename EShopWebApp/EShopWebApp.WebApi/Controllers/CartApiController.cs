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

        public CartApiController(IProductService productService)
        {
           
            _productService = productService;
        }

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(200, Type = typeof(ProductViewModel))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Get(string id)
        {

            try
            {
                var product = await _productService.GetByIdAsync(Guid.Parse(id));
                return this.Ok(product);
            }
            catch (Exception )
            {
                return this.BadRequest();
            }

           
           
           
        }
        [HttpPost]
        public IActionResult YourAction([FromBody] ProductViewApiModel item)
        {
            // Process the received JSON object
            // Access model properties here

            // Return appropriate response
            return Ok(); // or any other IHttpActionResult depending on your logic
        }
    }
}

