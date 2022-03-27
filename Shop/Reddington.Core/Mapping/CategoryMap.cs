using Reddington.Core.Domian;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Reddington.Data.Mapping
{
    public class CategoryMap : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasMany(p => p.Children).WithOne(p => p.ParentCategory).HasForeignKey(p => p.ParentID)
                .OnDelete(DeleteBehavior.NoAction);
            builder.HasData(new Category
            {
                ID = 1,
                ParentID = 1,
                Name = "دسته",
                CreateOn = DateTime.Now,
                UpdateOn = DateTime.Now
            });
        }
    }
}
