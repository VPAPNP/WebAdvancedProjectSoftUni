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
    public class ImageService : IImageService
    {
        private readonly ApplicationDbContext db;

        public ImageService(ApplicationDbContext db)
        {
            this.db = db;

        }

        public ImageViewModel CreateImage(IFormFile imageFile, string fileName)
        {
            
            
                MemoryStream ms = new MemoryStream();
                imageFile.CopyTo(ms);
                ImageViewModel img = new ImageViewModel()
                            {
                                Name = imageFile.FileName,
                                Picture = ms.ToArray()


                            };
                            
                
                ms.Close();
                ms.Dispose();


            return img;
        }

        public Task DownloadImageAsync(Guid Id)
        {
            throw new NotImplementedException();
        }

        public async Task<ImageViewModel> GetImageById(Guid Id)
        {
            var image = await db.Photos.FindAsync(Id);


            ImageViewModel imageViewModel = new ImageViewModel()
            {
                Name = image.Name,
                Picture = image.Picture
            };
            return imageViewModel;
        }
    }
}
