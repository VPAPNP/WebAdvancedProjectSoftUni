using EShopWebApp.Core.Contracts;
using EShopWebApp.Core.ViewModels.ProductViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EShopWebApp.Areas.Admin.Controllers
{
    [Authorize]
    public class ProductController : BaseAdminController
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IBrandService _brandService;
        private readonly IPhotoService _imageService;

        public ProductController(IProductService productService, ICategoryService categoryService, IBrandService brandService, IPhotoService imageService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _brandService = brandService;
            _imageService = imageService;
        }
        public async Task<IActionResult> Create(ProductCreateFormViewModel productCreateForm)
        {
            var categories = await _categoryService.GetAllAsync();
            var brands = await _brandService.GetAllAsync();

            productCreateForm.Categories = categories;
            productCreateForm.Brands = brands;
            if (!User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Home");
            }

            if (!User.Identity!.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(productCreateForm);
        }

        [HttpPost]
        public async Task<IActionResult> Create(IFormFile file,ProductCreateViewModel productView)
        {
            if (!User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Home");
            }

            if (!User.Identity!.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
           
            
            if (!ModelState.IsValid)
            {
                var categories = await _categoryService.GetAllAsync();
                var brands = await _brandService.GetAllAsync();
                var productCreateForm = new ProductCreateFormViewModel
                {
                    Name = productView.Name,
                    Description = productView.Description,
                    Price = productView.Price,
                    StockQuantity = productView.StockQuantity,
                    BrandId = productView.BrandId,
                    CreatedOn = productView.CreatedOn,
                    CategoryId = productView.CategoryId,
                    Categories = categories,
                    Brands = brands
                };
                return View(productCreateForm);
            }


            await _productService.CreateAsync(file,productView);

            return RedirectToAction("All", "Product");
        }

        public async Task<IActionResult> All()
        {
            var products = await _productService.GetAllAsync();
            return View(products);
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            await _productService.DeleteAsync(id);
            return RedirectToAction("All", "Product");
        }

        public async Task<IActionResult> Edit(string id)
        {
            var product = await _productService.GetByIdAsync(Guid.Parse(id));
            var categories = await _categoryService.GetAllAsync();
            var brands = await _brandService.GetAllAsync();
            var photo = await _imageService.GetPhotoById(Guid.Parse(product.PhotoId));
            var productEditForm = new ProductEditFormViewModel
            {

                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                BrandId = product.BrandId,
                CreatedOn = product.CreatedOn,
                CategoryId = product.CategoryId.ToString()!,
                Categories = categories,
                PhotoId =Guid.Parse( product.PhotoId),
                Brands = brands
               

            };
            return View(productEditForm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(IFormFile file ,string id, ProductEditViewModel editProductModel)
        {
            //var image = await _imageService.GetPhotoByName(editProductModel.PhotoName);

            //if (image == null) 
            //{
            //    image = _imageService.CreateImage(file, file.FileName);
            //}
            

            if (!ModelState.IsValid)
            {
                var categories = await _categoryService.GetAllAsync();
                var brands = await _brandService.GetAllAsync();
                
                var productEditForm = new ProductEditFormViewModel
                {
                    Name = editProductModel.Name,
                    Description = editProductModel.Description,
                    Price = editProductModel.Price,
                    PhotoId = editProductModel.ImageId,
                    StockQuantity = editProductModel.StockQuantity,
                    BrandId = editProductModel.BrandId,
                    CategoryId = editProductModel.CategoryId,
                    Categories = categories,
                    Brands = brands
                };
                return View(productEditForm);
            }
            await _productService.EditAsync(file,Guid.Parse(id), editProductModel);
            return RedirectToAction("All", "Product");
        }
    }
}
