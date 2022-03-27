using System;
using System.Collections.Generic;
using System.Text;

namespace Reddington.Core.Domian
{
    public class ProductCategory:BaseRelation
    {
      
        public int ProductID { get; set; }
        public int CategoryID { get; set; }
        public virtual Category Category { get; set; }
        public virtual Product Product { get; set; }
    }
}
