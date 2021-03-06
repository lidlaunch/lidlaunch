﻿using LidLaunchWebsite.Classes;
using LidLaunchWebsite.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using ShipStationAccess.V2;
using ShipStationAccess.V2.Models;
using ZXing;
using ZXing.Common;
using System.Drawing;
using System.Dynamic;
using System.Configuration;

namespace LidLaunchWebsite.Controllers
{
    public class BulkController : Controller
    {
        // GET: Bulk
        
        public ActionResult Index()
        {
            BulkOrder bulkOrder = new BulkOrder();
            bulkOrder.PaymentCompleteGuid = Guid.NewGuid().ToString();
            return View(bulkOrder);
        }
        public ActionResult IntoStep()
        {
            dynamic model = new ExpandoObject();

            //model.idVal = bulkOrderId;
            //model.fromBulkEdit = fromBulkEdit;

            return PartialView("IntoStep", model);
        }
        public ActionResult HatSelectStep()
        {
            //model.idVal = bulkOrderId;
            //model.fromBulkEdit = fromBulkEdit;
            BulkData data = new BulkData();
            BulkOrderHatSelectModel hatSelectModel = new BulkOrderHatSelectModel();
            List<MasterBulkOrderItem> masterItemList = data.GetMasterBulkOrderItems(false);

            hatSelectModel.FlexFit6277Items = masterItemList.Where(i => i.Manufacturer == "FlexFit" && i.ItemStyle == "6277" && i.Available).ToList();
            hatSelectModel.FlexFit6277Items = hatSelectModel.FlexFit6277Items.OrderBy(i => i.DisplayOrder).ToList();

            hatSelectModel.FlexFit6511Items = masterItemList.Where(i => i.Manufacturer == "FlexFit" && (i.ItemStyle == "6511 Trucker Fitted" || i.ItemStyle == "6311 Trucker Fitted") && i.Available).ToList();
            hatSelectModel.FlexFit6511Items = hatSelectModel.FlexFit6511Items.OrderBy(i => i.DisplayOrder).ToList();

            hatSelectModel.FlexFit110Items = masterItemList.Where(i => i.Manufacturer == "FlexFit" && i.ItemStyle == "110M Trucker Snapback" && i.Available).ToList();
            hatSelectModel.FlexFit110Items = hatSelectModel.FlexFit110Items.OrderBy(i => i.DisplayOrder).ToList();

            hatSelectModel.FlexFit6297Items = masterItemList.Where(i => i.Manufacturer == "FlexFit" && i.ItemStyle == "6297F Flat Bill Fitted" && i.Available).ToList();
            hatSelectModel.FlexFit6297Items = hatSelectModel.FlexFit6297Items.OrderBy(i => i.DisplayOrder).ToList();

            hatSelectModel.Yupoong6089Items = masterItemList.Where(i => i.Manufacturer == "Yupoong" && i.ItemStyle == "6089M Flat Bill Snapback" && i.Available).ToList();
            hatSelectModel.Yupoong6089Items = hatSelectModel.Yupoong6089Items.OrderBy(i => i.DisplayOrder).ToList();

            hatSelectModel.YupoongDadCapItems = masterItemList.Where(i => i.Manufacturer == "Yupoong" && i.ItemStyle == "6245CM Dad Cap" && i.Available).ToList();
            hatSelectModel.YupoongDadCapItems = hatSelectModel.YupoongDadCapItems.OrderBy(i => i.DisplayOrder).ToList();

            hatSelectModel.Yupoong6606Items = masterItemList.Where(i => i.Manufacturer == "Yupoong" && i.ItemStyle == "6606 Trucker Snapback" && i.Available).ToList();
            hatSelectModel.Yupoong6606Items = hatSelectModel.Yupoong6606Items.OrderBy(i => i.DisplayOrder).ToList();

            hatSelectModel.Yupoong6006Items = masterItemList.Where(i => i.Manufacturer == "Yupoong" && i.ItemStyle == "6006 Flat Bill Trucker Snapback" && i.Available).ToList();
            hatSelectModel.Yupoong6006Items = hatSelectModel.Yupoong6006Items.OrderBy(i => i.DisplayOrder).ToList();

            hatSelectModel.YupoongShortBeanieItems = masterItemList.Where(i => i.Manufacturer == "Yupoong" && i.ItemStyle == "Short Beanie" && i.Available).ToList();
            hatSelectModel.YupoongShortBeanieItems = hatSelectModel.YupoongShortBeanieItems.OrderBy(i => i.DisplayOrder).ToList();

            hatSelectModel.YupoongCuffedBeanieItems = masterItemList.Where(i => i.Manufacturer == "Yupoong" && i.ItemStyle == "Cuffed Beanie" && i.Available).ToList();
            hatSelectModel.YupoongCuffedBeanieItems = hatSelectModel.YupoongCuffedBeanieItems.OrderBy(i => i.DisplayOrder).ToList();

            hatSelectModel.Richardson112Items = masterItemList.Where(i => i.Manufacturer == "Richardson" && i.ItemStyle == "112 Trucker Snapback" && i.Available).ToList();
            hatSelectModel.Richardson112Items = hatSelectModel.Richardson112Items.OrderBy(i => i.DisplayOrder).ToList();

            hatSelectModel.ClassicCapsItems = masterItemList.Where(i => i.Manufacturer == "Classic Caps" && i.ItemStyle == "USA100 Trucker Snapback" && i.Available).ToList();
            hatSelectModel.ClassicCapsItems = hatSelectModel.ClassicCapsItems.OrderBy(i => i.DisplayOrder).ToList();


            return PartialView("HatSelectStep", hatSelectModel);
        }
        public ActionResult ThreadColorChart()
        {
            return PartialView("ThreadColorChart");
        }
        public ActionResult ArtworkStep()
        {
            dynamic model = new ExpandoObject();

            //model.idVal = bulkOrderId;
            //model.fromBulkEdit = fromBulkEdit;

            return PartialView("ArtworkStep", model);
        }
        public ActionResult OrderLookup()
        {
            return View();
        }
        public ActionResult Payment(string id)
        {
            BulkData data = new BulkData();
            
            BulkOrder bulkOrder = new BulkOrder();
            bulkOrder = data.GetBulkOrder(0, "", id);

            if(bulkOrder != null && !bulkOrder.OrderPaid)
            {
                var success = data.UpdateBulkOrderPaidByPaymentCompleteGuid(id);
                EmailFunctions emailFunc = new EmailFunctions();
                List<PaypalItem> items = new List<PaypalItem>();
                foreach (BulkOrderItem bulkItem in bulkOrder.lstItems)
                {
                    PaypalItem newItem = new PaypalItem();
                    newItem.name = bulkItem.ItemName;
                    newItem.price = bulkItem.ItemCost;
                    newItem.quantity = bulkItem.ItemQuantity.ToString();
                    items.Add(newItem);
                }
                var dateFrom = data.AddBusinessDays(DateTime.Now, 10).ToString("MM/dd/yyyy");
                var dateTo = data.AddBusinessDays(DateTime.Now, 14).ToString("MM/dd/yyyy");
                var emailSuccess = emailFunc.sendEmail(bulkOrder.CustomerEmail, bulkOrder.CustomerName, emailFunc.bulkOrderPaymentEmail(items, bulkOrder.OrderTotal.ToString(), bulkOrder.Id.ToString(), bulkOrder.PaymentGuid, dateFrom + " - " + dateTo), "LidLaunch Payment Confirmation", "", "");
                var addBulkOrderLogSuccess = data.AddBulkOrderLog(bulkOrder.Id, 0, "Payment Received");
            }
            
            return View(bulkOrder);
        }

        public string SetBulkOrderAsShipped(string bulkOrderId, string trackingNumber, string noEmail)
        {
            BulkData data = new BulkData();

            BulkOrder bulkOrder = new BulkOrder();
            bulkOrder = data.GetBulkOrder(Convert.ToInt32(bulkOrderId), "", "");
            //if (bulkOrder != null && !bulkOrder.OrderComplete && trackingNumber != "")
            if (bulkOrder != null && !bulkOrder.OrderComplete)
            {
                var success = data.UpdateBulkOrderSetOrderAsShipped(Convert.ToInt32(bulkOrderId), trackingNumber);
                var addBulkOrderLogSuccess = data.AddBulkOrderLog(bulkOrder.Id, Convert.ToInt32(Session["UserID"]), "Order Marked As Shipped");
                EmailFunctions emailFunc = new EmailFunctions();

                if (Convert.ToBoolean(noEmail))
                {
                    return success.ToString();
                }
                else
                {
                    //uncomment for trust pilot roll out...
                    //var emailSuccess = emailFunc.sendEmai(bulkOrder.CustomerEmail, bulkOrder.CustomerName, emailFunc.bulkOrderShippedEmail(trackingNumber), "LidLaunch Order #BO-" + bulkOrderId + " Shipped", "", "lidlaunch.com+db98b13610@invite.trustpilot.com");
                    var emailSuccess = emailFunc.sendEmail(bulkOrder.CustomerEmail, bulkOrder.CustomerName, emailFunc.bulkOrderShippedEmail(trackingNumber), "LidLaunch Order #BO-" + bulkOrderId + " Shipped", "", "");
                    return emailSuccess.ToString();
                }
                
            }

            return "";
        }

        public string SearchBulkDesigns(string email)
        {
            BulkData data = new BulkData();
            List<Design> lstDesigns = new List<Design>();

            lstDesigns = data.GetBulkDesigns(email);

            return Newtonsoft.Json.JsonConvert.SerializeObject(lstDesigns);  
        }

        public string OrderSearch(string email, string firstName, string lastName, string zipCode)
        {
            BulkData data = new BulkData();
            List<BulkOrder> lstBulkOrders = new List<BulkOrder>();
            var orderString = "";

            lstBulkOrders = data.SearchBulkOrders(email, firstName, lastName, zipCode);

            lstBulkOrders = lstBulkOrders.OrderByDescending(bo => bo.Id).ToList();

            foreach(BulkOrder bo in lstBulkOrders)
            {
                var status = "";

                if(bo.OrderPaid)
                {
                    status = bo.OrderComplete ? "COMPLETE" : "IN PROCESS";
                }
                else if(bo.OrderRefunded)
                {
                    status = "REFUNDED";
                }
                else
                {
                    status = "UNPAID";
                }
                orderString += "<tr><td>" + bo.Id.ToString() + "</td><td>" + bo.OrderDate + "</td><td>" + status + "</td><td><a href='/bulk/orderstatus?id=" + bo.PaymentGuid + "' target='_blank'><input type='button' value='VIEW DETAILS' class='smallButton' /></a></td></tr>";
            }

            return orderString;
        }

        public string RequestDigitizingRevision(string id, string text, string bulkOrderId, bool customerAdded)
        {
            BulkData data = new BulkData();
            var revisionStatus = customerAdded ? "1:Pending" : "5:OutsourcedChangesPending";
            var success = data.AddDigitizingRevision(Convert.ToInt32(id), text, customerAdded, revisionStatus);
            if(success)
            {
                BulkData bulkData = new BulkData();
                BulkOrder bulkOrder = bulkData.GetBulkOrder(Convert.ToInt32(bulkOrderId), "", "");
                EmailFunctions email = new EmailFunctions();
                email.sendEmail(bulkOrder.CustomerEmail, bulkOrder.CustomerName, email.revisionRequestedEmail(text), "Order " + bulkOrderId + " : Artwork Revision Request", "digitizing@lidlaunch.com", "");
                var addBulkOrderLogSuccess = data.AddBulkOrderLog(bulkOrder.Id, Convert.ToInt32(Session["UserID"]), "Digitizing Revision: " + (customerAdded ? "Customer Requested" : "Internally Requested") + " :: " + text);
            }
            return success.ToString();
        }

        public string InternallyApproveDigitizing(string id, string bulkOrderId)
        {
            BulkData data = new BulkData();
            var success = data.InternallyApproveBulkOrderDigitizing(Convert.ToInt32(id));
            if (success)
            {
                BulkData bulkData = new BulkData();
                BulkOrder bulkOrder = bulkData.GetBulkOrder(Convert.ToInt32(bulkOrderId), "", "");
                EmailFunctions email = new EmailFunctions();
                if (!bulkOrder.lstDesigns.Any(d => !d.InternallyApproved))
                {
                    email.sendEmail(bulkOrder.CustomerEmail, bulkOrder.CustomerName, email.digitizingPreviewUploaded(bulkOrder.PaymentGuid), "View & Approve Your Stitch Previews", "", "");
                    var addBulkOrderLogSuccess = data.AddBulkOrderLog(bulkOrder.Id, Convert.ToInt32(Session["UserID"]), "Digitzing Approved Interally : DesignId = " + id.ToString());
                }
            }
            return success.ToString();
        }

        public string InternallyApproveBulkOrder(string id, string approve)
        {
            BulkData data = new BulkData();
            var success = data.InternallyApproveBulkOrder(Convert.ToInt32(id), Convert.ToBoolean(approve));

            var addBulkOrderLogSuccess = data.AddBulkOrderLog(Convert.ToInt32(id), Convert.ToInt32(Session["UserID"]), "Bulk Order Marked " + (Convert.ToBoolean(approve) ? "Ready for Production" : "NOT Ready for Production"));
            //if (success)
            //{
            //    BulkData bulkData = new BulkData();
            //    BulkOrder bulkOrder = bulkData.GetBulkOrder(Convert.ToInt32(id), "", "");
            //    EmailFunctions email = new EmailFunctions();
            //    if (!bulkOrder.lstDesigns.Any(d => !d.InternallyApproved))
            //    {
            //        email.sendEmail(bulkOrder.CustomerEmail, bulkOrder.CustomerName, email.digitizingPreviewUploaded(bulkOrder.PaymentGuid), "View & Approve Your Stitch Previews", "");
            //    }
            //}
            return success.ToString();
        }        

        public string ApproveDigitizing(string id, string bulkOrderId)
        {
            BulkData data = new BulkData();
            var success = data.ApproveBulkOrderDigitizing(Convert.ToInt32(id));
            if (success)
            {
                BulkData bulkData = new BulkData();
                BulkOrder bulkOrder = bulkData.GetBulkOrder(Convert.ToInt32(bulkOrderId), "", "");
                EmailFunctions email = new EmailFunctions();
                email.sendEmail(bulkOrder.CustomerEmail, bulkOrder.CustomerName, email.digitizingApproved(), "You Have Approved Your Artwork Previews", "", "");
                var addBulkOrderLogSuccess = data.AddBulkOrderLog(Convert.ToInt32(bulkOrderId), 0, "Customer Approved Digitzing For Design Id: " + id);
            }
            return success.ToString();
        }

        public string CreateBulkOrder(string items, string email, string artworkPlacement, string orderNotes, string orderTotal, string paymentCompleteGuid, string shippingCost, HttpPostedFileBase fileContent, string billToState, string billToAddress, string billToZip, string billToPhone, string billToCity, string billToName, string shipToState, string shipToAddress, string shipToZip, string shipToPhone, string shipToCity, string shipToName, string backStitching, string leftSideStitching, string rightSideStitching, string backStitchingComment, string leftSideStitchingComment, string rightSideStitchingComment)
        {
            try
            {
                var developmentMode = Convert.ToBoolean(ConfigurationManager.AppSettings["DevelopmentMode"]);
                var jss = new JavaScriptSerializer();
                var cartItems = jss.Deserialize<List<PaypalItem>>(items);
                var artworkPath = "";

                if (fileContent != null && fileContent.ContentLength > 0)
                {
                    // get a stream
                    var stream = fileContent.InputStream;
                    // and optionally write the file to disk
                    var extension = Path.GetExtension(fileContent.FileName);

                    var fileName = Guid.NewGuid() + extension;
                    artworkPath = fileName;
                    var path = Path.Combine(HttpRuntime.AppDomainAppPath + "/Images/BulkOrderArtwork", fileName);

                    fileContent.SaveAs(path);
                }
                else
                {
                    HttpPostedFileBase fileContent2 = null;
                    if (Request != null && Request.Files.Count > 0)
                    {
                        fileContent2 = Request.Files[0];
                    }

                    if (fileContent2 != null && fileContent2.ContentLength > 0)
                    {
                        // get a stream
                        var stream = fileContent2.InputStream;
                        // and optionally write the file to disk
                        var extension = Path.GetExtension(fileContent2.FileName);

                        var fileName = Guid.NewGuid() + extension;
                        artworkPath = fileName;
                        var path = Path.Combine(HttpRuntime.AppDomainAppPath + "/Images/BulkOrderArtwork/", fileName);

                        fileContent2.SaveAs(path);
                    }
                }

                BulkData bulkData = new BulkData();
                var paymentGuid = Guid.NewGuid().ToString();
                var orderId = bulkData.CreateBulkOrder(shipToName, email, shipToPhone, Convert.ToDecimal(orderTotal), orderNotes, artworkPath, artworkPlacement, cartItems, paymentCompleteGuid, paymentGuid, Convert.ToDecimal(shippingCost), shipToName, shipToAddress, shipToCity, shipToState, shipToZip, shipToPhone, billToName, billToAddress, billToCity, billToState, billToZip, billToPhone, Convert.ToBoolean(backStitching), Convert.ToBoolean(leftSideStitching), Convert.ToBoolean(rightSideStitching), Convert.ToString(backStitchingComment), Convert.ToString(leftSideStitchingComment), Convert.ToString(rightSideStitchingComment));

                DesignData designData = new DesignData();
                var designId = designData.CreateDesign(artworkPath, "", 0.0M, 0.0M, 0.0M, 0.0M, 0.0M, 0.0M, 0.0M, 0.0M, "Front", "", "", "", "");

                bulkData.CreateBulkOrderDesign(orderId, designId);

                try
                {
                    //create barcode file
                    var barcodeImage = "BO-" + orderId.ToString() + ".jpg";
                    if (!System.IO.File.Exists(HttpRuntime.AppDomainAppPath + "/Images/Barcodes/" + barcodeImage))
                    {
                        //generate barcode image
                        IBarcodeWriter barcodeWriter = new BarcodeWriter
                        {
                            Format = BarcodeFormat.CODE_39,
                            Options = new EncodingOptions
                            {
                                Height = 100,
                                Width = 300
                            }
                        };
                        Bitmap barcode = barcodeWriter.Write("BO-" + orderId.ToString());
                        barcode.Save(HttpRuntime.AppDomainAppPath + "/Images/Barcodes/" + barcodeImage);
                    }
                    else
                    {
                        //do nothing
                    }
                }
                catch(Exception ex)
                {                    
                    Logger.Log("Error Creating Barcode : Bulk Order Id = " + orderId.ToString() + " :: EXCEPTION :" + ex.Message.ToString());
                }
                

                if (orderId > 0)
                {
                    try
                    {
                        EmailFunctions emailFunc = new EmailFunctions();
                        var emailSuccess = emailFunc.sendEmail(email, shipToName, emailFunc.bulkOrderEmail(cartItems, orderTotal, orderId.ToString(), paymentGuid), "LidLaunch Order Confirmation", "", "");
                    }
                    catch (Exception ex)
                    {
                        Logger.Log("Error Sending Bulk Order Email Confirmation: Bulk Order ID: " + orderId.ToString() + " EmailTo: " + email + " - Exception: " + ex.Message.ToString());
                    }

                    var addBulkOrderLogSuccess = bulkData.AddBulkOrderLog(Convert.ToInt32(orderId), 0, "Bulk Order Created");

                    if (!developmentMode)
                    {
                        try
                        {
                            //insert order into ship station
                            ShipStationCredentials credentials = new ShipStationCredentials("a733e1314b6f4374bd12f4a32d4263b9", "bd45d90bfbae40d39f5d7e8b3966f130");
                            ShipStationService shipService = new ShipStationService(credentials);
                            ShipStationAccess.V2.Models.Order.ShipStationOrder order = new ShipStationAccess.V2.Models.Order.ShipStationOrder();
                            order.OrderNumber = "BO-" + orderId.ToString();
                            order.OrderKey = "BO-" + orderId.ToString();
                            order.OrderDate = DateTime.Now;
                            ShipStationAddress billAddress = new ShipStationAddress();
                            billAddress.Name = billToName;
                            billAddress.Phone = billToPhone;
                            billAddress.State = billToState;
                            billAddress.PostalCode = billToZip;
                            billAddress.Street1 = billToAddress;
                            billAddress.City = billToCity;
                            billAddress.Country = "US";
                            order.BillingAddress = billAddress;
                            ShipStationAddress shipAddress = new ShipStationAddress();
                            shipAddress.Name = shipToName;
                            shipAddress.Phone = shipToPhone;
                            shipAddress.State = shipToState;
                            shipAddress.PostalCode = shipToZip;
                            shipAddress.Street1 = shipToAddress;
                            shipAddress.City = shipToCity;
                            shipAddress.Country = "US";
                            order.ShippingAddress = shipAddress;
                            order.CustomerEmail = email;
                            order.AmountPaid = Convert.ToDecimal(orderTotal);
                            order.CustomerNotes = orderNotes;
                            order.OrderStatus = ShipStationAccess.V2.Models.Order.ShipStationOrderStatusEnum.awaiting_shipment;
                            shipService.UpdateOrderAsync(order);
                        }
                        catch (Exception ex)
                        {
                            Logger.Log("Error Importing Into Ship Station: Bulk Order ID: " + ex.Message.ToString());
                        }
                    }
                    return orderId.ToString();
                }
                else
                {
                    Logger.Log("Bulk Order Order ID was not > 0 : " + email + " - " + shipToName + " ::: Items: " + items + " ::: OrderNotes: " + orderNotes + ", Total: " + orderTotal + ", Placement: " + artworkPlacement);
                    return "";
                }

            }
            catch (Exception ex)
            {
                Logger.Log("Error Submitting Order: " + ex.Message.ToString());
            }
            return "";            
            
        }

        public ActionResult OrderStatus(string id)
        {
            BulkOrder bulkOrder = new BulkOrder();
            BulkData data = new BulkData();
            bulkOrder = data.GetBulkOrder(0, id, "");          

            return View(bulkOrder);
        }

        public string CreateBulkOrderBatch()
        {
            if (checkAdminLoggedIn())
            {
                BulkData data = new BulkData();
                var batchId = data.CreateBulkOrderBatch();
                

                return batchId.ToString();
            }
            else
            {
                return "";
            }            
        }

        public ActionResult BulkOrderBatches()
        {
            BulkData data = new BulkData();
            List<OrderBatch> lstBatches = new List<OrderBatch>();

            lstBatches = data.GetBulkOrderBatches();

            return PartialView("BulkOrderBatches", lstBatches); ;
        }

        public ActionResult PrintPackingSlip(string bulkOrderId)
        {
            BulkData data = new BulkData();
            BulkOrder bulkOrder = new BulkOrder();

            bulkOrder = data.GetBulkOrder(Convert.ToInt32(bulkOrderId), "", "");

            var addBulkOrderLogSuccess = data.AddBulkOrderLog(Convert.ToInt32(bulkOrderId), 0, "Packing Slip Printed");

            return View(bulkOrder);
        }

        public ActionResult PrintBulkOrderBatchBulkOrders(string bulkBatchId, string bulkOrderId, string rework)
        {
            BulkData data = new BulkData();
            BulkBatchOrder bulkBatchOrder = new BulkBatchOrder();
            List<BulkOrder> lstBulkOrders = new List<BulkOrder>();
            bool isRework = Convert.ToBoolean(rework);
            int id = Convert.ToInt32(bulkOrderId);

            if(isRework)
            {
                lstBulkOrders = data.GetBulkOrderData("rework");
            } else
            {
                lstBulkOrders = data.GetBulkOrdersByBatchId(Convert.ToInt32(bulkBatchId));
            }
            lstBulkOrders = lstBulkOrders.Where(b => b.OrderPaid && (!b.OrderComplete || b.lstItems.Any(li => li.BulkRework.Id > 0))).ToList();

            bulkBatchOrder.lstBulkOrders = lstBulkOrders;

            if(id > 0)
            {
                bulkBatchOrder.lstBulkOrders = lstBulkOrders.Where(b => b.Id == id).ToList();
            }

            bulkBatchOrder.lstBulkOrders = bulkBatchOrder.lstBulkOrders.OrderByDescending(lb => lb.lstItems.Any(i => i.ItemName == "Expediting Fee") || lb.HasRework).ThenBy(b => b.ProjectedShipDateLong).ToList();

            return View(bulkBatchOrder);
        }

        public ActionResult BulkOrderBatch(string bulkBatchId, string onlyOutOfStock)
        {
            BulkData data = new BulkData();
            BulkBatchOrder bulkBatchOrder = new BulkBatchOrder();
            List<BulkOrder> lstBulkOrders = new List<BulkOrder>();
            List<BulkOrderBatchMissingItems> lstMissingItems = new List<BulkOrderBatchMissingItems>();

            lstBulkOrders = data.GetBulkOrdersByBatchId(Convert.ToInt32(bulkBatchId));

            lstBulkOrders = lstBulkOrders.Where(b => b.OrderPaid).ToList();

            bulkBatchOrder.lstBulkOrders = lstBulkOrders;
            bulkBatchOrder.batchInfo = data.GetBulkOrderBatch(Convert.ToInt32(bulkBatchId));

            List<BulkOrderItem> products = new List<BulkOrderItem>();

            foreach (BulkOrder bulkOrder in lstBulkOrders)
            {
                foreach (BulkOrderItem item in bulkOrder.lstItems)
                {
                    if (products.Any(p => p.ItemName == item.ItemName))
                    {
                        products.Find(p => p.ItemName == item.ItemName).ItemQuantity += item.ItemQuantity;
                    } else
                    {
                        products.Add(item);
                    }
                }
            }

            products.RemoveAll(p => p.ItemName == "Artwork Setup/Digitizing");
            products.RemoveAll(p => p.ItemName == "Shipping");
            products.RemoveAll(p => p.ItemName == "Back Stitching");
            products.RemoveAll(p => p.ItemName == "Left Side Stitching");
            products.RemoveAll(p => p.ItemName == "Right Side Stitching");
            products.RemoveAll(p => p.ItemName == "3D Puff");
            products.RemoveAll(p => p.ItemName == "Expediting Fee");
            products.RemoveAll(p => p.ItemName == "Additional Artwork Setup");

            bulkBatchOrder.lstItemsToOrder = products;

            List<MasterBulkOrderItem> masterItems = data.GetMasterBulkOrderItems(false);
            foreach (BulkOrderItem item in bulkBatchOrder.lstItemsToOrder)
            {
                if (item.MasterItemId > 0)
                {
                    if (item.ItemName.ToUpper().Contains("OSFA"))
                    {
                        item.InternalStock = masterItems.First(mI => mI.Id == item.MasterItemId).OSFAInternalStock;
                        item.ExtrernalStock = masterItems.First(mI => mI.Id == item.MasterItemId).OSFAExternalStock;
                    }
                    if (item.ItemName.ToUpper().Contains("S/M"))
                    {
                        item.InternalStock = masterItems.First(mI => mI.Id == item.MasterItemId).SMInternalStock;
                        item.ExtrernalStock = masterItems.First(mI => mI.Id == item.MasterItemId).SMExternalStock;
                    }
                    if (item.ItemName.ToUpper().Contains("L/XL"))
                    {
                        item.InternalStock = masterItems.First(mI => mI.Id == item.MasterItemId).LXLInternalStock;
                        item.ExtrernalStock = masterItems.First(mI => mI.Id == item.MasterItemId).LXLExternalStock;
                    }
                    if (item.ItemName.ToUpper().Contains("XL/XXL"))
                    {
                        item.InternalStock = masterItems.First(mI => mI.Id == item.MasterItemId).XLXXLInternalStock;
                        item.ExtrernalStock = masterItems.First(mI => mI.Id == item.MasterItemId).XLXXLExternalStock;
                    }
                }                
            }

            bulkBatchOrder.lstItemsToOrder = bulkBatchOrder.lstItemsToOrder.OrderByDescending(bo => bo.ItemName).ToList();

            bulkBatchOrder.lstItemsToOrder.Add(new BulkOrderItem { ItemName = "TOTAL HATS", ItemQuantity = products.Sum(p => p.ItemQuantity) });
            bulkBatchOrder.lstItemsToOrder.Add(new BulkOrderItem { ItemName = "ESTIMATED TOTAL COST", ItemQuantity = Convert.ToInt32(data.GetBlankCost(Convert.ToInt32(bulkBatchId))) });

            




            bulkBatchOrder.lstMissingItems = data.GetBulkOrderBatchMissingItems(Convert.ToInt32(bulkBatchId));

            var filterOnlyOutOfstock = false;
            if(onlyOutOfStock != "" || onlyOutOfStock == null)
            {
                filterOnlyOutOfstock = Convert.ToBoolean(onlyOutOfStock);
            }
            if (filterOnlyOutOfstock)
            {
                HashSet<int> missingItemIds = new HashSet<int>(bulkBatchOrder.lstMissingItems.Where(mi => mi.MissingQuantity > 0).Select(mi => mi.MasterBulkOrderItemId));
                bulkBatchOrder.lstItemsToOrder = bulkBatchOrder.lstItemsToOrder.Where(i => missingItemIds.Contains(i.MasterItemId)).ToList();
            }


            return View(bulkBatchOrder);
        }

        public ActionResult AdminReview(int bulkOrderId, bool fromBulkEdit)
        {
            dynamic model = new ExpandoObject();

            model.idVal = bulkOrderId;
            model.fromBulkEdit = fromBulkEdit;         
            

            return PartialView("AdminReview", model);
        }

        public string SaveAdminReview(string bulkOrderId, string comment, string designerReview)
        {
            BulkData data = new BulkData();
            var success = data.SaveAdminReview(Convert.ToInt32(bulkOrderId), comment, Convert.ToBoolean(designerReview));
            if (Convert.ToBoolean(designerReview))
            {
                BulkOrder bulkOrder = new BulkOrder();
                bulkOrder = data.GetBulkOrder(Convert.ToInt32(bulkOrderId), "", "");
                if(bulkOrder.lstDesigns.Any(d => d.Revision))
                {
                    data.AddDigitizingRevision(bulkOrder.lstDesigns.First(d => d.Revision).Id, "REVISION INTERANALLY REQUESTED: " + comment, false, "4:InternalChangesPending");
                }
            }
            var addBulkOrderLogSuccess = data.AddBulkOrderLog(Convert.ToInt32(bulkOrderId), Convert.ToInt32(Session["UserId"]), "Order Set As " + (Convert.ToBoolean(designerReview) ? "Designer Review" : "Admin Review") + " : " + comment);
            return success.ToString();
        }

        public string UpdateAdminReviewFinished(string bulkOrderId)
        {
            BulkData data = new BulkData();
            var success = data.UpdateAdminReviewFinished(Convert.ToInt32(bulkOrderId));
           
            return success.ToString();
        }

        public string GetPreExistingDesign(string searchString, string bulkOrderId)
        {
            BulkData data = new BulkData();
            List<BulkOrder> lstBulkOrders = data.GetBulkOrderData("");
            searchString = searchString.ToLower().Replace("bo-", "");
            int searchStringIntValue = 0;
            int.TryParse(searchString, out searchStringIntValue);
            if (searchStringIntValue == 0)
            {
                lstBulkOrders = lstBulkOrders.Where(b => b.CustomerEmail.ToLower() == searchString.ToLower().Trim()).ToList();
            }
            else
            {
                lstBulkOrders = lstBulkOrders.Where(b => b.Id == searchStringIntValue).ToList();
            }

            
            var returnHtml = "";
            foreach(BulkOrder bulkOrder in lstBulkOrders)
            {
                foreach(Design design in bulkOrder.lstDesigns.Where(d => d.InternallyApproved).ToList())
                {
                    returnHtml += "<div id='bulkDesignOption'>";
                    returnHtml += "<div><b>Order ID: BO-" + bulkOrder.Id + " : " + bulkOrder.OrderDate.ToString("dd/MM/yyyy") + "</b></div>";
                    returnHtml += "<div><img src='../Images/DesignImages/Digitizing/Preview/" + design.DigitizedPreview + "'/></div>";
                    returnHtml += "<div><input type='button' onclick='selectDesign(" + bulkOrderId + ", " + design.Id + ");' class='smallButton' value='Use This Design'/></div>";
                    returnHtml += "</div>";
                }
                
            }
            return returnHtml;
        }

        public string GetPreExistingDesignInternal(string searchString, string bulkOrderId)
        {
            BulkData data = new BulkData();
            List<BulkOrder> lstBulkOrders = data.GetBulkOrderData("");
            searchString = searchString.ToLower().Replace("bo-", "");
            int searchStringIntValue = 0;
            int.TryParse(searchString, out searchStringIntValue);
            if (searchStringIntValue == 0)
            {
                lstBulkOrders = lstBulkOrders.Where(b => b.CustomerEmail.ToLower() == searchString.ToLower().Trim()).ToList();
            }
            else
            {
                lstBulkOrders = lstBulkOrders.Where(b => b.Id == searchStringIntValue).ToList();
            }


            var returnHtml = "";
            foreach (BulkOrder bulkOrder in lstBulkOrders)
            {
                foreach (Design design in bulkOrder.lstDesigns)
                {
                    returnHtml += "<div id='bulkDesignOption'>";
                    returnHtml += "<div><b>Order ID: BO-" + bulkOrder.Id + " : " + bulkOrder.OrderDate.ToString("dd/MM/yyyy") + " : " + design.Name + "</b></div>";
                    returnHtml += "<div><img src='../Images/DesignImages/Digitizing/Preview/" + design.DigitizedPreview + "'/></div>";
                    returnHtml += "<div><a href='../Images/DesignImages/Digitizing/Info/" + design.DigitizedProductionSheet + "' target='_blank'>VIEW PDF</a></div>";
                    returnHtml += "<div><input type='button' onclick='selectDesign(" + bulkOrderId + ", " + design.Id + ");' class='smallButton' value='Use This Design'/></div>";                    
                    returnHtml += "</div>";
                }
            }
            return returnHtml;
        }

        public bool checkAdminLoggedIn()
        {
            if (Convert.ToInt32(Session["UserID"]) == 1 || Convert.ToInt32(Session["UserID"]) == 1579)
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