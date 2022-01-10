using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CycleHire.Services
{
    public interface IImageUploader
    {
        Task<string[]> UploadImagesAsync(List<IFormFile> images);
        Task<String> UploadImageAsync(IFormFile image);
        string GetBikeImageById(string publicId, int? height = null, int? width = null);
        string GetProfileImageById(string publicId, int? height = null, int? width = null);
    }
}
