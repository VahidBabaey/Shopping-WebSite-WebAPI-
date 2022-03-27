using System;
using System.Collections.Generic;
using System.Text;

namespace Reddington.Core.Domian
{
    public class Picture : BaseEntity
    {
        public string VirtualPath { get; set; }
        public string MimeType { get; set; }
    }
}
