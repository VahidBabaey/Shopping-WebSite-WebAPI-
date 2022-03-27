using System;
using System.Collections.Generic;
using System.Text;

namespace Reddington.Core.Domian
{
    public class Customer : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName {
            get => FirstName + " " + LastName;
        }
        public string AdminComment { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public byte[] Timestamp { get; set; }
        public bool Deleted { get; set; }
        public virtual ICollection<ShoppingCartItem> ShoppingCartItems { get; set; }

        public virtual ICollection<Order> Orders { get; set; }

    }
}
