using Reddington.Services.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Reddington.Services.Catalog
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryListItemDTO>> SearchCategoryAsync();
        Task<CategoryListItemDTO> SearchCategoryByIDAsync(int id);
        Task<CategoryDTO> RegisterCategoryAsync(CategoryDTO categoryDTO);
        Task RemoveCategoryAsync(int id);
        Task<bool> IsExistCategoryAsync(int id);
        Task UpdateCategoryAsync(CategoryDTO categoryDTO);
    }
}