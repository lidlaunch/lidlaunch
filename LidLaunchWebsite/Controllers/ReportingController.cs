using LidLaunchWebsite.Classes;
using LidLaunchWebsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LidLaunchWebsite.Controllers
{
    public class ReportingController : Controller
    {
        // GET: Reporting
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetExpensesByDateRange(string dateFrom, string dateTo)
        {
            CostEsimate costEsimate = GetCostEstimate(dateFrom, dateTo);            

            return View(costEsimate);

        }

        private CostEsimate GetCostEstimate(string dateFrom, string dateTo)
        {
            CostEsimate costEsimate = new CostEsimate();
            if (dateFrom != null && dateTo != null && dateFrom != "" && dateTo != "" && checkAdminLoggedIn())
            {
                BulkData bulkData = new BulkData();
                List<BulkOrder> lstBulkOrders = bulkData.GetBulkOrderData("");
                List<MasterBulkOrderItem> lstMasterItems = bulkData.GetMasterBulkOrderItems(false);

                lstBulkOrders = lstBulkOrders.Where(b => b.PaymentDate >= Convert.ToDateTime(dateFrom) && b.PaymentDate <= Convert.ToDateTime(dateTo) && b.OrderPaid).ToList();

                var totalHatCost = 0.00M;
                var totalBoxCost = 0.00M;
                var total8x8x6Boxes = 0;
                var total10x8x6Boxes = 0;
                var total12x8x6Boxes = 0;
                var total16x8x6Boxes = 0;
                var total24x8x6Boxes = 0;
                var totalMiscBoxes = 0;
                //current average shipping cost is $8.83
                var estimatedShippingCost = lstBulkOrders.Count * 8.83M;
                var estimatedDigitizingCost = lstBulkOrders.Where(b => !b.OrderNotes.Contains("ARTWORK PRE-EXISTING")).Count() * 12;
                var estimatedSuppliesCost = 0.00M;
                var entireTotalHats = 0;
                var totalRevenue = 0.00M;
                var totalShippingRevenue = 0.00M;
                var paypalFees = 0.00M;

                foreach (BulkOrder bulkOrder in lstBulkOrders)
                {
                    totalRevenue += bulkOrder.OrderTotal;
                    var totalHats = 0;
                    foreach (BulkOrderItem item in bulkOrder.lstItems)
                    {
                        MasterBulkOrderItem masterItem = new MasterBulkOrderItem();
                        if (item.MasterItemId != 0)
                        {
                            masterItem = lstMasterItems.First(i => i.Id == item.MasterItemId);
                        }

                        if (masterItem != null && masterItem.Manufacturer != null && masterItem.Manufacturer != "")
                        {
                            if (masterItem.Manufacturer.ToUpper() != "LIDLAUNCH")
                            {
                                totalHatCost += masterItem.Cost * item.ItemQuantity;
                                totalHats += item.ItemQuantity;
                            }
                        }
                        if (masterItem != null && masterItem.Manufacturer != null && masterItem.Manufacturer != "")
                        {
                            if (masterItem.Manufacturer.ToUpper() == "LIDLAUNCH")
                            {
                                if (item.ItemName.ToUpper() == "SHIPPING")
                                {
                                    totalShippingRevenue += item.ItemCost;
                                }
                            }
                        }
                    }
                    if (totalHats <= 4)
                    {
                        totalBoxCost += Boxes.BOX8x8x6.Cost;
                        total8x8x6Boxes += 1;
                    }
                    if (totalHats > 4 && totalHats <= 6)
                    {
                        totalBoxCost += Boxes.BOX10x8x6.Cost;
                        total10x8x6Boxes += 1;
                    }
                    if (totalHats > 7 && totalHats <= 10)
                    {
                        totalBoxCost += Boxes.BOX12x8x6.Cost;
                        total12x8x6Boxes += 1;
                    }
                    if (totalHats > 11 && totalHats <= 16)
                    {
                        totalBoxCost += Boxes.BOX16x8x6.Cost;
                        total16x8x6Boxes += 1;
                    }
                    if (totalHats > 17 && totalHats <= 30)
                    {
                        totalBoxCost += Boxes.BOX24x8x6.Cost;
                        total24x8x6Boxes += 1;
                    }
                    if (totalHats > 30 && totalHats <= 60)
                    {
                        totalBoxCost += Boxes.BOX24x8x12.Cost;
                        total24x8x6Boxes += 2;
                    }
                    if (totalHats > 60)
                    {
                        totalBoxCost += 0;
                        totalMiscBoxes += 1;
                    }

                    entireTotalHats += totalHats;

                }

                //needles/backing/thread/tape/label/bag/postcard
                estimatedSuppliesCost = entireTotalHats * 0.25M;
                paypalFees = lstBulkOrders.Count() * 0.33M + (totalRevenue * 0.029M);

                costEsimate.Total8x8x6Boxes = total8x8x6Boxes;
                costEsimate.TotalEstimatedBoxesCost = totalBoxCost;
                costEsimate.Total10x8x6Boxes = total10x8x6Boxes;
                costEsimate.Total12x8x6Boxes = total12x8x6Boxes;
                costEsimate.Total16x8x6Boxes = total16x8x6Boxes;
                costEsimate.Total24x8x6Boxes = total24x8x6Boxes;
                costEsimate.TotalMiscBoxes = totalMiscBoxes;
                costEsimate.TotalEstimatedSuppliesCost = estimatedSuppliesCost;
                costEsimate.TotalShippingEstimatedCost = estimatedShippingCost;
                costEsimate.TotalHatsCost = totalHatCost;
                costEsimate.TotalHats = entireTotalHats;
                costEsimate.TotalEstimatedDigitizingCost = estimatedDigitizingCost;
                costEsimate.GrandEstimatedTotal = totalBoxCost + estimatedSuppliesCost + estimatedShippingCost + totalHatCost + estimatedDigitizingCost + paypalFees;
                costEsimate.TotalOrderRevenue = totalRevenue;
                costEsimate.TotalShippingRevenueReceived = totalShippingRevenue;
                costEsimate.TotalOrders = lstBulkOrders.Count();
                costEsimate.PayPalTranscationFeesTotal = paypalFees;
                costEsimate.DateFrom = dateFrom;
                costEsimate.DateTo = dateTo;
            }

            return costEsimate;
        }
        public ActionResult ProfitLoss(string dateFrom, string dateTo)
        {
            ExpenseData expenseData = new ExpenseData();
            ProfitLoss model = new ProfitLoss();
            if (checkAdminLoggedIn())
            {
                model.CostEstimate = GetCostEstimate(dateFrom, dateTo);
                model.Expenses = expenseData.GetExpenses(Convert.ToDateTime(dateFrom), Convert.ToDateTime(dateTo));
                model.Expenses = model.Expenses.OrderBy(e => e.DateFrom).ToList();
            }
            else
            {
                return RedirectToAction("Login", "User");
            }
            
            return View(model);
        }
        public bool checkAdminLoggedIn()
        {
            if (Convert.ToInt32(Session["UserID"]) == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }



    
}