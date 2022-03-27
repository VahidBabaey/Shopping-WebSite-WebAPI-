using System;
using System.Collections.Generic;
using System.Text;

namespace Reddington.Core.Domian
{
    public class ProductPicture:BaseRelation
    {
        public int ProductID { get; set; }
        public int PictureID { get; set; }
        public virtual Product Product { get; set; }
        public virtual Picture Picture { get; set; }
        public int DisplayOrder { get; set; }
    }
}
