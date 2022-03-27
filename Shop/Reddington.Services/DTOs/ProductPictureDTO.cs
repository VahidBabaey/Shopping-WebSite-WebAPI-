﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Reddington.Services.DTOs
{
    public class ProductPictureDTO : BaseDTO
    {
        public int ProductID { get; set; }
        public int PictureID { get; set; }
        public int DisplayOrder { get; set; }
    }
}
