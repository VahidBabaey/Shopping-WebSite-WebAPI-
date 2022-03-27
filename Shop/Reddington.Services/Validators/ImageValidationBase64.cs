using Microsoft.AspNetCore.Http;
using Reddington.Services.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace Reddington.Services.Validators
{
    public class ImageValidationBase64 : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return new ValidationResult(ErrorMessage);
            var image = value as PictureUploadBase64DTO;
            var format = image.File.Split(",")[0];

            try
            {
                var buffer = Convert.FromBase64String(image.File.Split(",")[1]);
                MemoryStream memory = new MemoryStream(buffer);
                Image.FromStream(memory);
            }
            catch (Exception ex)
            {
                return new ValidationResult(ErrorMessage);
            }


            image.File = image.File.Split(",")[1];

            var ImageContentType = new List<string>
            {
                "image/jpg",
                "image/jpeg",
                "image/gif",
                "image/png",
            } as IReadOnlyCollection<string>;

            var ImageExtension = new List<string>
            {
                ".jpg",
                ".png",
                ".gif",
                ".jpeg",
            } as IReadOnlyCollection<string>;

            var contentType = ImageContentType.FirstOrDefault(p => format.Contains(p));

            if (contentType == null)
                return new ValidationResult(ErrorMessage);


            var fileExtension = ImageExtension.FirstOrDefault(p => contentType.Contains(p.Replace(".", "")));

            if (!string.IsNullOrEmpty(fileExtension))
                fileExtension = fileExtension.ToLowerInvariant();

            return ValidationResult.Success;
        }
    }
}
