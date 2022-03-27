using Reddington.Core.Tasks;
using Reddington.Services.Catalog;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Reddington.Services.Catalog;

namespace Reddington.Services.Catalog
{
    public class StockQuantityTask : ITaskSchduler
    {
        private readonly IProductService _productService;
        private readonly ILogger<StockQuantityTask> _logger;

        public StockQuantityTask(IProductService productService, ILogger<StockQuantityTask> logger)
        {
            _productService = productService;
            _logger = logger;

            IsActiveInStartup = true;
            Cron = "*/5 * * * * *";
        }

        public bool IsActiveInStartup { get; set; }
        public string Cron { get; set; }

        public void Run()
        {
            var _list = _productService.SearchUnAvailableProductAsync().Result;
            if(_list.Count()>0)
            {
                foreach (var item in _list)
                {
                    _logger.LogInformation(" The Product {0} Is Low Stock Quantity", item.ProductName);

                }
            }
        }
    }
}
