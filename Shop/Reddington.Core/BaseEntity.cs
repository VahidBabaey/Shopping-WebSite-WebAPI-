using Reddington.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Reddington.Core
{
    public  class BaseEntity:Entity,IDateEntity
    {
        public int ID { get; set; }
        public DateTime CreateOn { get; set; }
        public DateTime UpdateOn { get; set; }
    }
}
