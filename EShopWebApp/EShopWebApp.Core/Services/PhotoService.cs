using EShopWebApp.Core.Contracts;
using EShopWebApp.Core.ViewModels.ImageViewModels;
using EShopWebApp.Infrastructure.Data;
using EShopWebApp.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace EShopWebApp.Core.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly ApplicationDbContext db;

        public PhotoService(ApplicationDbContext db)
        {
            this.db = db;

        }

        public PhotoViewModel CreateImage(IFormFile imageFile, string fileName)
        {
            
            
                MemoryStream ms = new MemoryStream();
                imageFile.CopyTo(ms);
                PhotoViewModel img = new PhotoViewModel()
                            {
                                Name = imageFile.FileName,
                                Picture = ms.ToArray()


                            };
                            
                
                ms.Close();
                ms.Dispose();


            return img;
        }
        //TODO: Implement DownloadPhotoAsync    
        public Task DownloadPhotoAsync(Guid Id)
        {
            throw new NotImplementedException();
        }

        public async Task<PhotoViewModel> GetPhotoById(Guid Id)
        {
            var photo = await db.Photos.FirstOrDefaultAsync(p => p.Id == Id);

            var photoViewModel = new PhotoViewModel()
            {
                Name = photo!.Name,
                Picture = photo.Picture
            };

            return photoViewModel;


        }

        public async Task<PhotoViewModel> GetPhotoByName(string name)
        {
            var photo = await db.Photos.FirstOrDefaultAsync(p => p.Name == name);

            var photoViewModel = new PhotoViewModel()
            {
                Name = photo!.Name,
                Picture = photo.Picture
            };

            return photoViewModel;
        }

        public async Task DeletePhotoAsync(Guid id)
        {
            var photo = await db.Photos.FirstOrDefaultAsync(p => p.Id == id);
            foreach (var item in db.Photos)
            {
                item.IsDeleted = true;
            }
            await db.SaveChangesAsync();
        }

        public async Task UploadImagesToProductAsync(Guid productId, IEnumerable<IFormFile> images)
        {
            foreach (var image in images)
            {
                var photo = CreateImage(image, image.FileName);
                var photoDb = new Photo()
                {
                    ProductId = productId,
                    Name = photo.Name,
                    Picture = photo.Picture
                };
                await db.Photos.AddAsync(photoDb);
                await db.SaveChangesAsync();
            }
            
        }

        public async Task<ICollection<PhotoViewModel>> GetPhotoByProductId(Guid id)
        {
            var photos = await db.Photos.Where(p => p.ProductId == id).ToListAsync();

            var photosViewModel = new List<PhotoViewModel>();

            foreach (var photo in photos) 
            {
                var photoViewModel = new PhotoViewModel()
                {
                    Name = photo.Name,
                    Picture = photo.Picture
                };
                photosViewModel.Add(photoViewModel);
            }

            return photosViewModel;
            
        }
    }
}
