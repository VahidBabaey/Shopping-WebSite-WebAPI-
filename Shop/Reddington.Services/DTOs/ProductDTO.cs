using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Reddington.Services.DTOs
{
    public class ProductDTO : BaseEntityDTO
    {
        //[ModelBinder(BinderType = typeof(MyBinder))]
        public string ProductName { get; set; }
        public int Price { get; set; }

        //[ModelBinder(BinderType = typeof(MyBinder))]
        public string Sku { get; set; }
        public int StockQuantity { get; set; }
        public DateTime PublishDate { get; set; }
    }

}
