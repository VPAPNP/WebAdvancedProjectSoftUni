using EShopWebApp.Core.Contracts;
using EShopWebApp.Core.Services.ServiceModels;
using EShopWebApp.Core.ViewModels.ProductViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EShopWebApp.Controllers
{

    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IBrandService _brandService;
        private readonly IPhotoService _photoService;
        

        public ProductController(IProductService productService,ICategoryService categoryService,IBrandService brandService,IPhotoService photoService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _brandService = brandService;
            _photoService = photoService;
            
        }
        

        public async Task<IActionResult> All([FromQuery]AllProductsQueryModel queryModel)
        {
            AllProductsFilteredAndPagedServiceModel serviceModel = await _productService.GetAllFilteredAndPagedAsync(queryModel);
            
            queryModel.Products = serviceModel.Products;
            queryModel.TotalProducts = serviceModel.TotalProducts;
           
            queryModel.Categories = await _categoryService.GetAllNamesAsync();
            queryModel.Brands = await _brandService.GetAllNamesAsync();

            return View(queryModel);
        }
        public async Task<IActionResult> Details(Guid id)
        {
            var product = await _productService.GetByIdAsync(id);

            var relatedProducts = await _productService.GetRelatedProductsAsync(Guid.Parse(product.CategoryId));

            var photos = await _photoService.GetPhotoByProductId(id);

            

            var productDetailsViewModel = new ProductDetailsViewModel
            {
                Product = product,
                RelatedProducts = relatedProducts.ToList(),
                Photos = photos.ToList()
            };
            return View(productDetailsViewModel);
        }
        

        

        
    }
}
