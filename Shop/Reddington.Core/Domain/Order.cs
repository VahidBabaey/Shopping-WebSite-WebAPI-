using System;
using System.Collections.Generic;
using System.Text;

namespace Reddington.Core.Domian
{
    public class Order : BaseEntity
    {
      
        public int OrderTotal { get; set; }
        public int OrderStatusId { get; set; }
        public OrderStatus OrderStatus
        {
            get => (OrderStatus)OrderStatusId;
            set => OrderStatusId = (int)value;
        }
        public bool Deleted { get; set; }
        public int CustomerID { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}
