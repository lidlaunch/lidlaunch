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
            var success = data.UpdateBulkOrderPaidByPaymentCompleteGuid(id);
            return View();
        }

        public string CreateBulkOrder(string items, string name, string email, string phone, string artworkPlacement, string orderNotes, string orderTotal, string paymentCompleteGuid)
        {
            var jss = new JavaScriptSerializer();
            var cartItems = jss.Deserialize<List<PaypalItem>>(items);
            var artworkPath = "";
            var fileContent = Request.Files[0];
            if (fileContent != null && fileContent.ContentLength > 0)
            {
                // get a stream
                var stream = fileContent.InputStream;
                // and optionally write the file to disk
                var extension = Path.GetExtension(fileContent.FileName);

                var fileName = Guid.NewGuid() + extension;
                artworkPath = fileName;
                var path = Path.Combine(Server.MapPath("~/Images/BulkOrderArtwork/"), fileName);

                fileContent.SaveAs(path);
            }

            BulkData bulkData = new BulkData();
            var orderId = bulkData.CreateBulkOrder(name, email, phone, Convert.ToDecimal(orderTotal), orderNotes, artworkPath, artworkPlacement, cartItems, paymentCompleteGuid);

            if(orderId > 0)
            {
                EmailFunctions emailFunc = new EmailFunctions();
                var emailSuccess = emailFunc.sendEmail(email, name, emailFunc.bulkOrderEmail(cartItems, orderTotal, orderId.ToString()), "Lid Launch Order Confirmation", "");
                return "success";
            } else
            {
                return "";
            }
            
        }

        public ActionResult ProcessPayment(string id)
        {
                BulkOrder bulkOrder = new BulkOrder();
                BulkData data = new BulkData();
                bulkOrder = data.GetBulkOrderByPaymentGuid(id);
                return View(bulkOrder);
        }
    }
}