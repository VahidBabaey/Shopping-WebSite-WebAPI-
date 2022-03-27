using System;
using System.Collections.Generic;
using System.Text;

namespace Reddington.Core.Domian
{
    public class ShoppingCartItem:BaseRelation
    {
        public int CustomerID { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }
      

        public virtual Customer Customer { get; set; }
        public virtual Product Product { get; set; }
    }
}
