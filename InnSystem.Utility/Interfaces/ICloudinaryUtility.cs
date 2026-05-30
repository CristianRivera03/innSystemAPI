using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace InnSystem.Utility.Interfaces
{
    public interface ICloudinaryUtility
    {
        Task<string> SubirImagenAsync(IFormFile archivo, string carpeta);
        Task<bool> EliminarImagenAsync(string publicId);
    }
}
