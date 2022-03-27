using Microsoft.EntityFrameworkCore;
using Reddington.Core.Caching;
using Reddington.Core.Domian;
using Reddington.Core.Extensions;
using Reddington.Data;
using Reddington.Services.DTOs;
using Reddington.Services.Extentions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reddington.Services.Catalog
{
    public class ProductService : IProductService
    {
        #region Fields
        private readonly IRepository<Product> _repositoryProduct = null;
        private readonly IRepository<ProductCategory> _repositoryProductCategory = null;
        public readonly IRepository<ProductPicture> _repositoryProductPicture = null;
        public readonly ICacheManager _cacheManager = null;

        private string ProductSearchKey = "Product-categoryId-{0}-productName-{1}-sku-{2}";
        public ProductService(IRepository<Product> repositoryProduct, IRepository<ProductCategory> repositoryProductCategory, IRepository<ProductPicture> repositoryProductPicture, ICacheManager cacheManager)
        {
            _repositoryProduct = repositoryProduct;
            _repositoryProductCategory = repositoryProductCategory;
            _repositoryProductPicture = repositoryProductPicture;
            _cacheManager = cacheManager;
        }




        #endregion

        public async Task<IEnumerable<ProductListItemDTO>> SearchProductsAsync(ProductFilterDTO productFilterDTO)
        {
            ProductSearchKey = String.Format(ProductSearchKey, productFilterDTO.CategoryId, productFilterDTO.ProductName, productFilterDTO.Sku);
            var _list = await _cacheManager.GetAsych(ProductSearchKey, 35, async () =>
             {
                 var query = _repositoryProduct.TableNoTracking;
                 if (!string.IsNullOrEmpty(productFilterDTO.ProductName))
                 {
                     query = query.Where(p => p.ProductName.Contains(productFilterDTO.ProductName));
                 }
                 if (productFilterDTO.FromPrice.HasValue && productFilterDTO.FromPrice != 0)
                 {
                     query = query.Where(p => p.Price >= productFilterDTO.FromPrice);
                 }
                 if (productFilterDTO.ToPrice.HasValue && productFilterDTO.ToPrice != 0)
                 {
                     query = query.Where(p => p.Price <= productFilterDTO.FromPrice);
                 }
                 if (!string.IsNullOrEmpty(productFilterDTO.Sku))
                 {
                     query = query.Where(p => p.Sku == productFilterDTO.Sku);
                 }
                 if (productFilterDTO.IsAvailable.HasValue && productFilterDTO.IsAvailable.Value)
                 {
                     query = query.Where(p => p.StockQuantity > 0);
                 }
                 if (productFilterDTO.FromPublishDate != null)
                 {
                     query = query.Where(p => p.PublishDate >= productFilterDTO.FromPublishDate);
                 }
                 if (productFilterDTO.FromPublishDate != null)
                 {
                     query = query.Where(p => p.PublishDate <= productFilterDTO.ToPublishDate);
                 }
                 if (productFilterDTO.CategoryId.HasValue && productFilterDTO.CategoryId != 0)
                 {
                     query = query.Where(p => p.ProductCategories.Any(q => q.CategoryID == productFilterDTO.CategoryId));
                 }
                 return await _repositoryProduct.TableNoTracking.Where(p => !p.Deleted)
                     .Select(p => new ProductListItemDTO
                     {
                         CreateOn = p.CreateOn,
                         UpdateOn = p.UpdateOn,
                         LocalPublishDate = p.PublishDate.ToPersian(),
                         PublishDate = p.PublishDate,
                         ID = p.ID,
                         ProductName = p.ProductName,
                         Price = p.Price,
                         Sku = p.Sku,
                         StockQuantity = p.StockQuantity,
                         LocalCreateOn = p.CreateOn.ToPersian(),
                         LocalUpdateOn = p.UpdateOn.ToPersian(),
                         CategoryNames = p.ProductCategories.Select(x => x.Category.Name).ToList(),
                         CategoryIds = p.ProductCategories.Select(x => x.CategoryID).ToList()
                     }).ToListAsync();
             });
            return _list;
        }
        public async Task<IEnumerable<ProductListItemDTO>> SearchAllProductsAsync()
        {
            var _list = await _repositoryProduct.TableNoTracking.Where(p => !p.Deleted)
                .Select(p => new ProductListItemDTO
                {
                    CreateOn = p.CreateOn,
                    UpdateOn = p.UpdateOn,
                    LocalPublishDate = p.PublishDate.ToPersian(),
                    PublishDate = p.PublishDate,
                    ID = p.ID,
                    ProductName = p.ProductName,
                    Price = p.Price,
                    Sku = p.Sku,
                    StockQuantity = p.StockQuantity,
                    LocalCreateOn = p.CreateOn.ToPersian(),
                    LocalUpdateOn = p.UpdateOn.ToPersian(),
                    CategoryNames = p.ProductCategories.Select(x => x.Category.Name).ToList(),
                    CategoryIds = p.ProductCategories.Select(x => x.CategoryID).ToList()
                }).ToListAsync();
            return _list;
        }

        public async Task<IEnumerable<ProductListItemDTO>> SearchUnAvailableProductAsync()
        {
            var _list = await _repositoryProduct.TableNoTracking.Where(p => !p.Deleted && p.StockQuantity <= 0)
                .Select(p => new ProductListItemDTO
                {
                    CreateOn = p.CreateOn,
                    UpdateOn = p.UpdateOn,
                    LocalPublishDate = p.PublishDate.ToPersian(),
                    PublishDate = p.PublishDate,
                    ID = p.ID,
                    ProductName = p.ProductName,
                    Price = p.Price,
                    Sku = p.Sku,
                    StockQuantity = p.StockQuantity,
                    LocalCreateOn = p.CreateOn.ToPersian(),
                    LocalUpdateOn = p.UpdateOn.ToPersian(),
                    CategoryNames = p.ProductCategories.Select(x => x.Category.Name).ToList(),
                    CategoryIds = p.ProductCategories.Select(x => x.CategoryID).ToList()
                }).ToListAsync();
            return _list;
        }

        public async Task<ProductListItemDTO> SearchProductByIDAsync(int id)
        {
            var product = await _repositoryProduct.GetByIDAsync(id);
            return product.TODTO<ProductListItemDTO>();
        }
        public async Task<ProductDTO> RegisterProductAsync(ProductDTO productDTO)
        {
            var product = productDTO.ToEntity<Product>();
            await _repositoryProduct.InsertAsync(product);
            productDTO.ID = product.ID;
            return productDTO;
        }
        public async Task<bool> IsExistProductAsync(int id)
        {
            var product = await _repositoryProduct.GetByIDNoTrackingAsync(id);
            if (product == null)
                return false;
            return true;
        }
        public async Task RemoveProductAsync(int id)
        {
            var product = _repositoryProduct.GetByID(id);
            product.Deleted = true;
            await _repositoryProduct.UpdateAsync(product);

        }
        public async Task UpdateProductAsync(ProductDTO productDTO)
        {
            //var category = categoryDTO.ToEntity<Category>();
            var product = _repositoryProduct.GetByID(productDTO.ID);
            product.ID = productDTO.ID;
            product.ProductName = productDTO.ProductName;
            product.Price = productDTO.Price;
            product.Sku = productDTO.Sku;
            product.PublishDate = productDTO.PublishDate;
            product.StockQuantity = product.StockQuantity;
            await _repositoryProduct.UpdateAsync(product);
        }
        public async Task UpdateProductStockQuantityDTOAsync(ProductStockQuantityDTO productStockQuantityDTO)
        {
            //var category = categoryDTO.ToEntity<Category>();
            var product = _repositoryProduct.GetByID(productStockQuantityDTO.ID);
            product.StockQuantity = product.StockQuantity;
            await _repositoryProduct.UpdateAsync(product);
        }
        public async Task AddProductToCategoryAsync(ProductCategoryDTO productCategoryDTO)
        {
            var productCategory = productCategoryDTO.ToEntity<ProductCategory>();
            await _repositoryProductCategory.InsertAsync(productCategory);
        }
        public async Task RemoveProductToCategoryAsync(ProductCategoryDTO productCategoryDTO)
        {
            var productCategory = productCategoryDTO.ToEntity<ProductCategory>();
            await _repositoryProductCategory.DeleteAsync(productCategory);
        }
        public async Task AddPictureForProduct(ProductPictureDTO productPictureDTO)
        {
            var productpicture = productPictureDTO.ToEntity<ProductPicture>();
            await _repositoryProductPicture.InsertAsync(productpicture);

        }
        public async Task RemovePictureofProduct(ProductPictureDTO productPictureDTO)
        {
            var productpicture = productPictureDTO.ToEntity<ProductPicture>();
            await _repositoryProductPicture.DeleteAsync(productpicture);

        }
        public async Task<List<int>> GetPictureofProduct(int productID)
        {
            return await _repositoryProductPicture.TableNoTracking.Where(p => p.ProductID == productID).Select(q => q.PictureID).ToListAsync();

        }
        public string GetProductName(int ProdId)
        {
            string name;
            if (ProdId == 1)
            {
                name = "Mobile";
            }
            else if (ProdId == 2)
            {
                name = "Shirt";
            }
            else
            {
                name = "Not Found";
            }
            return name;
        }
        public int GetPriceByDiscount(int ProductID)
        {

            var prod = _repositoryProduct.GetByID(ProductID);

            var cats = prod.ProductCategories.Where(p => p.Category.DiscountAmount != 0).ToList();
            if (cats == null || cats.Count == 0)
                return prod.Price;

            var dis = cats.Max(p => p.Category.DiscountAmount);
            return (prod.Price - ((prod.Price * dis) / 100));
        }
    }
}
