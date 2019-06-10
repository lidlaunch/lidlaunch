using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LidLaunchWebsite.Models
{
    public class HatColor
    {
        public int colorId { get; set; }
        public int typeId { get; set; }
        public string color { get; set; }
        public bool availableToCreate { get; set; }
        public string colorCode { get; set; }
        public string  creationImage { get; set; }
    }
}