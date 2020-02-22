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
        private readonly IFormFile formFile;
        public UploadImage(IConfiguration configuration, IFormFile formFile)
        {
            this.configuration = configuration;
            this.formFile = formFile;
        }
        public string Upload(IFormFile formFile)
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
                var file = formFile.FileName;
                var stream = formFile.OpenReadStream();

                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(file, stream)
                };

                var uploadResult = cloudinary.Upload(uploadParams);
                return uploadResult.Uri.ToString();
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }
    }
}
