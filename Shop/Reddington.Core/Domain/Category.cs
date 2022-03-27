using System;
using System.Collections.Generic;
using System.Text;

namespace Reddington.Core.Domian
{
    public class Category :BaseEntity
    {
        public string Name { get; set; }
        public int ParentID { get; set; }
        public virtual Category ParentCategory { get; set; }
        public int DiscountAmount { get; set; }
        public virtual ICollection<Category> Children { get; set; }
        public virtual ICollection<ProductCategory> ProductCategories { get; set; }
    }
}
