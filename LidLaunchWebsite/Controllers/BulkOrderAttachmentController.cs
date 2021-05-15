using LidLaunchWebsite.Classes;
using LidLaunchWebsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace LidLaunchWebsite.Controllers
{
    public class BulkOrderAttachmentController : Controller
    {
        // GET: BulkOrderAttachment
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ViewAttachments(string bulkOrderId)
        {
            BulkOrderAttachmentData data = new BulkOrderAttachmentData();
            List<BulkOrderAttachment> lstAttachments = data.GetBulkOrderAttachments(Convert.ToInt32(bulkOrderId));

            return PartialView(lstAttachments);
        }

        public ActionResult AttachmentEdit(string attachmentId, string bulkOrderId)
        {
            BulkOrderAttachment attachment = new BulkOrderAttachment();
            BulkOrderAttachmentData data = new BulkOrderAttachmentData();
            if (Convert.ToInt32(attachmentId) > 0)
            {                
                attachment = data.GetBulkOrderAttachment(Convert.ToInt32(attachmentId));
            } else
            {
                attachment.Id = 0;
                attachment.BulkOrderId = Convert.ToInt32(bulkOrderId);
            }
            return PartialView(attachment);
        }

        public string EditBulkDesignAttachment(string attachmentId, string bulkOrderId, string attachmentComment, string attachmentType)
        {
            BulkOrderAttachment attachment = new BulkOrderAttachment();
            BulkOrderAttachmentData data = new BulkOrderAttachmentData();
            BulkData bulkData = new BulkData();
            var success = false;

            if (!checkLoggedIn())
            {
                //do nothing
            }
            else
            {
                var attachmentSource = Request.Files["attachment"];
                if (attachmentSource != null && attachmentSource.ContentLength > 0)
                {
                    // get a stream
                    var stream = attachmentSource.InputStream;
                    // and optionally write the file to disk
                    var extension = System.IO.Path.GetExtension(attachmentSource.FileName);
                    var guidName = Guid.NewGuid().ToString();

                    attachment.AttachmentPath = "W" + bulkOrderId + "-" + guidName + "-" + Guid.NewGuid().ToString() + extension;
                    attachment.AttachmentName = attachmentSource.FileName;

                    var path = System.IO.Path.Combine(HttpRuntime.AppDomainAppPath + "/Images/BulkOrderAttachments/", attachment.AttachmentPath);

                    attachmentSource.SaveAs(path);
                }

                attachment.AttachmentComment = attachmentComment;
                attachment.AttachmentType = attachmentType;
                attachment.Deleted = false;
                attachment.BulkOrderId = Convert.ToInt32(bulkOrderId);

                if(Convert.ToInt32(attachmentId) > 0) {
                    success = data.UpdateBulkOrderAttachment(attachment);
                } else
                {
                    var newAttachmentId = data.CreateBulkOrderAttachment(attachment);

                    var addBulkOrderLogSuccess = bulkData.AddBulkOrderLog(Convert.ToInt32(bulkOrderId), Convert.ToInt32(Session["UserId"]), attachmentType + " Attachment Uploaded");

                    if (newAttachmentId > 0)
                    {
                        success = true;
                    }
                }                
            }

            var json = new JavaScriptSerializer().Serialize(success);
            return json;
        }

        public bool checkLoggedIn()
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