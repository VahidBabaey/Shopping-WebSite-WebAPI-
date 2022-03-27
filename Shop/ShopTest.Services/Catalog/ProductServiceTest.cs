using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Reddington.Core.Domian;
using Reddington.Data;
using Reddington.Services.Catalog;
using Reddington.Services.DTOs;
using Shop.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopTest.Services.Catalog
{
    [TestClass]
    public class ProductServiceTest
    {
        IProductService productService = null;
        ProductController productController = null;
        Mock<IRepository<Product>> mockRepositoryProduct = null;
        Mock<IProductService> mockProductService = null;

        [TestInitialize]
        public void TestInitialize()
        {
            mockRepositoryProduct = new Mock<IRepository<Product>>();
            var product = new Product()
            {
                ID = 1,
                Price = 1000,
                ProductName = "Prod1"
            };

            var category = new Category()
            {
                ID = 1,
                Name = "Cat1",
                DiscountAmount = 30
            };

            var productCategory = new ProductCategory()
            {
                CategoryID = 1,
                ProductID = 1,
                Product = product,
                Category = category
            };

            product.ProductCategories = new List<ProductCategory>();
            product.ProductCategories.Add(productCategory);
            mockRepositoryProduct.Setup(x => x.GetByID(It.IsAny<int>())).Returns(product);
            productService = new ProductService(mockRepositoryProduct.Object, null, null, null);
            mockProductService = new Mock<IProductService>();
            var _listProduct = Task.Run<IEnumerable<ProductListItemDTO>>(() =>
            {
                var listprods = new List<ProductListItemDTO>();

                listprods.Add(new ProductListItemDTO()
                {
                    ID = 1,
                    Price = 1000,
                    Sku = "5756"
                });

                return listprods;

            });
            mockProductService.Setup(p => p.SearchAllProductsAsync()).Returns(_listProduct);
            productController = new ProductController(mockProductService.Object, null, null);

        }
        [TestMethod]
        public void GetListAsyncTest()
        {


            var result = productController.GetListAsync().Result;

            if (result is ObjectResult)
            {
                var data = result as ObjectResult;

                var data2 = data.Value as IEnumerable<ProductListItemDTO>;


                //Assert.IsInstanceOfType(data.Value, typeof(IEnumerable<ProductListItem>));
                Assert.IsTrue(data2.Count() > 0);
            }
            else
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void GetProductNameTest()
        {
            
            var res = productService.GetProductName(1);
            Assert.AreEqual("Mobile", res);
        }
        [TestMethod]
        public void GetPriceByDiscountTest()
        {

            var res = productService.GetPriceByDiscount(1);
            Assert.AreEqual(700, res);
        }
    }
}
