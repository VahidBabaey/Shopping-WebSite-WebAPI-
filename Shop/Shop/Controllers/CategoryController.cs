using Castle.Core.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Reddington.Core.Domian;
using Reddington.Core.Extensions;
using Reddington.Core.Tasks;
using Reddington.Data;
using Reddington.Framework;
using Reddington.Framework.Infrastructure.Filters;
using Reddington.Services.Catalog;
using Reddington.Services.DTOs;
using Reddington.Services.Extentions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Controllers
{
    [Authorize]
    public class CategoryController : ReddingtonController
    {
        #region Fields
        private readonly ICategoryService _categoryServices = null;
        private readonly ILogger<CategoryController> _logger = null;
        private readonly StockQuantityTask _stockQuantityTask = null;

        public CategoryController(ICategoryService categoryServices, ILogger<CategoryController> logger, StockQuantityTask stockQuantityTask)
        {
            _categoryServices = categoryServices;
            _logger = logger;
            _stockQuantityTask = stockQuantityTask;
        }

        //private readonly IMemoryCache _memoryCache = null;
        //private readonly IDistributedCache _distributedCache = null;






        #endregion
        /// <summary>
        /// این تایع یک  دسته جدید را ثبت می کند
        /// </summary>

        /// <returns></returns>

        [ServiceFilter(typeof(LogFilterAttribute))]
        [AddHeader("Author" , "Raymond Reddington")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetListAsync()
        {
           // _stockQuantityTask.ExecuteTask();
            //object _list = null;
            //_list = _memoryCache.Get("Category");
            //if(_list == null)
            //{
            //    await _categoryServices.SearchCategoryAsync();
            //    var cacheEntryOptions = new MemoryCacheEntryOptions();

            //    cacheEntryOptions.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(20);

            //    _memoryCache.Set("Category", _list, cacheEntryOptions);
            //}
            //var _listString = await _distributedCache.GetStringAsync("Category");
            //if(string.IsNullOrEmpty(_listString))
            //{
            //    var _listData = await _categoryServices.SearchCategoryAsync();
            //    if(_listData.Count() != 0)
            //    {
            //        _listString = JsonConvert.SerializeObject(_listData);
            //        var options = new DistributedCacheEntryOptions()
            //         .SetSlidingExpiration(TimeSpan.FromSeconds(60));
            //        await _distributedCache.SetStringAsync("Category", _listString, options);

            //    }
            //    _list = _listString;
            //}
            //else
            //{
            //    _list = JsonConvert.DeserializeObject<IEnumerable<CategoryListItemDTO>>(_listString);
            //}
            var listcategory = await _categoryServices.SearchCategoryAsync();
            return Ok(listcategory);
        }
        [HttpGet("find/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Find(int id)
        {
            var categoryDTO = await _categoryServices.SearchCategoryByIDAsync(id);
            if(categoryDTO == null)
            {
                return NotFound();
            }
            

            return Ok(categoryDTO);
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> RegisterAsync([FromForm] CategoryDTO categoryDTO)
        {
            if (categoryDTO.ID != 0)
                return BadRequest();
            await _categoryServices.RegisterCategoryAsync(categoryDTO);
            return CreatedAtAction("find", new { id = categoryDTO.ID }, categoryDTO);
        }
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdateAsync([FromForm] CategoryDTO categoryDTO)
        {
            if (await _categoryServices.IsExistCategoryAsync(categoryDTO.ID))
                return NotFound();
            await _categoryServices.UpdateCategoryAsync(categoryDTO);
            return NoContent();
        }
        [HttpDelete("id")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> RemoveAsync(int id)
        {
            
            if (!await _categoryServices.IsExistCategoryAsync(id))
                return NotFound();
            await _categoryServices.RemoveCategoryAsync(id);
            return Ok();
        }

    }
}
