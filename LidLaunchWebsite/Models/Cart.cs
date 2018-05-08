using System;
using System.Collections.Generic;

namespace LidLaunchWebsite.Models
{
    public class Cart 
    {
        public int Id { get; set; }
        public List<Product> lstProducts { get; set; }
        public decimal Total { get; set; }
    }
}