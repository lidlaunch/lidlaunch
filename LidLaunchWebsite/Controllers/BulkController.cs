using LidLaunchWebsite.Classes;
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
            dynamic model = new ExpandoObject();

            //model.idVal = bulkOrderId;
            //model.fromBulkEdit = fromBulkEdit;

            return PartialView("HatSelectStep", model);
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
                var emailSuccess = emailFunc.sendEmail(bulkOrder.CustomerEmail, bulkOrder.CustomerName, emailFunc.bulkOrderPaymentEmail(items, bulkOrder.OrderTotal.ToString(), bulkOrder.Id.ToString(), bulkOrder.PaymentGuid, dateFrom + " - " + dateTo), "Lid Launch Payment Confirmation", "");
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
                EmailFunctions emailFunc = new EmailFunctions();

                if (Convert.ToBoolean(noEmail))
                {
                    return success.ToString();
                }
                else
                {
                    var emailSuccess = emailFunc.sendEmail(bulkOrder.CustomerEmail, bulkOrder.CustomerName, emailFunc.bulkOrderShippedEmail(trackingNumber), "Lid Launch Order Shipped", "");
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

        public string RequestDigitizingRevision(string id, string text, string bulkOrderId, bool customerAdded)
        {
            BulkData data = new BulkData();
            var success = data.AddDigitizingRevision(Convert.ToInt32(id), text, customerAdded);
            if(success)
            {
                BulkData bulkData = new BulkData();
                BulkOrder bulkOrder = bulkData.GetBulkOrder(Convert.ToInt32(bulkOrderId), "", "");
                EmailFunctions email = new EmailFunctions();
                email.sendEmail(bulkOrder.CustomerEmail, bulkOrder.CustomerName, email.revisionRequestedEmail(text), "Order " + bulkOrderId + " : Artwork Revision Request", "digitizing@lidlaunch.com");                
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
                    email.sendEmail(bulkOrder.CustomerEmail, bulkOrder.CustomerName, email.digitizingPreviewUploaded(bulkOrder.PaymentGuid), "View & Approve Your Stitch Previews", "");
                }
            }
            return success.ToString();
        }

        public string InternallyApproveBulkOrder(string id, string approve)
        {
            BulkData data = new BulkData();
            var success = data.InternallyApproveBulkOrder(Convert.ToInt32(id), Convert.ToBoolean(approve));
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
                email.sendEmail(bulkOrder.CustomerEmail, bulkOrder.CustomerName, email.digitizingApproved(), "You Have Approved Your Artwork Previews", "");
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
                        var emailSuccess = emailFunc.sendEmail(email, shipToName, emailFunc.bulkOrderEmail(cartItems, orderTotal, orderId.ToString(), paymentGuid), "Lid Launch Order Confirmation", "");
                    }
                    catch (Exception ex)
                    {
                        Logger.Log("Error Sending Bulk Order Email Confirmation: Bulk Order ID: " + orderId.ToString() + " EmailTo: " + email + " - Exception: " + ex.Message.ToString());
                    }

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

            return View(bulkOrder);
        }

        public ActionResult PrintBulkOrderBatchBulkOrders(string bulkBatchId, string rework)
        {
            BulkData data = new BulkData();
            BulkBatchOrder bulkBatchOrder = new BulkBatchOrder();
            List<BulkOrder> lstBulkOrders = new List<BulkOrder>();
            bool isRework = Convert.ToBoolean(rework);

            if(isRework)
            {
                lstBulkOrders = data.GetBulkOrderData("rework");
            } else
            {
                lstBulkOrders = data.GetBulkOrdersByBatchId(Convert.ToInt32(bulkBatchId));
            }
            lstBulkOrders = lstBulkOrders.Where(b => b.OrderPaid && !b.OrderComplete).ToList();

            bulkBatchOrder.lstBulkOrders = lstBulkOrders;

            return View(bulkBatchOrder);
        }

        public ActionResult BulkOrderBatch(string bulkBatchId)
        {
            BulkData data = new BulkData();
            BulkBatchOrder bulkBatchOrder = new BulkBatchOrder();
            List<BulkOrder> lstBulkOrders = new List<BulkOrder>();

            lstBulkOrders = data.GetBulkOrdersByBatchId(Convert.ToInt32(bulkBatchId));

            lstBulkOrders = lstBulkOrders.Where(b => b.OrderPaid).ToList();

            bulkBatchOrder.lstBulkOrders = lstBulkOrders;
            bulkBatchOrder.batchInfo = new OrderBatch();
            bulkBatchOrder.batchInfo.BatchId = Convert.ToInt32(bulkBatchId);

            List<BulkOrderItem> products = new List<BulkOrderItem>();

            foreach(BulkOrder bulkOrder in lstBulkOrders)
            {
                foreach(BulkOrderItem item in bulkOrder.lstItems)
                {
                    if(products.Any(p => p.ItemName == item.ItemName))
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

            bulkBatchOrder.lstItemsToOrder = products;

            bulkBatchOrder.lstItemsToOrder = bulkBatchOrder.lstItemsToOrder.OrderByDescending(bo => bo.ItemName).ToList();

            bulkBatchOrder.lstItemsToOrder.Add(new BulkOrderItem { ItemName = "TOTAL HATS", ItemQuantity = products.Sum(p => p.ItemQuantity) });
            bulkBatchOrder.lstItemsToOrder.Add(new BulkOrderItem { ItemName = "ESTIMATED TOTAL COST", ItemQuantity = Convert.ToInt32(data.GetBlankCost(Convert.ToInt32(bulkBatchId))) });

            



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
            
            return success.ToString();
        }

        public string UpdateAdminReviewFinished(string bulkOrderId)
        {
            BulkData data = new BulkData();
            var success = data.UpdateAdminReviewFinished(Convert.ToInt32(bulkOrderId));
           
            return success.ToString();
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