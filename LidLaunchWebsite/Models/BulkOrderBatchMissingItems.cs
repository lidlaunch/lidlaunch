using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LidLaunchWebsite.Models
{
    public class BulkOrderBatchMissingItems
    {
        public int Id { get; set; }
        public int BulkOrderBatchId { get; set; }
        public int MasterBulkOrderItemId { get; set; }        
        public int MissingQuantity { get; set; }
        public string OrderedFromSource { get; set; }
        public bool Ordered { get; set; }
        public bool OutOfStock { get; set; }
        public string TrackingNumber { get; set; }
        public string ItemName { get; set; }
    }
}