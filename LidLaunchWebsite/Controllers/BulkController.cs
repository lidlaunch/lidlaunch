using LidLaunchWebsite.Classes;
using LidLaunchWebsite.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

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
                var emailSuccess = emailFunc.sendEmail(bulkOrder.CustomerEmail, bulkOrder.CustomerName, emailFunc.bulkOrderPaymentEmail(items, bulkOrder.OrderTotal.ToString(), bulkOrder.Id.ToString(), bulkOrder.PaymentGuid), "Lid Launch Payment Confirmation", "");
            }
            
            return View();
        }

        public string SetBulkOrderAsShipped(string bulkOrderId, string trackingNumber)
        {
            BulkData data = new BulkData();

            BulkOrder bulkOrder = new BulkOrder();
            bulkOrder = data.GetBulkOrder(Convert.ToInt32(bulkOrderId), "", "");

            if (bulkOrder != null && !bulkOrder.OrderComplete)
            {
                var success = data.UpdateBulkOrderSetOrderAsShipped(Convert.ToInt32(bulkOrderId), trackingNumber);
                EmailFunctions emailFunc = new EmailFunctions();
                
                var emailSuccess = emailFunc.sendEmail(bulkOrder.CustomerEmail, bulkOrder.CustomerName, emailFunc.bulkOrderShippedEmail(trackingNumber), "Lid Launch Order Shipped", "");

                return emailSuccess.ToString();
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




        public string ApproveDigitizing(string id)
        {
            BulkData data = new BulkData();
            var success = data.ApproveBulkOrderDigitizing(Convert.ToInt32(id));
            return success.ToString();
        }

        public string CreateBulkOrder(string items, string name, string email, string phone, string artworkPlacement, string orderNotes, string orderTotal, string paymentCompleteGuid, string shippingCost, HttpPostedFileBase fileContent)
        {
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
                var path = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory + "Images\\BulkOrderArtwork\\", fileName);

                fileContent.SaveAs(path);
            }
            else
            {
                HttpPostedFileBase fileContent2 = null;
                if (Request.Files.Count > 0)
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
                    var path = Path.Combine(Server.MapPath("~/Images/BulkOrderArtwork/"), fileName);

                    fileContent2.SaveAs(path);
                }
            }

            BulkData bulkData = new BulkData();
            var paymentGuid = Guid.NewGuid().ToString();
            var orderId = bulkData.CreateBulkOrder(name, email, phone, Convert.ToDecimal(orderTotal), orderNotes, artworkPath, artworkPlacement, cartItems, paymentCompleteGuid, paymentGuid, Convert.ToDecimal(shippingCost));

            DesignData designData = new DesignData();
            var designId = designData.CreateDesign(artworkPath, "", 0.0M, 0.0M, 0.0M, 0.0M, 0.0M, 0.0M, 0.0M, 0.0M);

            bulkData.CreateBulkOrderDesign(orderId, designId);

            if(orderId > 0)
            {
                EmailFunctions emailFunc = new EmailFunctions();
                var emailSuccess = emailFunc.sendEmail(email, name, emailFunc.bulkOrderEmail(cartItems, orderTotal, orderId.ToString(), paymentGuid), "Lid Launch Order Confirmation", "");
                return orderId.ToString();
            } else
            {
                return "";
            }
            
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
            BulkData data = new BulkData();
            var batchId = data.CreateBulkOrderBatch();

            return batchId.ToString();
        }

        public ActionResult BulkOrderBatches()
        {
            BulkData data = new BulkData();
            List<OrderBatch> lstBatches = new List<OrderBatch>();

            lstBatches = data.GetBulkOrderBatches();

            return PartialView("BulkOrderBatches", lstBatches); ;
        }

        public ActionResult PrintBulkOrderBatchBulkOrders(string bulkBatchId)
        {
            BulkData data = new BulkData();
            BulkBatchOrder bulkBatchOrder = new BulkBatchOrder();
            List<BulkOrder> lstBulkOrders = new List<BulkOrder>();

            lstBulkOrders = data.GetBulkOrdersByBatchId(Convert.ToInt32(bulkBatchId));

            bulkBatchOrder.lstBulkOrders = lstBulkOrders;

            return View(bulkBatchOrder);
        }

        public ActionResult BulkOrderBatch(string bulkBatchId)
        {
            BulkData data = new BulkData();
            BulkBatchOrder bulkBatchOrder = new BulkBatchOrder();
            List<BulkOrder> lstBulkOrders = new List<BulkOrder>();

            lstBulkOrders = data.GetBulkOrdersByBatchId(Convert.ToInt32(bulkBatchId));

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

            bulkBatchOrder.lstItemsToOrder = products;

            bulkBatchOrder.lstItemsToOrder = bulkBatchOrder.lstItemsToOrder.OrderByDescending(bo => bo.ItemName).ToList();

            return View(bulkBatchOrder);
        }
    }
}