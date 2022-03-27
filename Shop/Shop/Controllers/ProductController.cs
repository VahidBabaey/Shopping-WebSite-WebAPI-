using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reddington.Core.Domian;
using Reddington.Core.Extensions;
using Reddington.Data;
using Reddington.Framework;
using Reddington.Service.Media;
using Reddington.Services.Catalog;
using Reddington.Services.DTOs;
using Reddington.Services.Extentions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Controllers
{
    public class ProductController : ReddingtonController
    {
        #region Fields
        private readonly IProductService _productServices = null;
        private readonly ICategoryService _categoryServices = null;
        private readonly IPictureService _pictureServices = null;

        public ProductController(IProductService productServices, ICategoryService categoryServices, IPictureService pictureServices)
        {
            _productServices = productServices;
            _categoryServices = categoryServices;
            _pictureServices = pictureServices;
        }




        #endregion

        #region ProductSearch
        public async Task<IActionResult> GetListAsync()
        {

            return Ok(await _productServices.SearchAllProductsAsync());
        }
        [HttpGet("find/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Find(int id)
        {
            var productDTO = await _productServices.SearchProductByIDAsync(id);
            if(productDTO == null)
            {
                return NotFound();
            }
            

            return Ok(productDTO);
        }
        [HttpGet("Search")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status201Created)]

        public async Task<IActionResult> SearchProduct([FromQuery] ProductFilterDTO productFilterDTO)
        {

            return Ok(await _productServices.SearchProductsAsync(productFilterDTO));
        }
        #endregion
        #region Product
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> RegisterAsync([FromForm] ProductDTO productDTO)
        {
            if (productDTO.ID != 0)
                return BadRequest();
            await _productServices.RegisterProductAsync(productDTO);
            return CreatedAtAction("find", new { id = productDTO.ID }, productDTO);
        }
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdateAsync([FromForm] ProductDTO productDTO)
        {
            if (!await _productServices.IsExistProductAsync(productDTO.ID))
                return NotFound();
            await _productServices.UpdateProductAsync(productDTO);
            return NoContent();
        }
        [HttpDelete("id")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> RemoveAsync(int id)
        {
            
            if (!await _productServices.IsExistProductAsync(id))
                return NotFound();
            await _productServices.RemoveProductAsync(id);
            return Ok();
        }
        #endregion 
        #region UpdateStockQuantity
        [HttpPut("UpdateStockQuantity")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdateStockQuantityAsync([FromQuery] ProductStockQuantityDTO productStockQuantityDTO)
        {
            if (!await _productServices.IsExistProductAsync(productStockQuantityDTO.ID))
                return NotFound();
            await _productServices.UpdateProductStockQuantityDTOAsync(productStockQuantityDTO);
            return NoContent();
        }
        #endregion
        #region ProductCategory
        [HttpPost("RegisterProductToCategory")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> RegisterProductToCategoryAsync([FromForm] ProductCategoryDTO productCategoryDTO)
        {
            if(!await _productServices.IsExistProductAsync(productCategoryDTO.ProductID))
            {
                return NotFound("Product Not Found :" + productCategoryDTO.ProductID);
            }
            if (!await _categoryServices.IsExistCategoryAsync(productCategoryDTO.CategoryID))
            {
                return NotFound("Category Not Found :" + productCategoryDTO.CategoryID);
            }
             await _productServices.AddProductToCategoryAsync(productCategoryDTO);
            return Ok();
        }
        [HttpPost("DeleteProductToCategory")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteProductToCategoryAsync(ProductCategoryDTO productCategoryDTO)
        {
            if (!await _productServices.IsExistProductAsync(productCategoryDTO.ProductID))
            {
                return NotFound("Product Not Found :" + productCategoryDTO.ProductID);
            }
            if (!await _categoryServices.IsExistCategoryAsync(productCategoryDTO.CategoryID))
            {
                return NotFound("Category Not Found :" + productCategoryDTO.CategoryID);
            }
            await _productServices.RemoveProductToCategoryAsync(productCategoryDTO);
            return Ok();
        }
        #endregion

        #region ProductPicture
        [HttpPost("RegisterPictureForProduct")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> RegisterPictureToProductAsync([FromForm] ProductPictureDTO productPictureDTO)
        {
            if (!await _productServices.IsExistProductAsync(productPictureDTO.ProductID))
            {
                return NotFound("Product Not Found :" + productPictureDTO.ProductID);
            }
            if (!await _pictureServices.CheckExists(productPictureDTO.PictureID))
            {
                return NotFound("Picture Not Found :" + productPictureDTO.PictureID);
            }
            await _productServices.AddPictureForProduct(productPictureDTO);
            return Ok();
        }
        [HttpDelete("DeletePictureofProduct")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeletePictureofProductAsync([FromForm] ProductPictureDTO productPictureDTO)
        {
            if (!await _productServices.IsExistProductAsync(productPictureDTO.ProductID))
            {
                return NotFound("Product Not Found :" + productPictureDTO.ProductID);
            }
            if (!await _pictureServices.CheckExists(productPictureDTO.PictureID))
            {
                return NotFound("Picture Not Found :" + productPictureDTO.PictureID);
            }
            await _productServices.RemovePictureofProduct(productPictureDTO);
            return Ok();
        }
        [HttpGet("SearchPictureofProduct")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> SearchPictureofProductAsync([FromForm] int productID)
        {
            if (!await _productServices.IsExistProductAsync(productID))
            {
                return NotFound("Product Not Found :" + productID);
            }
            await _productServices.GetPictureofProduct(productID);
            return Ok();
        }

        #endregion
    }
}
