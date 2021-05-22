using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using LidLaunchWebsite.Classes;
using LidLaunchWebsite.Models;

namespace LidLaunchWebsite.Controllers
{
    public class InventoryController : Controller
    {
        // GET: Inventory
        public ActionResult Index()
        {
            return View();
        }

        public string GetStyles()
        {
            SSActivewear.Request ssactivewear = new SSActivewear.Request();
            ssactivewear.GET_Styles();
            return "success";
        }

        public string GetProducts()
        {
            SSActivewear.Request ssactivewear = new SSActivewear.Request();
            ssactivewear.GET_Products();
            return "success";
        }
        public ActionResult View()
        {
            if (Convert.ToInt32(Session["UserID"]) > 0)
            {
                if (CheckLoggedIn())
                {
                    BulkData data = new BulkData();
                    BulkOrderHatSelectModel hatSelectModel = new BulkOrderHatSelectModel();
                    List<MasterBulkOrderItem> masterItemList = data.GetMasterBulkOrderItems(false);

                    hatSelectModel.FlexFit6277Items = masterItemList.Where(i => i.Manufacturer == "FlexFit" && i.ItemStyle == "6277").ToList();
                    hatSelectModel.FlexFit6277Items = hatSelectModel.FlexFit6277Items.OrderBy(i => i.DisplayOrder).ToList();

                    hatSelectModel.FlexFit6511Items = masterItemList.Where(i => i.Manufacturer == "FlexFit" && (i.ItemStyle == "6511 Trucker Fitted" || i.ItemStyle == "6311 Trucker Fitted")).ToList();
                    hatSelectModel.FlexFit6511Items = hatSelectModel.FlexFit6511Items.OrderBy(i => i.DisplayOrder).ToList();

                    hatSelectModel.FlexFit110Items = masterItemList.Where(i => i.Manufacturer == "FlexFit" && i.ItemStyle == "110M Trucker Snapback").ToList();
                    hatSelectModel.FlexFit110Items = hatSelectModel.FlexFit110Items.OrderBy(i => i.DisplayOrder).ToList();

                    hatSelectModel.FlexFit6297Items = masterItemList.Where(i => i.Manufacturer == "FlexFit" && i.ItemStyle == "6297F Flat Bill Fitted").ToList();
                    hatSelectModel.FlexFit6297Items = hatSelectModel.FlexFit6297Items.OrderBy(i => i.DisplayOrder).ToList();

                    hatSelectModel.Yupoong6089Items = masterItemList.Where(i => i.Manufacturer == "Yupoong" && i.ItemStyle == "6089M Flat Bill Snapback").ToList();
                    hatSelectModel.Yupoong6089Items = hatSelectModel.Yupoong6089Items.OrderBy(i => i.DisplayOrder).ToList();

                    hatSelectModel.YupoongDadCapItems = masterItemList.Where(i => i.Manufacturer == "Yupoong" && i.ItemStyle == "6245CM Dad Cap").ToList();
                    hatSelectModel.YupoongDadCapItems = hatSelectModel.YupoongDadCapItems.OrderBy(i => i.DisplayOrder).ToList();

                    hatSelectModel.Yupoong6606Items = masterItemList.Where(i => i.Manufacturer == "Yupoong" && i.ItemStyle == "6606 Trucker Snapback").ToList();
                    hatSelectModel.Yupoong6606Items = hatSelectModel.Yupoong6606Items.OrderBy(i => i.DisplayOrder).ToList();

                    hatSelectModel.Yupoong6006Items = masterItemList.Where(i => i.Manufacturer == "Yupoong" && i.ItemStyle == "6006 Flat Bill Trucker Snapback").ToList();
                    hatSelectModel.Yupoong6006Items = hatSelectModel.Yupoong6006Items.OrderBy(i => i.DisplayOrder).ToList();

                    hatSelectModel.YupoongShortBeanieItems = masterItemList.Where(i => i.Manufacturer == "Yupoong" && i.ItemStyle == "Short Beanie").ToList();
                    hatSelectModel.YupoongShortBeanieItems = hatSelectModel.YupoongShortBeanieItems.OrderBy(i => i.DisplayOrder).ToList();

                    hatSelectModel.YupoongCuffedBeanieItems = masterItemList.Where(i => i.Manufacturer == "Yupoong" && i.ItemStyle == "Cuffed Beanie").ToList();
                    hatSelectModel.YupoongCuffedBeanieItems = hatSelectModel.YupoongCuffedBeanieItems.OrderBy(i => i.DisplayOrder).ToList();

                    hatSelectModel.Richardson112Items = masterItemList.Where(i => i.Manufacturer == "Richardson" && i.ItemStyle == "112 Trucker Snapback").ToList();
                    hatSelectModel.Richardson112Items = hatSelectModel.Richardson112Items.OrderBy(i => i.DisplayOrder).ToList();

                    hatSelectModel.ClassicCapsItems = masterItemList.Where(i => i.Manufacturer == "Classic Caps" && i.ItemStyle == "USA100 Trucker Snapback").ToList();
                    hatSelectModel.ClassicCapsItems = hatSelectModel.ClassicCapsItems.OrderBy(i => i.DisplayOrder).ToList();

                    return View(hatSelectModel);
                }
                else
                {
                    return RedirectToAction("Login", "User", null);
                }
            }
            else
            {
                return RedirectToAction("Login", "User", null);
            }
        }

        public string UpdateMasterBulkOrderItemInternalInventory(string id, string inventoryValue, string sizeValue)
        {
            BulkData data = new BulkData();
            MasterBulkOrderItem item = data.GetMasterBulkOrderItem(Convert.ToInt32(id));

            if(sizeValue == "OSFA")
            {
                item.OSFAInternalStock = Convert.ToInt32(inventoryValue);
            }
            if(sizeValue == "SM")
            {
                item.SMInternalStock = Convert.ToInt32(inventoryValue);
            }
            if(sizeValue == "LXL")
            {
                item.LXLInternalStock = Convert.ToInt32(inventoryValue);
            }
            if (sizeValue == "XL/XXL")
            {
                item.XLXXLInternalStock = Convert.ToInt32(inventoryValue);
            }

            var success = data.UpdateMasterBulkOrderItemInternalInventory(item);

            return new JavaScriptSerializer().Serialize(success);
        }

        public string UpdateInternalQuantitiesByBatchId(string batchId)
        {
            BulkData data = new BulkData();
            //update internal inventory values
            List<BulkOrder> lstBulkOrders = new List<BulkOrder>();

            lstBulkOrders = data.GetBulkOrdersByBatchId(Convert.ToInt32(batchId));

            lstBulkOrders = lstBulkOrders.Where(b => b.OrderPaid).ToList();

            List<BulkOrderItem> items = new List<BulkOrderItem>();

            foreach (BulkOrder bulkOrder in lstBulkOrders)
            {
                foreach (BulkOrderItem item in bulkOrder.lstItems)
                {
                    if (items.Any(p => p.ItemName == item.ItemName))
                    {
                        items.Find(p => p.ItemName == item.ItemName).ItemQuantity += item.ItemQuantity;
                    }
                    else
                    {
                        items.Add(item);
                    }
                }
            }

            items.RemoveAll(p => p.ItemName == "Artwork Setup/Digitizing");
            items.RemoveAll(p => p.ItemName == "Shipping");
            items.RemoveAll(p => p.ItemName == "Back Stitching");
            items.RemoveAll(p => p.ItemName == "Left Side Stitching");
            items.RemoveAll(p => p.ItemName == "Right Side Stitching");
            items.RemoveAll(p => p.ItemName == "3D Puff");
            items.RemoveAll(p => p.ItemName == "Expediting Fee");
            items.RemoveAll(p => p.ItemName == "Additional Artwork Setup");

            foreach (BulkOrderItem bulkOrderItem in items)
            {
                if (bulkOrderItem.MasterItemId > 0)
                {
                    MasterBulkOrderItem masterItem = data.GetMasterBulkOrderItem(Convert.ToInt32(bulkOrderItem.MasterItemId));

                    if (bulkOrderItem.ItemName.ToUpper().Contains("OSFA"))
                    {
                        if (masterItem.OSFAInternalStock<bulkOrderItem.ItemQuantity)
                        {
                            masterItem.OSFAInternalStock = 0;
                        }
                        else
                        {
                            masterItem.OSFAInternalStock = masterItem.OSFAInternalStock - bulkOrderItem.ItemQuantity;
                        }
                    }
                    if (bulkOrderItem.ItemName.ToUpper().Contains("S/M"))
                    {
                        if (masterItem.SMInternalStock<bulkOrderItem.ItemQuantity)
                        {
                            masterItem.SMInternalStock = 0;
                        }
                        else
                        {
                            masterItem.SMInternalStock = masterItem.SMInternalStock - bulkOrderItem.ItemQuantity;
                        }
                    }
                    if (bulkOrderItem.ItemName.ToUpper().Contains("L/XL"))
                    {
                        if (masterItem.LXLInternalStock<bulkOrderItem.ItemQuantity)
                        {
                            masterItem.LXLInternalStock = 0;
                        }
                        else
                        {
                            masterItem.LXLInternalStock = masterItem.LXLInternalStock - bulkOrderItem.ItemQuantity;
                        }
                    }
                    if (bulkOrderItem.ItemName.ToUpper().Contains("XL/XXL"))
                    {
                        if (masterItem.XLXXLInternalStock<bulkOrderItem.ItemQuantity)
                        {
                            masterItem.XLXXLInternalStock = 0;
                        }
                        else
                        {
                            masterItem.XLXXLInternalStock = masterItem.XLXXLInternalStock - bulkOrderItem.ItemQuantity;
                        }
                    }

                    var success = data.UpdateMasterBulkOrderItemInternalInventory(masterItem);
                }
            }

            var sucess = data.UpdateBulkOrderBatchInterntalStockUpdated(Convert.ToInt32(batchId));

            return new JavaScriptSerializer().Serialize(sucess);

        }
        public bool CheckLoggedIn()
        {
            if (Convert.ToInt32(Session["UserID"]) == 1 || Convert.ToInt32(Session["UserID"]) == 643 || Convert.ToInt32(Session["UserID"]) == 1579)
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