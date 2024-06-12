using EShopWebApp.Core.ViewModels.PhotoViewModels;
using EShopWebApp.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Http;

namespace EShopWebApp.Core.Contracts
{
    public interface IPhotoService
    {
        PhotoViewModel CreateImage(IFormFile imageFile, string fileName);

        Task DownloadPhotoAsync(Guid Id);

        Task<PhotoViewModel> GetPhotoById(Guid Id);

        Task<PhotoViewModel> GetPhotoByName(string name);

        Task DeletePhotoAsync(Guid id);

        Task<ICollection<PhotoViewModel>> GetPhotoByProductId(Guid id);

        Task UploadImagesToProductAsync(Guid productId, IEnumerable<IFormFile> images);
        Task<ICollection<PhotoViewModel>> GetAllPhotosByProductId(Guid productId);
    }
}
