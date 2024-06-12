using EShopWebApp.Core.Contracts;
using EShopWebApp.Core.ViewModels.PhotoViewModels;
using Microsoft.AspNetCore.Mvc;



namespace EShopWebApp.Areas.Admin.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class PhotoApiController : ControllerBase
    {
        private readonly IPhotoService _photoService;

        public PhotoApiController(IPhotoService photoService)
        {
            _photoService = photoService;
        }
        [HttpGet()]
        public void Get()
        {
            Console.WriteLine();
        }


        [HttpGet("getproductphotos")]
        public async Task<ICollection<PhotoViewModel>> Get(string id)
        {
            var photos = await _photoService.GetAllPhotosByProductId(Guid.Parse(id));

            return photos;

        }



        [HttpDelete("deletephoto")]
        public async Task<IActionResult> Delete([FromBody] string id)
        {


            await _photoService.DeletePhotoAsync(Guid.Parse(id));

            return Ok();
        }
    }
}
