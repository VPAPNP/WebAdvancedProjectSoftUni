using Microsoft.AspNetCore.Http;

namespace EShopWebApp.Core.Contracts
{
    public interface IImageService
    {
        Task <string> UploadImageAsync(IEnumerable<IFormFile> imageFile, string fileName);

        Task DownloadImageAsync(Guid Id);
    }
}
