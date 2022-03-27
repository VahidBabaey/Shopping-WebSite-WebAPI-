using Reddington.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Reddington.Services.Catalog
{
    public interface IProductService
    {
        Task AddPictureForProduct(ProductPictureDTO productPictureDTO);
        Task AddProductToCategoryAsync(ProductCategoryDTO productCategoryDTO);
        Task<List<int>> GetPictureofProduct(int productID);
        int GetPriceByDiscount(int ProductID);
        string GetProductName(int ProdId);
        Task<bool> IsExistProductAsync(int id);
        Task<ProductDTO> RegisterProductAsync(ProductDTO productDTO);
        Task RemovePictureofProduct(ProductPictureDTO productPictureDTO);
        Task RemoveProductAsync(int id);
        Task RemoveProductToCategoryAsync(ProductCategoryDTO productCategoryDTO);
        Task<IEnumerable<ProductListItemDTO>> SearchAllProductsAsync();
        Task<ProductListItemDTO> SearchProductByIDAsync(int id);
        Task<IEnumerable<ProductListItemDTO>> SearchProductsAsync(ProductFilterDTO productFilterDTO);
        Task<IEnumerable<ProductListItemDTO>> SearchUnAvailableProductAsync();
        Task UpdateProductAsync(ProductDTO productDTO);
        Task UpdateProductStockQuantityDTOAsync(ProductStockQuantityDTO productStockQuantityDTO);
    }
}
