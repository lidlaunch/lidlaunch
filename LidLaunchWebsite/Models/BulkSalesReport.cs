using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LidLaunchWebsite.Models
{
    public class BulkSalesReport
    {
        public List<BulkSale> BulkSales { get; set; }
    }
    public class BulkSale
    { 
        public DateTime Date { get; set; }
        public int OrderCount { get; set; }
        public int OrderTotals { get; set; }
    }

}