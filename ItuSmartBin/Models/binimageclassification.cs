using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ItuSmartBin.Models
{
    public class binimageclassification
    {
        public string Tagname { get; set; }

        public string percentage { get; set; }

        public string results { get; set; }

        public byte[] Image { get; set; }
    }
}