﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;

namespace Reddington.Services.Validators
{
    public class ImageValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {

            if (value == null)
                return new ValidationResult(ErrorMessage);

            var ImageContentType = new List<string>
            {
                "image/jpg",
                "image/jpeg",
                ".image/gif",
                "image/png",
            } as IReadOnlyCollection<string>;

            var ImageExtension = new List<string>
            {
                ".jpg",
                ".png",
                ".gif",
                ".jpeg",
            } as IReadOnlyCollection<string>;

            var image = value as IFormFile;
            var contentType = image.ContentType;


            var fileExtension = Path.GetExtension(image.FileName);

            if (!string.IsNullOrEmpty(fileExtension))
                fileExtension = fileExtension.ToLowerInvariant();

            if (!ImageContentType.Contains(contentType) || !ImageExtension.Contains(fileExtension))
            {
                return new ValidationResult(ErrorMessage);
            }
            return ValidationResult.Success;
        }
    }
}
