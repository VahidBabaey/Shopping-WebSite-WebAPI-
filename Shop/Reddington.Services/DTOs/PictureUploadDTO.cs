using Microsoft.AspNetCore.Http;
using Reddington.Services.Validators;
using System;
using System.Collections.Generic;
using System.Text;

namespace Reddington.Services.DTOs
{
    public class PictureUploadDTO : BaseDTO
    {
        [ImageValidation(ErrorMessage = "فرمت عکس صحیح نیست")]
        public IFormFile File { get; set; }
        public string ContentType { get; set; }
        public string fileExtension { get; set; }
    }
}
