using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;

namespace CommonLayer.ImageUpload
{
    public class UploadImage
    {
        private readonly IConfiguration configuration;
        private readonly IFormFile file;
        public UploadImage(IConfiguration configuration, IFormFile file)
        {
            this.configuration = configuration;
            this.file = file;
        }
        public string Upload(IFormFile file)
        {
            try
            {
                var cloudeName = configuration["Cloudinary:CloudName"];
                var keyName = configuration["Cloudinary:APIKey"];
                var secretKey = configuration["Cloudinary:APISecret"];

                Account account = new Account()
                {
                    Cloud = cloudeName,
                    ApiKey = keyName,
                    ApiSecret = secretKey
                };

                Cloudinary cloudinary = new Cloudinary(account);
                var fileValue = file.FileName;
                var stream = file.OpenReadStream();

                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(fileValue, stream)
                };

                var uploadResult = cloudinary.Upload(uploadParams);
                 string uri=uploadResult.Uri.ToString();
                return uri;
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }
    }
}
