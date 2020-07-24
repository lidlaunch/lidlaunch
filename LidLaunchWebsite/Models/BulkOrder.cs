using System.Collections.Generic;
using System;

namespace LidLaunchWebsite.Models
{
    public class BulkOrder
    {
        public int Id { get; set; }
        public List<BulkOrderItem> lstItems { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }
        public decimal OrderTotal { get; set; }
        public bool HatsOrdered { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime ShipDate { get; set; }
        public string TrackingNumber { get; set; }
        public bool OrderComplete { get; set; }
        public string OrderNotes { get; set; }
        public string ArtworkImage { get; set; }
        public string HatsOrderedSource { get; set; }
        public string HatsOrderedTracking { get; set; }
        public bool OrderCanceled { get; set; }
        public string ArtworkPosition { get; set; }
        public bool OrderPaid { get; set; }
        public string PaymentGuid { get; set; }
        public string PaymentCompleteGuid { get; set; }
        public List<Design> lstDesigns { get; set; }
        public List<Note> lstNotes{ get; set; }
}
}