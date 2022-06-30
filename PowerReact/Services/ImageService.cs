using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using PowerReact.Data;

namespace PowerReact.Services
{
    public class ImageService
    {
        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _env;
        private readonly DataContext _context;

        public ImageService(IConfiguration config,  IWebHostEnvironment env, DataContext context )
        {
            _config = config;
            _env = env;
            _context = context;
        }

        public async Task<Boolean> AddImageAsync(IFormFile file)
        {

            if (file.Length > 0) {
                string filePath = Path.Combine(_env.ContentRootPath , "Images", file.FileName);
                if (File.Exists(filePath))
                {
                    using (Stream fileStream = new FileStream(filePath, FileMode.Open)) {
                        await file.CopyToAsync(fileStream);
                    }
                }
                else
                {
                    using (Stream fileStream = new FileStream(filePath, FileMode.Create)) {
                        await file.CopyToAsync(fileStream);
                    }
                }

                return true;
            }

            return false;
        }

        public async Task<Boolean> DeleteImageAsync(string publicId)
        {
            var product = await _context.Products.FindAsync(publicId);
            if (product == null) return false;
            var productFullName = product.PictureUrl.Split("/");
            var productName = productFullName[^1];

            string filePath = Path.Combine(_env.ContentRootPath + "Images", productName);
            File.Delete(filePath);
            return true;
        }

        public async Task<Tuple<byte[],string>> GetImage(string imageName)
        {
            string filePath = Path.Combine(_env.ContentRootPath + "Images", imageName);
            byte[] bytes = await System.IO.File.ReadAllBytesAsync(filePath);
            string contentType = "";
            new FileExtensionContentTypeProvider().TryGetContentType(filePath, out contentType);
            return Tuple.Create<byte[], string>(bytes, contentType);
        }

    }
}