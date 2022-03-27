using System;
using System.Collections.Generic;
using System.Text;

namespace Reddington.Core.Domian
{
    public class OrderItem : BaseRelation,IDateEntity
    {
        public int ProductID { get; set; }
        public int OrderID { get; set; }
        public int UnitPrice { get; set; }
        public int Quantity { get; set; }
        public virtual Product Product { get; set; }
        public virtual Order Order { get; set; }
        public DateTime CreateOn { get; set; }
        public DateTime UpdateOn { get; set; }
    }
}
