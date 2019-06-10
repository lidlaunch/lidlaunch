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
            return View();
        }

        public ActionResult Payment()
        {
            return View();
        }

        public string CreateBulkOrder(string items, string name, string email, string phone, string artworkPlacement, string orderNotes, string orderTotal)
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
            var orderId = bulkData.CreateBulkOrder(name, email, phone, Convert.ToDecimal(orderTotal), orderNotes, artworkPath, artworkPlacement, cartItems);

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
    }
}