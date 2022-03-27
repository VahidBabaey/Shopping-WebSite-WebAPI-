using System;
using System.Collections.Generic;
using System.Text;

namespace Reddington.Services.DTOs
{
    public class ProductCategoryDTO : BaseDTO
    {
        public int ProductID { get; set; }
        public int CategoryID { get; set; }
    }
}
