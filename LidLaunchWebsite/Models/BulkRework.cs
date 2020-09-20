using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LidLaunchWebsite.Models
{
    public class BulkRework
    {
        public int Id { get; set; }
        public int BulkOrderItemId { get; set; }
        public int BulkOrderBatchId { get; set; }        
        public int Quantity { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Note { get; set; }
        public bool IsMissingBlank { get; set; }
        public string MissingBlankName { get; set; }
    }
}