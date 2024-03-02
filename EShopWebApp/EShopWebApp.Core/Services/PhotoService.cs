using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Core;
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

        public Task DownloadPhotoAsync(Guid Id)
        {
            throw new NotImplementedException();
        }

        public async Task<PhotoViewModel> GetPhotoById(Guid Id)
        {
            var photo = await db.Photos.FirstOrDefaultAsync(p => p.Id == Id);

            var photoViewModel = new PhotoViewModel()
            {
                Name = photo.Name,
                Picture = photo.Picture
            };

            return photoViewModel;


        }

        public async Task<PhotoViewModel> GetPhotoByName(string name)
        {
            var photo = await db.Photos.FirstOrDefaultAsync(p => p.Name == name);

            var photoViewModel = new PhotoViewModel()
            {
                Name = photo.Name,
                Picture = photo.Picture
            };

            return photoViewModel;
        }

        public async Task DeletePhotoAsync(Guid id)
        {
            var photo = await db.Photos.FirstOrDefaultAsync(p => p.Id == id);
            db.Photos.Remove(photo!);
            await db.SaveChangesAsync();
        }
    }
}
