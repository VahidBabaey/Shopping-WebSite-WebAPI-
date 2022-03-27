using Microsoft.EntityFrameworkCore;
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

    public class CategoryService:ICategoryService
    {
        #region Fields
        private readonly IRepository<Category> _repositoryCategory = null;
        #endregion
        public CategoryService(IRepository<Category> repositoryCategory)
        {
            _repositoryCategory = repositoryCategory;
        }
        public async Task<IEnumerable<CategoryListItemDTO>> SearchCategoryAsync()
        {
            var _list = await _repositoryCategory.TableNoTracking.Select(p => new CategoryListItemDTO
            {
                ID = p.ID,
                Name = p.Name,
                ParentID = p.ParentID,
                ParentName = p.ParentCategory.Name,
                ChildCount = p.Children.Count,
                CreateOn = p.CreateOn,
                UpdateOn = p.UpdateOn,
                LocalCreateOn = p.CreateOn.ToPersian(),
                LocalUpdateOn = p.UpdateOn.ToPersian()

            }).ToListAsync();
            return _list;
        }
        public async Task<CategoryListItemDTO> SearchCategoryByIDAsync(int id)
        {
            var category = await _repositoryCategory.GetByIDAsync(id);
            return category.TODTO<CategoryListItemDTO>();
        }
        public async Task<bool> IsExistCategoryAsync(int id)
        {
            var category = await _repositoryCategory.GetByIDNoTrackingAsync(id);
            if (category == null)
                return false;
            return true;
        }
        public async Task<CategoryDTO> RegisterCategoryAsync(CategoryDTO categoryDTO)
        {
            var category = categoryDTO.ToEntity<Category>();
            await _repositoryCategory.InsertAsync(category);
            categoryDTO.ID = category.ID;
            return categoryDTO;
        }
        public async Task RemoveCategoryAsync(int id)
        {
            var category = _repositoryCategory.GetByID(id);
            await _repositoryCategory.DeleteAsync(category);

        }
        public async Task UpdateCategoryAsync(CategoryDTO categoryDTO)
        {
            //var category = categoryDTO.ToEntity<Category>();
            var category = _repositoryCategory.GetByID(categoryDTO.ID);
            category.ID = categoryDTO.ID;
            category.Name = categoryDTO.Name;
            category.ParentID = categoryDTO.ParentID;
            await _repositoryCategory.UpdateAsync(category);
        }
    }
}
