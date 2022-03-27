using System;
using System.Collections.Generic;
using System.Text;

namespace Reddington.Services.DTOs
{
    public class PictureDTO : BaseEntityDTO
    {
        public string VirtualPath { get; set; }
        public string MimeType { get; set; }
    }
}
