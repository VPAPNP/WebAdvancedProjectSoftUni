using EShopWebApp.Core.ViewModels.ImageViewModels;
using Microsoft.AspNetCore.Http;

namespace EShopWebApp.Core.Contracts
{
    public interface IImageService
    {
        ImageViewModel CreateImage(IFormFile imageFile, string fileName);

        Task DownloadImageAsync(Guid Id);

        Task<ImageViewModel> GetImageById(Guid Id);
    }
}
