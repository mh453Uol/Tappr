using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CycleHire.Services
{
    public class ImageUploader : IImageUploader
    {
        public AuthImageUploaderOptions Options { get; }
        private readonly Account Account;
        private readonly Cloudinary Cloundinary;
        private const string DefaultProfileImageId = "default-profile_ohrm32";
        private const string DefaultBikeImageId = "image_wp4zfb";

        public ImageUploader(IOptions<AuthImageUploaderOptions> optionsAccessor)
        {
            Options = optionsAccessor.Value;

            Account = new Account(Options.CloudinaryCloudId,
               Options.CloudinaryApiKey, Options.CloudinaryApiSecret);

            Cloundinary = new Cloudinary(Account);
        }

        public async Task<string[]> UploadImagesAsync(List<IFormFile> images)
        {
            return await UploadImagesViaCloundinary(images);
        }

        public async Task<string> UploadImageAsync(IFormFile image)
        {
            if (image.Length > 0)
            {
                var file = new ImageUploadParams()
                {
                    File = new FileDescription(image.FileName, image.OpenReadStream())
                };

                var id = await Cloundinary.UploadAsync(file);

                return id.PublicId;
            };

            return null;
        }
        private async Task<string[]> UploadImagesViaCloundinary(List<IFormFile> images)
        {
            return await Task.WhenAll(images.Select(image => UploadImageAsync(image)));
        }

        public string GetBikeImageById(string publicId, int? height, int? width)
        {
            // Load default
            if (String.IsNullOrEmpty(publicId))
            {
                return Cloundinary.Api.UrlImgUp
                    .Transform(new CloudinaryDotNet.Transformation()
                            .Width(690)
                            .Height(690)
                            .Crop("fill"))
                            .BuildUrl(DefaultProfileImageId);
            }

            if (width != null && height != null)
            {
                return Cloundinary.Api.UrlImgUp
                    .Transform(new CloudinaryDotNet.Transformation()
                            .Width(width)
                            .Height(height)
                            .Crop("fill"))
                            .BuildUrl(publicId);
            }

            return Cloundinary.Api.UrlImgUp.BuildUrl(publicId);
        }

        public string GetProfileImageById(string publicId, int? height = null, int? width = null)
        {
            if (String.IsNullOrEmpty(publicId))
            {
                return Cloundinary.Api.UrlImgUp
                    .Transform(new CloudinaryDotNet.Transformation()
                    .Width(height)
                    .Height(width)
                    .Crop("fill"))
                    .BuildUrl(DefaultProfileImageId);
            }

            if (width != null && height != null)
            {
                return Cloundinary.Api.UrlImgUp
                    .Transform(new CloudinaryDotNet.Transformation()
                            .Width(width)
                            .Height(height)
                            .Crop("fill"))
                            .BuildUrl(publicId);
            }

            return Cloundinary.Api.UrlImgUp.BuildUrl(publicId);
        }
    }
}
