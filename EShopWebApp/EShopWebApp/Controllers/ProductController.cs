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
        

        public ProductController(IProductService productService,ICategoryService categoryService,IBrandService brandService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _brandService = brandService;
            
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

            var productDetailsViewModel = new ProductDetailsViewModel
            {
                Product = product,
                RelatedProducts = relatedProducts.ToList()
            };
            return View(productDetailsViewModel);
        }
        

        

        
    }
}
