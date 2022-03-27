using Reddington.Services.Validators;
using System;
using System.Collections.Generic;
using System.Text;

namespace Reddington.Services.DTOs
{
    public class PictureUploadBase64DTO
    {
        //[ImageValidationBase64(ErrorMessage = "فرمت عکس صحیح نیست")]
        public string File { get; set; }
        public string ContentType { get; set; }
        public string fileExtension { get; set; }
    }
}
