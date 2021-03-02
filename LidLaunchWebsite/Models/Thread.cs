using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LidLaunchWebsite.Models
{
    public class Thread
    {
        public int Id { get; set; }
        public int ColorCode { get; set; }
        public string RGBValue { get; set; }
        public string PreviewImage { get; set; }        
    }
}