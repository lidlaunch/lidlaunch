using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LidLaunchWebsite.Models
{
    public class DigitizingOrder
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public decimal Width { get; set; }
        public decimal Height { get; set; }
        public string Notes { get; set; }
        public int DesignId { get; set; }
        public int StitchCount { get; set; }
        public decimal Total { get; set; }
        public bool Completed { get; set; }
        public bool Rework { get; set; }
        public bool Approved { get; set; }
        public string AlterationsNote { get; set; }
        public Design design { get; set; }
        public bool HasPaid { get; set; }
        public string PaymentGuid { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}