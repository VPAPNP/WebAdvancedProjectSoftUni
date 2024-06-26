﻿using EShopWebApp.Core.Services.ServiceModels;
using EShopWebApp.Core.ViewModels.ProductViewModels;
using Microsoft.AspNetCore.Http;

namespace EShopWebApp.Core.Contracts
{
    public interface IProductService
    {
        Task<ICollection<ProductAllViewModel>> GetAllAsync();

        Task<ProductAllViewModel> GetByIdAsync(Guid id);

        Task CreateAsync(IEnumerable<IFormFile>files, IFormFile file,ProductCreateViewModel productCreateViewModel);
        Task DeleteAsync(Guid id);

        Task EditAsync(IEnumerable<IFormFile> files, IFormFile file,Guid id,ProductEditViewModel productEditViewModel);

        Task<AllProductsFilteredAndPagedServiceModel> GetAllFilteredAndPagedAsync(AllProductsQueryModel productsQueryModel);

        Task<ICollection<ProductAllViewModel>> GetLastThreeAddedAsync();

        Task<ICollection<ProductAllViewModel>> GetRelatedProductsAsync(Guid categoryId);

    }
}
