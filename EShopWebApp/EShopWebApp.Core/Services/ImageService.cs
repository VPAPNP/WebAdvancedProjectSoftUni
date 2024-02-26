using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Core;
using EShopWebApp.Core.Contracts;
using EShopWebApp.Infrastructure.Data;
using EShopWebApp.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Http;

namespace EShopWebApp.Core.Services
{
    public class ImageService : IImageService
    {
        private readonly ApplicationDbContext db;

        public ImageService(ApplicationDbContext db)
        {
            this.db = db;

        }

        public Task<string> UploadImageAsync(IEnumerable<IFormFile> imageFile, string fileName)
        {
            foreach (var file in imageFile)
            {
                MemoryStream ms = new MemoryStream();
                file.CopyTo(ms);
                Image img = new Image()
                {
                    Name = file.FileName,
                    Picture = ms.ToArray()


                };
                
                ms.Close();
                ms.Dispose();

                db.Image.Add(img);
                db.SaveChanges();
            }

            return Task.FromResult("Image uploaded successfully");
        }

        public Task DownloadImageAsync(Guid Id)
        {
            var image = db.Image.FirstOrDefault(x => x.Id == Id);

            MemoryStream ms = new MemoryStream(image.Picture);

            var picture =
            
        }
    }
}
