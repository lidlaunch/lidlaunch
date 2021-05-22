using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LidLaunchWebsite.Models
{
    public class CostEsimate
    {
        public int TotalHats { get; set; }
        public decimal TotalHatsCost { get; set; }
        public decimal TotalEstimatedSuppliesCost { get; set; }
        public int Total8x8x6Boxes { get; set; }
        public int Total10x8x6Boxes { get; set; }
        public int Total12x8x6Boxes { get; set; }
        public int Total16x8x6Boxes { get; set; }
        public int Total24x8x6Boxes { get; set; }
        public int TotalMiscBoxes { get; set; }
        public decimal TotalEstimatedBoxesCost { get; set; }
        public decimal TotalEstimatedDigitizingCost { get; set; }
        public decimal TotalShippingEstimatedCost { get; set; }        
        public int TotalOrders { get; set; }
        public decimal TotalOrderRevenue { get; set; }
        public decimal TotalShippingRevenueReceived { get; set; }
        public decimal GrandEstimatedTotal { get; set; }
        public decimal PayPalTranscationFeesTotal { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
    }
}