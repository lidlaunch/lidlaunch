using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LidLaunchWebsite.Models
{
    public class Dashboard
    {

    }
    public class DashboardProduct
    { 
        public int OrderID { get; set; }
        public int UserId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }
        public string ShippingAddress { get; set; }    


    }
    public class DesignerDashboard {
        public List<DesignerDashboardSales> lstSales { get; set; }
        public List<DesignerDashboardProduct> lstProducts { get; set; }
        public decimal Profit { get; set; }
        public int TotalProductsSold { get; set; }
        public decimal TotalAvailableForPayout { get; set; }
    }
    public class DesignerDashboardSales {
        public int TotalSales { get; set; }
        public string ProductName { get; set; }
        public string ProductImage { get; set; }
        public int SaleProfitNumber { get; set; }
    }
    public class DesignerDashboardProduct
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductImage { get; set; }
        public bool Private { get; set; }
        public bool Approved { get; set; }
    }
    public class AdminDashboard {
        public List<Sale> lstSales { get; set; }
        public decimal TotalSales { get; set; }
        public decimal TotalSalesWithoutShipping { get; set; }
        public int TotalSalesCount { get; set; }
        public decimal TotalDesignerProfits { get; set; }
        public decimal EstimatedShippingCosts { get; set; }
        public decimal TotalShippingCollected { get; set; }
        public decimal TotalShippingActual { get; set; }
        public decimal EstimatedTotalProfit { get; set; }
        public decimal EstimatedExcessShipping { get; set; }
        public decimal DigitizingTotal { get; set; }
        public decimal DigitizingExcess { get; set; }
    }
    public class BatchDashboard
    {
        public List<OrderBatch> lstBatches { get; set; }
        public List<BatchSale> lstReadySales { get; set; }
        public List<BatchSale> lstNotReadySales { get; set; }
        public List<OrderBatch> lstOrderBatches { get; set; }
    }
    public class OrderBatch
    {
        public int BatchId { get; set; }
        public DateTime DateBatched { get; set; }
        public string Status { get; set; }
    }
    public class BatchSale {
        public List<Sale> lstSales { get; set; }
        public string ProductName { get; set; }
        public string ArtSource { get; set; }
        public string PreviewImage { get; set; }
        public string DigitizedFile { get; set; }
        public string DigitizedProductionSheet { get; set; }
        public string DigitizedPreview { get; set; }
        public int ProductId { get; set; }
        public List<HatTypeSize> lstHatTypeSizes { get; set; }
    }
    public class HatTypeSize {
        public string Size { get; set; }
        public string Type { get; set; }
        public string Color { get; set; }
        public int Count { get; set; }
    }
    public class Sale {
        public int OrderId { get; set; }
        public int OrderProductId { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerEmail { get; set; }
        public string ShippingAddress { get; set; }
        public decimal OrderTotal { get; set; }
        public decimal ShippingPaid { get; set; }
        public decimal TaxesPaid { get; set; }
        public decimal ShippingActual { get; set; }
        public decimal DigitizingCost { get; set; }
        public string OrderStaus { get; set; }
        public DateTime OrderDate { get; set; }
        public bool HasPaid { get; set; }
        public string ProductName { get; set; }
        public int DesignId { get; set; }
        public int DesignerId { get; set; }
        public string Size { get; set; }
        public bool Produced { get; set; }
        public bool Shipped { get; set; }
        public string TrackingNumber { get; set; }
        public string ArtSource { get; set; }
        public string PreviewImage { get; set; }
        public string DigitizedFile { get; set; }
        public string DigitizedProductionSheet { get; set; }
        public string DigitizedPreview { get; set; }
        public decimal EmbroiderdWidth { get; set; }
        public decimal EmbroideredHeight { get; set; }
        public decimal EmbroideredX { get; set; }
        public decimal EmbroideredY { get; set; }
        public string TypeText { get; set; }
        public string ProductIdentifier { get; set; }
        public string HatColor { get; set; }
    }
    public class HatManager
    {
        public List<HatType> lstHatTypes { get; set; }
    }        

    public class BulkOrderListSectionModel
    {
        public List<BulkOrder> lstBulkOrders { get; set; }
        public string Name { get; set; }
        public int SortOrder { get; set; }
        public string textColor { get; set; }
        public string backgroundColor { get; set; }
    }

    public class ViewBulkOrdersModel
    {
        public List<BulkOrderListSectionModel>  lstSections { get; set; }
        public List<BulkOrder> lstBulkOrders { get; set; }
        public List<OrderBatch> lstBulkOrderBatches { get; set; }
    }
}