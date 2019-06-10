using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LidLaunchWebsite.Models
{
    public class PaypalItem
    {
        public string name { get; set; }
        public string quantity { get; set; }
        public decimal price { get; set; }
        public string currency { get; set; }
    }
}