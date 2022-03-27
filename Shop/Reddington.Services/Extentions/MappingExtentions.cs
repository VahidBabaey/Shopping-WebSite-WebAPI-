using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mapster;
using Reddington.Core;
using Reddington.Core.Domian;
using Reddington.Core.Extensions;
using Reddington.Services.DTOs;

namespace Reddington.Services.Extentions
{
    public static class MappingExtentions
    {
        public static TDTO TODTO<TDTO>(this Entity entity) where TDTO:BaseDTO
        {
            if(entity == null)
            {
                return null;
            }
            if(typeof(TDTO).GetInterface("IDateDTO")!=null && entity.GetType().GetInterface("IDateEntity")!=null)
            {
                TypeAdapterConfig<IDateEntity, TDTO>.NewConfig().Map("LocalCreateOn", p => p.CreateOn.ToPersian())
                    .Map("LocalUpdateOn", p => p.UpdateOn.ToPersian());
            }
            var dto = entity.Adapt<TDTO>();
            if(entity is Category && dto is CategoryDTO)
            {
                var category = entity as Category;
                var categoryDTO = dto as CategoryDTO;
                if (category.ParentCategory != null)
                    categoryDTO.ParentName = category.ParentCategory?.Name;
            }
            if (entity is Category && dto is CategoryListItemDTO)
            {
                var category = entity as Category;
                var categoryDTO = dto as CategoryListItemDTO;
                if (category.ParentCategory != null)
                    categoryDTO.ParentName = category.ParentCategory?.Name;
                if (category.ProductCategories != null)
                    categoryDTO.ProductCount = category.ProductCategories.Count;
                if (category.Children != null)
                    categoryDTO.ChildCount = category.Children.Count;
            }
            if (entity is Product && dto is ProductListItemDTO)
            {
                var product = entity as Product;
                var productdto = dto as ProductListItemDTO;
                productdto.LocalPublishDate = product.PublishDate.ToPersian();
                if(product.ProductCategories!=null)
                {
                    productdto.CategoryNames = product.ProductCategories.Select(p => p.Category.Name).ToList();
                    productdto.CategoryIds = product.ProductCategories.Select(p => p.CategoryID).ToList();
                }

            }
            return dto;
        }
        public static TEntity ToEntity<TEntity>(this BaseDTO baseDTO) where TEntity:Entity
        {
            if(typeof(TEntity).GetInterface("IDateEntity")!=null)
            {
                TypeAdapterConfig<BaseDTO, TEntity>.NewConfig().Ignore("CreateOn", "UpdateOn", "LocalCreateOn", "LocalUpdateOn");
            }
            var entity = baseDTO.Adapt<TEntity>();
            //if(entity is Product && baseDTO is ProductDTO)
            //{
            //    var product = entity as Product;
            //    var productdto = baseDTO as ProductDTO;
            //    product.PublishDate = productdto.PublishDate.PersianToDateTime();

            //}
            return entity;
        }
    }
}
