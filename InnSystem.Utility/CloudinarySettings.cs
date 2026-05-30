using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using InnSystem.Model;
using InnSystem.Utility.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace InnSystem.Utility
{
    public class CloudinaryUtility : ICloudinaryUtility
    {
        private readonly Cloudinary _cloudinary;
        public CloudinaryUtility(IOptions<CloudinarySettings> config)
        {
            var account = new Account(
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret
            );
            _cloudinary = new Cloudinary(account);
        }
        public async Task<string> SubirImagenAsync(IFormFile archivo, string carpeta)
        {
            if (archivo == null || archivo.Length == 0)
                return string.Empty;
            var uploadResult = new ImageUploadResult();
            using (var stream = archivo.OpenReadStream())
            {
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(archivo.FileName, stream),
                    Folder = $"InnSystem/{carpeta}",
                    Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face")
                };
                uploadResult = await _cloudinary.UploadAsync(uploadParams);
            }
            return uploadResult.SecureUrl?.ToString() ?? string.Empty;
        }
        public async Task<bool> EliminarImagenAsync(string publicId)
        {
            if (string.IsNullOrEmpty(publicId)) return false;
            var deletionParams = new DeletionParams(publicId);
            var result = await _cloudinary.DestroyAsync(deletionParams);
            return result.Result == "ok";
        }
    }
}
