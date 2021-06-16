using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LidLaunchWebsite.Models
{
    public class Expense
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public decimal Amount { get; set; }
        public bool Deleted { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Attachment { get; set; }

        public const string Payroll = "Payroll";
        public const string Advertising = "Advertising";
        public const string Rent = "Rent";
        public const string Utilities = "Utilities";
        public const string Insurance = "Insurance";
        public const string Blanks = "Blanks";
        public const string Benefits = "Benefits";
        public const string Shipping = "Shipping";
        public const string ShippingSupplies = "ShippingSupplies";        
        public const string ProductionSupplies = "ProductionSupplies";
        public const string OfficeSupplies = "OfficeSupplies";
        public const string Services = "Services";
        public const string Digitizing = "Digitizing";
        public const string Legal = "Legal";
        public const string EquipmentInstallment = "EquipmentInstallment";
        public const string EquipmentOneTime = "EquipmentOneTime";
        
        
        public const string Maintenance = "Maintenance";
        public const string Technology = "Technology";
        public const string Travel = "Travel";
        public const string Meals = "Meals";
        public const string Misc = "Misc";

    }
}