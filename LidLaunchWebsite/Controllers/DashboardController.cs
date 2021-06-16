using LidLaunchWebsite.Classes;
using LidLaunchWebsite.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using ZXing;
using ZXing.Common;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System.Text;
using User = LidLaunchWebsite.Models.User;

namespace LidLaunchWebsite.Controllers
{
    public class DashboardController : Controller
    {
        // GET: Terms
        public ActionResult Designer()
        {
            if (Convert.ToInt32(Session["UserID"]) > 0)
            {
                if (Convert.ToInt32(Session["DesignerID"]) > 0)
                {
                    DashboardData data = new DashboardData();
                    DesignerDashboard dashboard = data.GetDesignerDashboard((Int32)Session["DesignerId"]);
                    Session["PayoutAmount"] = dashboard.TotalAvailableForPayout;
                    return View(dashboard);
                }
                else
                {
                    return RedirectToAction("Index", "Designer", null);
                }
            }
            else
            {
                return RedirectToAction("Login", "User", null);
            }
           
        }

        public ActionResult Profit()
        {
            if (Convert.ToInt32(Session["UserID"]) > 0)
            {
                if (Convert.ToInt32(Session["UserID"]) == 1)
                {
                    DashboardData data = new DashboardData();
                    AdminDashboard dashboard = data.GetAdminDashboard();

                    for (int i = dashboard.lstSales.Count - 1; i >= 0; i--)
                    {
                        // Do processing here, then...
                        if (dashboard.lstSales[i].HasPaid == false)
                        {
                            dashboard.lstSales.RemoveAt(i);
                        }
                    }

                    return View(dashboard);
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
        public ActionResult Orders()
        {
            if (checkAdminLoggedIn())
            {
                DashboardData data = new DashboardData();
                AdminDashboard dashboard = data.GetAdminDashboard();
                return View(dashboard);
            }
            else
            {
                return RedirectToAction("Login", "User", null);
            }

        }
        public ActionResult BatchOrder(string batchId)
        {
            if (checkAdminLoggedIn())
            {
                DashboardData data = new DashboardData();
                BatchDashboard dashboard = data.GetBatch(Convert.ToInt32(batchId));
                return View(dashboard);
            } else
            {
                return RedirectToAction("Login", "User", null);
            }
        }
        public ActionResult CreateBatch()
        {
            if (checkAdminLoggedIn())
            {
                DashboardData data = new DashboardData();
                BatchDashboard dashboard = data.GetBatches();
                return View(dashboard);
            }
            else
            {
                return RedirectToAction("Login", "User", null);
            }
        }
        public ActionResult PendingProducts()
        {
            if (checkAdminLoggedIn())
            {
                List<Product> lstProducts = new List<Product>();
                ProductData data = new ProductData();
                lstProducts = data.GetProductsToApprove();
                return View(lstProducts);
            }
            else
            {
                return RedirectToAction("Login", "User", null);
            }
        }

        public ActionResult NeedsDigitizing()
        {
            if (checkAdminLoggedIn())
            {
                List<Product> lstProducts = new List<Product>();
                ProductData data = new ProductData();
                lstProducts = data.GetArtworkForDigitizing();
                return View(lstProducts);
            }
            else
            {
                return RedirectToAction("Login", "User", null);
            }
        }
        public ActionResult HatManager()
        {
            if (checkAdminLoggedIn())
            {
                HatData data = new HatData();
                HatManager hatManager = new HatManager();
                hatManager.lstHatTypes = data.GetHatTypes();
                return View(hatManager);
            }
            else
            {
                return RedirectToAction("Login", "User", null);
            }
        }
        public ActionResult HatTypeEdit(int hatTypeId = 0)
        {
            if (checkAdminLoggedIn())
            {
                HatData data = new HatData();
                HatType hatType = new HatType();
                if (hatTypeId > 0)
                {
                    hatType = data.GetHatType(hatTypeId);
                }                    
                return View(hatType);
            }
            else
            {
                return RedirectToAction("Login", "User", null);
            }          
            
        }
        public string UploadHatCreationImage()
        {
            var returnValue = "";
            try
            {
                if (!checkLoggedIn())
                {
                    //do nothing
                }
                else
                {
                    var fileContent = Request.Files[0];
                    if (fileContent != null && fileContent.ContentLength > 0)
                    {
                        // get a stream
                        var stream = fileContent.InputStream;
                        // and optionally write the file to disk
                        //var extension = Path.GetExtension(fileContent.FileName);

                        var fileName = Request.Files[0].FileName;
                        var path = System.IO.Path.Combine(HttpRuntime.AppDomainAppPath + "/Images/HatAssets/", fileName);

                        fileContent.SaveAs(path);
                        returnValue = fileName;                        
                    }
                }
            }
            catch (Exception ex)
            {
                return "";
            }

            var json = new JavaScriptSerializer().Serialize(returnValue);
            return json;
        }
        public string UploadHatTypePreviewImage()
        {
            var returnValue = "";
            try
            {
                if (!checkLoggedIn())
                {
                    //do nothing
                }
                else
                {
                    var fileContent = Request.Files[0];
                    if (fileContent != null && fileContent.ContentLength > 0)
                    {
                        // get a stream
                        var stream = fileContent.InputStream;
                        // and optionally write the file to disk
                        //var extension = Path.GetExtension(fileContent.FileName);

                        var fileName = Request.Files[0].FileName;
                        var path = System.IO.Path.Combine(HttpRuntime.AppDomainAppPath + "/Images/HatAssets/", fileName);

                        fileContent.SaveAs(path);
                        returnValue = fileName;
                    }
                }
            }
            catch (Exception ex)
            {
                return "";
            }

            var json = new JavaScriptSerializer().Serialize(returnValue);
            return json;
        }
        
        public string SaveHatType (List<HatColor> hatTypeColors, int hatTypeId, string hatTypeName, string hatTypePreview, string description, string manufacturer, string productIdentifier, string basePrice)
        {
            HatData hatData = new HatData();
            HatType hatType = new HatType();
            hatType.Description = "test";
            hatType.ProductImage = hatTypePreview;
            hatType.ManufacturerId = 1;
            hatType.Name = hatTypeName;
            hatType.ProductIdentifier = "abc123";
            if (hatTypeId == 0)
            {
                //create new hat type from scratch                
                hatTypeId = hatData.CreateHatType(hatType);
                hatType.Id = hatTypeId;
            }
            else
            {
                //update existing hat type
                hatData.UpdateHatType(hatType);
                hatType.Id = hatTypeId;
            }

            foreach(HatColor hatTypeColor in hatTypeColors)
            {
                //update / create hat type color
                hatTypeColor.typeId = hatType.Id;
                hatData.CreateTypeColor(hatTypeColor);
            }

            return "";
        }
        public ActionResult EditProduct(string id)
        {
            if (checkAdminLoggedIn())
            {
                UpdateProduct updateProd = new UpdateProduct();
                List<Category> lstCategories = new List<Category>();
                List<Product> lstParentProducts = new List<Product>();
                ProductData data = new ProductData();
                Product product = new Product();
                product = data.GetProduct(Convert.ToInt32(id),0,0);
                lstCategories = data.GetCategories();
                lstParentProducts = data.GetDesignerProductsForParentList((Int32)Session["DesignerID"]);
                updateProd.lstParentProducts = lstParentProducts;

                updateProd.lstCategories = lstCategories;
                updateProd.Product = product;

                if (product.DesignerId == (Int32)Session["DesignerID"] || (Int32)Session["UserID"] == 1)
                {
                    Session["ProductID"] = Convert.ToInt32(id);
                    return View(updateProd);
                }
                else
                {
                    Session["ProductID"] = null;
                    return RedirectToAction("Login", "User", null);
                }                
            }
            else
            {
                Session["ProductID"] = null;
                return RedirectToAction("Index", "Home", null);
            }
        }

        public string CreateBatchOrder()
        {
            DashboardData dashboardData = new DashboardData();

            var batchId = dashboardData.CreateOrderBatch();

            var json = new JavaScriptSerializer().Serialize(batchId);

            return json;
        }

        public string RequestPayout()
        {
            DashboardData dashboardData = new DashboardData();
            DesignerData designerData = new DesignerData();
            Designer designer = designerData.GetDesigner((Int32)Session["UserID"]);

            var payoutId = dashboardData.CreateDesignerPayout(designer.Id, (decimal)Session["PayoutAmount"], designer.PaypalAddress);

            if (payoutId > 0)
            {                
                EmailFunctions email = new EmailFunctions();
                User user = new User();
                UserData userData = new UserData();
                user = userData.GetUser((Int32)Session["UserID"]);
                var success = email.sendEmail(user.Email, user.FirstName + ' ' + user.LastName, email.payoutEmail(designer.PaypalAddress, Session["PayoutAmount"].ToString()), "Your Payout Request Has Been Submitted", designer.PaypalAddress);
                Session["PayoutAmount"] = null;
            }

            var json = new JavaScriptSerializer().Serialize(payoutId);

            return json;
        }
        public string DenyProduct(string id, string denyReason)
        {
            bool success = false;
            if (Convert.ToInt32(Session["UserID"]) > 0)
            {
                if (checkLoggedIn())
                {
                    ProductData prodData = new ProductData();

                    success = prodData.DenyProduct(Convert.ToInt32(id));

                    if (success)
                    {
                        //sendEmail
                        ProductData data = new ProductData();
                        ProductAndDesignerInfo info = data.GetProductAndDesignerInfo(Convert.ToInt32(id));

                        if(info != null)
                        {
                            EmailFunctions email = new EmailFunctions();
                            email.sendEmail(info.UserEmail, "LidLaunch Designer", email.productDenyEmail(info.ProductName, denyReason, info.ArtSource), "Your LidLaunch Design Was Denied", "");
                        }
                    }
                }
                else
                {
                    success = false;
                }
            }
            else
            {
                success = false;
            }
            var json = new JavaScriptSerializer().Serialize(success);

            return json;

        }

        public string CreateBulkDesign(string designName, string bulkOrderId, string notifyCustomer)
        {
            bool success = false;
            DesignData designData = new DesignData();
            BulkData bulkData = new BulkData();
            var artworkFileName = "";
            var dstFileName = "";
            var pdfFileName = "";
            var previewFileName = "";
            var embFileName = "";

            if (!checkLoggedIn() && !checkDigitizer())
            {
                //do nothing
            }
            else
            {
                var artSource = Request.Files["artSource"];
                if (artSource != null && artSource.ContentLength > 0)
                {
                    // get a stream
                    var stream = artSource.InputStream;
                    // and optionally write the file to disk
                    var extension = System.IO.Path.GetExtension(artSource.FileName);

                    artworkFileName = "W" + bulkOrderId + "-" + designName + "-" + Guid.NewGuid().ToString() + extension;

                    var path = System.IO.Path.Combine(HttpRuntime.AppDomainAppPath + "/Images/BulkOrderArtwork/", artworkFileName);

                    artSource.SaveAs(path);
                }
                var dst = Request.Files["dst"];
                if (dst != null && dst.ContentLength > 0)
                {
                    // get a stream
                    var stream = dst.InputStream;
                    // and optionally write the file to disk
                    var extension = System.IO.Path.GetExtension(dst.FileName);

                    dstFileName = "W" + bulkOrderId + "-" + designName + "-" + Guid.NewGuid().ToString() + extension;

                    var path = System.IO.Path.Combine(HttpRuntime.AppDomainAppPath + "/Images/DesignImages/Digitizing/DST", dstFileName);

                    dst.SaveAs(path);
                }
                var pdf = Request.Files["pdf"];
                if (pdf != null && pdf.ContentLength > 0)
                {
                    // get a stream
                    var stream = pdf.InputStream;
                    // and optionally System.IO.Path the file to disk
                    var extension = System.IO.Path.GetExtension(pdf.FileName);

                    pdfFileName = "W" + bulkOrderId + "-" + designName + "-" + Guid.NewGuid().ToString() + extension;

                    var path = System.IO.Path.Combine(HttpRuntime.AppDomainAppPath + "/Images/DesignImages/Digitizing/Info", pdfFileName);

                    pdf.SaveAs(path);
                }
                var pngPreviewew = Request.Files["pngPreview"];
                if (pngPreviewew != null && pngPreviewew.ContentLength > 0)
                {
                    // get a stream
                    var stream = pngPreviewew.InputStream;
                    // and optionally write the file to disk
                    var extension = System.IO.Path.GetExtension(pngPreviewew.FileName);

                    previewFileName = "W" + bulkOrderId + "-" + designName + "-" + Guid.NewGuid().ToString() + extension;

                    var path = System.IO.Path.Combine(HttpRuntime.AppDomainAppPath + "/Images/DesignImages/Digitizing/Preview", previewFileName);

                    pngPreviewew.SaveAs(path);
                }
                var emb = Request.Files["emb"];
                if (emb != null && emb.ContentLength > 0)
                {
                    // get a stream
                    var stream = emb.InputStream;
                    // and optionally write the file to disk
                    var extension = System.IO.Path.GetExtension(emb.FileName);

                    embFileName = "W" + bulkOrderId + "-" + designName + "-" + Guid.NewGuid().ToString() + extension;

                    var path = System.IO.Path.Combine(HttpRuntime.AppDomainAppPath + "/Images/DesignImages/Digitizing/EMB", embFileName);

                    emb.SaveAs(path);
                }

                var designId = designData.CreateDesign(artworkFileName, "", 0.0M, 0.0M, 0.0M, 0.0M, 0.0M, 0.0M, 0.0M, 0.0M, designName, dstFileName, pdfFileName, embFileName, previewFileName);

                success = bulkData.CreateBulkOrderDesign(Convert.ToInt32(bulkOrderId), designId);

                var addBulkOrderLogSuccess = bulkData.AddBulkOrderLog(Convert.ToInt32(bulkOrderId), Convert.ToInt32(Session["UserId"]), "Design added to bulk order, Design Id: " + designId.ToString());

                if (success && Convert.ToBoolean(notifyCustomer))
                {
                    BulkOrder bulkOrder = bulkData.GetBulkOrder(Convert.ToInt32(bulkOrderId), "", "");
                    EmailFunctions email = new EmailFunctions();
                    email.sendEmail(bulkOrder.CustomerEmail, bulkOrder.CustomerName, email.digitizingPreviewUploaded(bulkOrder.PaymentGuid), "View & Approve Your Stitch Previews", "");
                }
            }        

            var json = new JavaScriptSerializer().Serialize(success);
            return json;
        }

        public string UpdateBulkDesign(string designName, string bulkOrderId, string designId, string notifyCustomer)
        {
            bool success = false;
            DesignData designData = new DesignData();
            BulkData bulkData = new BulkData();
            var artworkFileName = "";
            var dstFileName = "";
            var pdfFileName = "";
            var previewFileName = "";
            var embFileName = "";

            if (!checkLoggedIn() && !checkDigitizer())
            {
                //do nothing
            }
            else
            {
                var artSource = Request.Files["artSource"];
                if (artSource != null && artSource.ContentLength > 0)
                {
                    // get a stream
                    var stream = artSource.InputStream;
                    // and optionally write the file to disk
                    var extension = System.IO.Path.GetExtension(artSource.FileName);

                    artworkFileName = "W" + bulkOrderId + "-" + designName + "-" + Guid.NewGuid().ToString() + extension;

                    var path = System.IO.Path.Combine(HttpRuntime.AppDomainAppPath + "/Images/BulkOrderArtwork/", artworkFileName);

                    artSource.SaveAs(path);
                }
                var dst = Request.Files["dst"];
                if (dst != null && dst.ContentLength > 0)
                {
                    // get a stream
                    var stream = dst.InputStream;
                    // and optionally write the file to disk
                    var extension = System.IO.Path.GetExtension(dst.FileName);

                    dstFileName = "W" + bulkOrderId + "-" + designName + "-" + Guid.NewGuid().ToString() + extension;

                    var path = System.IO.Path.Combine(HttpRuntime.AppDomainAppPath + "/Images/DesignImages/Digitizing/DST", dstFileName);

                    dst.SaveAs(path);
                }
                var pdf = Request.Files["pdf"];
                //int stitchCount = 0;
                if (pdf != null && pdf.ContentLength > 0)
                {
                    // get a stream
                    var stream = pdf.InputStream;
                    // and optionally write the file to disk
                    var extension = System.IO.Path.GetExtension(pdf.FileName);

                    pdfFileName = "W" + bulkOrderId + "-" + designName + "-" + Guid.NewGuid().ToString() + extension;

                    var path = System.IO.Path.Combine(HttpRuntime.AppDomainAppPath + "/Images/DesignImages/Digitizing/Info", pdfFileName);

                    pdf.SaveAs(path);

                    ////parse pdf here
                    //using (PdfReader reader = new PdfReader(path))
                    //{
                    //    StringBuilder text = new StringBuilder();

                    //    for (int i = 1; i <= reader.NumberOfPages; i++)
                    //    {
                    //        text.Append(PdfTextExtractor.GetTextFromPage(reader, i));
                    //    }

                    //    stitchCount =  newText.Sub

                    //    var newText = text.ToString();
                    //    Console.WriteLine(newText);
                    //}

                }

                var pngPreviewew = Request.Files["pngPreview"];
                if (pngPreviewew != null && pngPreviewew.ContentLength > 0)
                {
                    // get a stream
                    var stream = pngPreviewew.InputStream;
                    // and optionally write the file to disk
                    var extension = System.IO.Path.GetExtension(pngPreviewew.FileName);

                    previewFileName = "W" + bulkOrderId + "-" + designName + "-" + Guid.NewGuid().ToString() + extension;

                    var path = System.IO.Path.Combine(HttpRuntime.AppDomainAppPath + "/Images/DesignImages/Digitizing/Preview", previewFileName);

                    pngPreviewew.SaveAs(path);
                }
                var emb = Request.Files["emb"];
                if (emb != null && emb.ContentLength > 0)
                {
                    // get a stream
                    var stream = emb.InputStream;
                    // and optionally write the file to disk
                    var extension = System.IO.Path.GetExtension(emb.FileName);

                    embFileName = "W" + bulkOrderId + "-" + designName + "-" + Guid.NewGuid().ToString() + extension;

                    var path = System.IO.Path.Combine(HttpRuntime.AppDomainAppPath + "/Images/DesignImages/Digitizing/EMB", embFileName);

                    emb.SaveAs(path);
                }

                success = designData.UpdateDesign(artworkFileName, "", 0.0M, 0.0M, 0.0M, 0.0M, 0.0M, 0.0M, 0.0M, 0.0M, designName, dstFileName, pdfFileName, embFileName, previewFileName, Convert.ToInt32(designId));

                if(success)
                {
                    var design = designData.GetDesign(Convert.ToInt32(designId));

                    if(design.Revision)
                    {
                        if (Convert.ToBoolean(notifyCustomer))
                        {
                            designData.UpdateDesignRevisionStatus(design.Id, "3:AwaitingCustomerApproval");
                        }
                        else
                        {
                            designData.UpdateDesignRevisionStatus(design.Id, "2:RevisionChangesDone");
                        }
                    }
                }

                var addBulkOrderLogSuccess = bulkData.AddBulkOrderLog(Convert.ToInt32(bulkOrderId), Convert.ToInt32(Session["UserId"]), "Design has been Updated, Design Id: " + designId.ToString() + (Convert.ToBoolean(notifyCustomer) ? " :: Customer Has been emailed link to approve." : ""));

                if (success && Convert.ToBoolean(notifyCustomer))
                {
                    BulkOrder bulkOrder = bulkData.GetBulkOrder(Convert.ToInt32(bulkOrderId), "", "");
                    EmailFunctions email = new EmailFunctions();
                    email.sendEmail(bulkOrder.CustomerEmail, bulkOrder.CustomerName, email.digitizingPreviewUploaded(bulkOrder.PaymentGuid), "View & Approve Your Stitch Previews", "");
                }
            }

            var json = new JavaScriptSerializer().Serialize(success);
            return json;
        }

        public string ContactBulkCustomer(string bulkOrderId, string emailType, string emailText)
        {
            try
            {
                if (checkLoggedIn())
                {
                    BulkData bulkData = new BulkData();
                    BulkOrder bulkOrder = bulkData.GetBulkOrder(Convert.ToInt32(bulkOrderId), "", "");
                    EmailFunctions email = new EmailFunctions();
                    //send email to customer about their order, revision request
                    //var success = email.sendEmail(bulkOrder.CustomerEmail, bulkOrder.CustomerName, email.digitizingPreviewUploaded(bulkOrder.PaymentGuid), "View & Approve Your Stitch Previews", "");
                    var success = true;

                    var addBulkOrderLogSuccess = bulkData.AddBulkOrderLog(Convert.ToInt32(bulkOrderId), Convert.ToInt32(Session["UserId"]), "Email Sent to Customer Website: " + emailText);

                    var json = new JavaScriptSerializer().Serialize(success);

                    return json;
                }
                else
                {
                    return new JavaScriptSerializer().Serialize(false);
                }                
            }
            catch (Exception ex)
            {
                return new JavaScriptSerializer().Serialize(false);
            }            
        }

        public string ApproveProduct(string id)
        {
            bool success = false;
            if (Convert.ToInt32(Session["UserID"]) > 0)
            {
                if (checkLoggedIn())
                {
                    ProductData prodData = new ProductData();                    

                    success = prodData.ApproveProduct(Convert.ToInt32(id));

                    if (success)
                    {
                        ProductData data = new ProductData();
                        ProductAndDesignerInfo info = data.GetProductAndDesignerInfo(Convert.ToInt32(id));
                        if (info != null)
                        {
                            EmailFunctions email = new EmailFunctions();
                            email.sendEmail(info.UserEmail, "LidLaunch Designer", email.productApprovedEmail(info.ProductName, "https://lidlaunch.com/Product?id=" + id), "Your LidLaunch Design Was Approved", "");
                        }
                    }
                }
                else
                {
                    success = false;
                }
            }
            else
            {
                success = false;
            }
            var json = new JavaScriptSerializer().Serialize(success);

            return json;

        }
        public bool checkDigitizer()
        {
            if(Convert.ToString(Session["UserEmail"]) == ConfigurationManager.AppSettings["DigitizerEmailAddress"])
            {
                return true;
            } 
            else
            {
                return false;
            }
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
        public string UploadDigitizedFile(string designId, string bulkOrderId)
        {
            var returnValue = "";
            try
            {
                if (!checkLoggedIn() && !checkDigitizer())
                {
                    //do nothing
                }
                else
                {
                    var fileContent = Request.Files[0];
                    if (fileContent != null && fileContent.ContentLength > 0)
                    {
                        // get a stream
                        var stream = fileContent.InputStream;
                        // and optionally write the file to disk
                        var extension = System.IO.Path.GetExtension(fileContent.FileName);
                        
                        var fileName = Guid.NewGuid().ToString() + extension;
                        if(bulkOrderId != "")
                        {
                            fileName = "W" + bulkOrderId + extension;
                        }
                        var path = System.IO.Path.Combine(HttpRuntime.AppDomainAppPath + "/Images/DesignImages/Digitizing/DST", fileName);

                        fileContent.SaveAs(path);                                
                        returnValue = fileName;

                        //update design digitized file in database
                        DesignData data = new DesignData();
                        var success = data.UpdateDesignDigitizedFile(Convert.ToInt32(designId), fileName);
                    }
                }
            }
            catch (Exception ex)
            {
                return "";
            }

            var json = new JavaScriptSerializer().Serialize(returnValue);
            return json;
        }
        public string UpdateDesignDigitizedPreview(string designId, string bulkOrderId)
        {
            var returnValue = "";
            try
            {
                if (!checkLoggedIn() && !checkDigitizer())
                {
                    //do nothing
                }
                else
                {
                    var fileContent = Request.Files[0];
                    if (fileContent != null && fileContent.ContentLength > 0)
                    {
                        // get a stream
                        var stream = fileContent.InputStream;
                        // and optionally write the file to disk
                        var extension = System.IO.Path.GetExtension(fileContent.FileName);

                        var fileName = Guid.NewGuid().ToString() + extension;
                        if (bulkOrderId != "")
                        {
                            fileName = "W" + bulkOrderId + extension;
                        }
                        var path = System.IO.Path.Combine(HttpRuntime.AppDomainAppPath + "/Images/DesignImages/Digitizing/Preview", fileName);

                        fileContent.SaveAs(path);
                        returnValue = fileName;

                        //update design digitized file in database
                        DesignData data = new DesignData();
                        var success = data.UpdateDesignDigitizedPreview(Convert.ToInt32(designId), fileName);

                        if (success)
                        {
                            BulkData bulkData = new BulkData();
                            BulkOrder bulkOrder = bulkData.GetBulkOrder(Convert.ToInt32(bulkOrderId), "", "");
                            if(bulkOrder.lstDesigns.Find(d => d.Id == Convert.ToInt32(designId)).InternallyApproved && !bulkOrder.lstDesigns.Find(d => d.Id == Convert.ToInt32(designId)).CustomerApproved && !bulkOrder.OrderComplete)
                            {
                                EmailFunctions email = new EmailFunctions();
                                email.sendEmail(bulkOrder.CustomerEmail, bulkOrder.CustomerName, email.digitizingPreviewUploaded(bulkOrder.PaymentGuid), "View & Approve Your Stitch Previews", "");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return "";
            }

            var json = new JavaScriptSerializer().Serialize(returnValue);
            return json;
        }

        public string UpdateDesignEMBFile(string designId, string bulkOrderId)
        {
            var returnValue = "";
            try
            {
                if (!checkLoggedIn() && !checkDigitizer())
                {
                    //do nothing
                }
                else
                {
                    var fileContent = Request.Files[0];
                    if (fileContent != null && fileContent.ContentLength > 0)
                    {
                        // get a stream
                        var stream = fileContent.InputStream;
                        // and optionally write the file to disk
                        var extension = System.IO.Path.GetExtension(fileContent.FileName);

                        var fileName = Guid.NewGuid().ToString() + extension;
                        if (bulkOrderId != "")
                        {
                            fileName = "W" + bulkOrderId + extension;
                        }
                        var path = System.IO.Path.Combine(HttpRuntime.AppDomainAppPath + "/Images/DesignImages/Digitizing/EMB", fileName);

                        fileContent.SaveAs(path);
                        returnValue = fileName;

                        //update design digitized file in database
                        DesignData data = new DesignData();
                        var success = data.UpdateDesignEMBFile(Convert.ToInt32(designId), fileName);
                    }
                }
            }
            catch (Exception ex)
            {
                return "";
            }

            var json = new JavaScriptSerializer().Serialize(returnValue);
            return json;
        }
        public string UpdateDesignDigitizedProductionSheet(string designId, string bulkOrderId)
        {
            var returnValue = "";
            try
            {
                if (!checkLoggedIn() && !checkDigitizer())
                {
                    //do nothing
                }
                else
                {
                    var fileContent = Request.Files[0];
                    if (fileContent != null && fileContent.ContentLength > 0)
                    {
                        // get a stream
                        var stream = fileContent.InputStream;
                        // and optionally write the file to disk
                        var extension = System.IO.Path.GetExtension(fileContent.FileName);

                        var fileName = Guid.NewGuid().ToString() + extension;
                        if (bulkOrderId != "")
                        {
                            fileName = "W" + bulkOrderId + extension;
                        }
                        var path = System.IO.Path.Combine(HttpRuntime.AppDomainAppPath + "/Images/DesignImages/Digitizing/Info", fileName);

                        fileContent.SaveAs(path);
                        returnValue = fileName;

                        //update design digitized file in database
                        DesignData data = new DesignData();
                        var success = data.UpdateDesignDigitizedProductionSheet(Convert.ToInt32(designId), fileName);
                    }
                }
            }
            catch (Exception ex)
            {
                return "";
            }

            var json = new JavaScriptSerializer().Serialize(returnValue);
            return json;
        }

        public string SendArtworkEmail(string bulkOrderId, string customerEmail)
        {
            BulkData bulkData = new BulkData();
            EmailFunctions email = new EmailFunctions();

            var success = email.sendEmail(customerEmail, "LidLaunch Customer", email.requestArtworkEmail(bulkOrderId), "Order #" + bulkOrderId + " Artwork Request", ""); 
            if (success)
            {
                bulkData.UpdateArtworkEmailSent(Convert.ToInt32(bulkOrderId));
                var addBulkOrderLogSuccess = bulkData.AddBulkOrderLog(Convert.ToInt32(bulkOrderId), Convert.ToInt32(Session["UserId"]), "Missing Artwork Email Sent");
            }            

            var json = new JavaScriptSerializer().Serialize(success);

            return json;
        }

        public string SendColorConfirmationEmail(string bulkOrderId, string customerEmail, string noteText)
        {
            BulkData bulkData = new BulkData();
            EmailFunctions email = new EmailFunctions();

            var success = email.sendEmail(customerEmail, "LidLaunch Customer", email.colorConfirmationEmail(bulkOrderId, noteText), "Order #" + bulkOrderId + " Color Confirmation Request", "");
            if (success)
            {
                bulkData.UpdateColorConfirmationEmailSent(Convert.ToInt32(bulkOrderId));
                var addBulkOrderLogSuccess = bulkData.AddBulkOrderLog(Convert.ToInt32(bulkOrderId), Convert.ToInt32(Session["UserId"]), "Color Confirmation Email Sent: " + noteText);
            }

            var json = new JavaScriptSerializer().Serialize(success);

            return json;
        }

        public string SendGeneralEmail(string bulkOrderId, string customerEmail, string emailText, string paymentGuid)
        {
            BulkData bulkData = new BulkData();
            EmailFunctions email = new EmailFunctions();

            var success = email.sendEmail(customerEmail, "LidLaunch Customer", email.generalBulkOrderEmail(emailText, paymentGuid), "Your Order #" + bulkOrderId, "");
            if (success)
            {
                var addBulkOrderLogSuccess = bulkData.AddBulkOrderLog(Convert.ToInt32(bulkOrderId), Convert.ToInt32(Session["UserId"]), "Email Sent To Customer From Website: " + emailText);
            }

            var json = new JavaScriptSerializer().Serialize(success);

            return json;
        }


        public string UpdateTracking(string orderProductId, string trackingNumber, string customerEmail)
        {
            DashboardData dashboardData = new DashboardData();

            var success = dashboardData.UpdateTracking(Convert.ToInt32(orderProductId), trackingNumber);

            if (success)
            {
                EmailFunctions email = new EmailFunctions();
                email.sendEmail(customerEmail, "LidLaunch Customer", email.orderShippedEmail(trackingNumber), "Your Order Is On It's Way!", "");
            }

            var json = new JavaScriptSerializer().Serialize(success);

            return json;
        }

        public ActionResult BulkOrderPriority(string filter)
        {

            if (!checkLoggedIn())
            {
                return RedirectToAction("Index", "Home", null);
            }
            else
            {
                BulkData data = new BulkData();
                ViewBulkOrdersModel model = new ViewBulkOrdersModel();
                model.lstBulkOrders = data.GetBulkOrderData(filter);
                model.lstBulkOrders = model.lstBulkOrders.OrderByDescending(bo => bo.OrderPaid).ToList();
                model.lstBulkOrderBatches = data.GetBulkOrderBatches();
                return View(model);
            }
        }

        public ActionResult ViewBulkOrders(string filter)
        {

            if (!checkLoggedIn())
            {
                return RedirectToAction("Index", "Home", null);
            }
            else
            {                
                BulkData data = new BulkData();
                ViewBulkOrdersModel model = new ViewBulkOrdersModel();
                model.lstBulkOrders = data.GetBulkOrderData(filter);
                model.lstBulkOrders = model.lstBulkOrders.OrderByDescending(bo => bo.OrderPaid).ToList();
                //model.lstBulkOrderBatches = data.GetBulkOrderBatches();

                BulkOrderListSectionModel allSection = new BulkOrderListSectionModel();
                allSection.Name = "All Orders";
                allSection.SortOrder = 0;
                allSection.lstBulkOrders = new List<BulkOrder>();
                allSection.lstBulkOrders = model.lstBulkOrders;
                allSection.textColor = "#000";
                allSection.backgroundColor = "#fff";

                BulkOrderListSectionModel adminReviewSection = new BulkOrderListSectionModel();
                adminReviewSection.Name = "Admin Review";
                adminReviewSection.SortOrder = 1;
                adminReviewSection.lstBulkOrders = new List<BulkOrder>();
                adminReviewSection.lstBulkOrders.AddRange(model.lstBulkOrders.Where(bo => bo.AdminReview && !bo.DesignerReview));
                adminReviewSection.lstBulkOrders = adminReviewSection.lstBulkOrders.OrderBy(bo => bo.PaymentDate).ToList();
                adminReviewSection.textColor = "#fff";
                adminReviewSection.backgroundColor = "#000";

                BulkOrderListSectionModel designerReviewSection = new BulkOrderListSectionModel();
                designerReviewSection.Name = "Designer Review";
                designerReviewSection.SortOrder = 2;
                designerReviewSection.lstBulkOrders = new List<BulkOrder>();
                designerReviewSection.lstBulkOrders.AddRange(model.lstBulkOrders.Where(bo => bo.DesignerReview));
                designerReviewSection.lstBulkOrders = designerReviewSection.lstBulkOrders.OrderBy(bo => bo.PaymentDate).ToList();
                designerReviewSection.textColor = "#fff";
                designerReviewSection.backgroundColor = "#000";

                BulkOrderListSectionModel noArtworkSection = new BulkOrderListSectionModel();
                noArtworkSection.Name = "No Artwork";                
                noArtworkSection.SortOrder = 3;
                noArtworkSection.lstBulkOrders = new List<BulkOrder>();
                noArtworkSection.lstBulkOrders.AddRange(model.lstBulkOrders.Where(bo => !bo.OrderComplete && !bo.ReadyForProduction && (bo.ArtworkImage == "" || (!bo.lstDesigns.Any(d => d.DigitizedPreview != "") && bo.OrderNotes.Contains("ARTWORK PRE-EXISTING")))));
                noArtworkSection.lstBulkOrders = noArtworkSection.lstBulkOrders.OrderBy(bo => bo.PaymentDate).ToList();
                noArtworkSection.textColor = "#000";
                noArtworkSection.backgroundColor = "#ccc";

                BulkOrderListSectionModel inRevisionSection = new BulkOrderListSectionModel();
                inRevisionSection.Name = "In Revision";
                inRevisionSection.SortOrder = 4;
                inRevisionSection.lstBulkOrders = new List<BulkOrder>();
                inRevisionSection.lstBulkOrders.AddRange(model.lstBulkOrders.Where(bo => !bo.OrderComplete && !bo.ReadyForProduction && bo.lstDesigns.Any(d => d.CustomerApproved == false && d.InternallyApproved) && bo.lstDesigns.Any(d => d.Revision && d.lstRevisionNotes.Any(rn => rn.CustomerAdded))));
                inRevisionSection.lstBulkOrders = inRevisionSection.lstBulkOrders.OrderBy(bo => bo.PaymentDate).ToList().OrderBy(bo => bo.lstDesigns.Max(d => d.RevisionStatus)).ToList(); ;
                inRevisionSection.textColor = "#fff";
                inRevisionSection.backgroundColor = "#8c4b00";


                BulkOrderListSectionModel awaitingInternalApproval = new BulkOrderListSectionModel();
                awaitingInternalApproval.Name = "Awaiting Internal Approval";
                awaitingInternalApproval.SortOrder = 5;
                awaitingInternalApproval.lstBulkOrders = new List<BulkOrder>();
                awaitingInternalApproval.lstBulkOrders.AddRange(model.lstBulkOrders.Where(bo => !bo.OrderComplete && !bo.ReadyForProduction && bo.lstDesigns.Any(d => d.DigitizedPreview != "") && bo.lstDesigns.Where(d => d.InternallyApproved).ToList().Count == 0));
                awaitingInternalApproval.lstBulkOrders = awaitingInternalApproval.lstBulkOrders.OrderBy(bo => bo.PaymentDate).ToList();
                awaitingInternalApproval.textColor = "#000";
                awaitingInternalApproval.backgroundColor = "#f9fc04";


                BulkOrderListSectionModel awaitingCustomerApproval = new BulkOrderListSectionModel();
                awaitingCustomerApproval.Name = "Awaiting Customer Approval";
                awaitingCustomerApproval.SortOrder = 6;
                awaitingCustomerApproval.lstBulkOrders = new List<BulkOrder>();
                awaitingCustomerApproval.lstBulkOrders.AddRange(model.lstBulkOrders.Where(bo => !bo.OrderComplete && !bo.ReadyForProduction && bo.lstDesigns.Any(d => d.CustomerApproved == false) && bo.lstDesigns.Any(d => d.InternallyApproved) && !bo.lstDesigns.Any(d => d.Revision) && bo.lstDesigns.Any(d => d.DigitizedPreview != "")));
                awaitingCustomerApproval.lstBulkOrders = awaitingCustomerApproval.lstBulkOrders.OrderBy(bo => bo.PaymentDate).ToList();
                awaitingCustomerApproval.textColor = "#fff";
                awaitingCustomerApproval.backgroundColor = "#fc8a04";

                BulkOrderListSectionModel awaitingProductionReview = new BulkOrderListSectionModel();
                awaitingProductionReview.Name = "Awaiting Production Review";
                awaitingProductionReview.SortOrder = 7;
                awaitingProductionReview.lstBulkOrders = new List<BulkOrder>();
                awaitingProductionReview.lstBulkOrders.AddRange(model.lstBulkOrders.Where(bo => !bo.OrderComplete && !bo.ReadyForProduction && bo.lstDesigns.Any(d => d.CustomerApproved == true))); adminReviewSection.lstBulkOrders.OrderByDescending(bo => bo.PaymentDate);
                awaitingProductionReview.lstBulkOrders = awaitingProductionReview.lstBulkOrders.OrderBy(bo => bo.PaymentDate).ToList();
                awaitingProductionReview.textColor = "#fff";
                awaitingProductionReview.backgroundColor = "#c504fc";

                BulkOrderListSectionModel readyForProduction = new BulkOrderListSectionModel();
                readyForProduction.Name = "Ready For Production";
                readyForProduction.SortOrder = 8;
                readyForProduction.lstBulkOrders = new List<BulkOrder>();
                readyForProduction.lstBulkOrders.AddRange(model.lstBulkOrders.Where(bo => !bo.OrderComplete && bo.ReadyForProduction));
                readyForProduction.lstBulkOrders = readyForProduction.lstBulkOrders.OrderBy(bo => bo.PaymentDate).ToList();
                readyForProduction.textColor = "#fff";
                readyForProduction.backgroundColor = "#1372d3";

                BulkOrderListSectionModel reworkSection = new BulkOrderListSectionModel();
                reworkSection.Name = "Rework";
                reworkSection.SortOrder = 9;
                reworkSection.lstBulkOrders = new List<BulkOrder>();
                reworkSection.lstBulkOrders.AddRange(model.lstBulkOrders.Where(bo => bo.lstItems.Any(i => i.BulkRework.Status == "In Progress")));
                reworkSection.lstBulkOrders = reworkSection.lstBulkOrders.OrderBy(bo => bo.PaymentDate).ToList();
                reworkSection.textColor = "#fff";
                reworkSection.backgroundColor = "#e92121";

                //BulkOrderListSectionModel completeSection = new BulkOrderListSectionModel();
                //completeSection.Name = "Completed";
                //completeSection.SortOrder = 10;
                //completeSection.lstBulkOrders = new List<BulkOrder>();
                //completeSection.lstBulkOrders.AddRange(model.lstBulkOrders.Where(bo => !bo.lstItems.Any(i => i.BulkRework.Status == "In Progress") && bo.OrderComplete));
                //completeSection.textColor = "#fff";
                //completeSection.backgroundColor = "#39b91c";

                model.lstSections = new List<BulkOrderListSectionModel>();
                model.lstSections.Add(allSection);
                model.lstSections.Add(adminReviewSection);
                model.lstSections.Add(designerReviewSection);
                model.lstSections.Add(noArtworkSection);
                model.lstSections.Add(inRevisionSection);
                model.lstSections.Add(awaitingInternalApproval);
                model.lstSections.Add(awaitingCustomerApproval);
                model.lstSections.Add(awaitingProductionReview);
                model.lstSections.Add(readyForProduction);
                model.lstSections.Add(reworkSection);
                //model.lstSections.Add(completeSection);

                return View(model);
            }
        }

        public ActionResult ShipBulkOrders(string filter)
        {
            if (!checkLoggedIn())
            {
                return RedirectToAction("Index", "Home", null);
            }
            else
            {
                BulkData data = new BulkData();
                ViewBulkOrdersModel model = new ViewBulkOrdersModel();
                model.lstBulkOrders = data.GetBulkOrderData(filter);
                model.lstBulkOrders = model.lstBulkOrders.OrderBy(bo => Convert.ToDateTime(bo.ProjectedShipDateLong)).ToList().OrderBy(bo => bo.OrderComplete).ToList();

                return View(model);
            }
        }

        public ActionResult MachinePanel (string bulkOrderId)
        {

            if (!checkLoggedIn())
            {
                return RedirectToAction("Index", "Home", null);
            }
            else
            {
                BulkData data = new BulkData();
                BulkOrder model = new BulkOrder();
                if(bulkOrderId != "0")
                {
                    model = data.GetBulkOrder(Convert.ToInt32(bulkOrderId.Replace("BO-","")), "", "");
                } else
                {
                    model.lstItems = new List<BulkOrderItem>();
                    model.lstDesigns = new List<Design>();
                    model.lstNotes = new List<Note>();                    
                }
                          
                return View(model);
            }
        }

        public ActionResult BulkOrderDetailsLookup(string bulkOrderId)
        {

            if (!checkLoggedIn())
            {
                return RedirectToAction("Index", "Home", null);
            }
            else
            {
                BulkData data = new BulkData();
                BulkOrder model = new BulkOrder();
                if (bulkOrderId != "0")
                {
                    model = data.GetBulkOrder(Convert.ToInt32(bulkOrderId.Replace("BO-", "")), "", "");
                }
                else
                {
                    model.lstItems = new List<BulkOrderItem>();
                    model.lstDesigns = new List<Design>();
                    model.lstNotes = new List<Note>();
                }

                return View(model);
            }
        }

        public string UpdateBulkOrderPaid(string bulkOrderId, string orderPaid)
        {

            if (Convert.ToInt32(Session["UserID"]) != 1)
            {
                return "false";
            } else
            {
                BulkData data = new BulkData();
                bool orderHasBeenPaid = Convert.ToBoolean(orderPaid);

                if (orderHasBeenPaid)
                {
                    data.UpdateBulkOrderPaid(Convert.ToInt32(bulkOrderId), false);
                    var addBulkOrderLogSuccess = data.AddBulkOrderLog(Convert.ToInt32(bulkOrderId), Convert.ToInt32(Session["UserId"]), "Bulk Order Marked UNPAID");
                    return "true";
                }
                else
                {
                    data.UpdateBulkOrderPaid(Convert.ToInt32(bulkOrderId), true);
                    var addBulkOrderLogSuccess = data.AddBulkOrderLog(Convert.ToInt32(bulkOrderId), Convert.ToInt32(Session["UserId"]), "Bulk Order Marked PAID");
                    return "true";
                }
            }            

        }

        public string UpdateBulkOrderRefunded(string bulkOrderId)
        {

            if (Convert.ToInt32(Session["UserID"]) != 1)
            {
                return "false";
            }
            else
            {
                BulkData data = new BulkData();
                var success = data.UpdateBulkOrderRefunded(Convert.ToInt32(bulkOrderId));
                if (success)
                {
                    var addBulkOrderLogSuccess = data.AddBulkOrderLog(Convert.ToInt32(bulkOrderId), Convert.ToInt32(Session["UserId"]), "BULK ORDER REFUNDED");
                }

                var json = new JavaScriptSerializer().Serialize(success);

                return json;
            }

        }

        public string SendDesignApprovalEmail(string bulkOrderId)
        {

            if (!checkLoggedIn())
            {
                return new JavaScriptSerializer().Serialize(false);
            }
            else
            {
                BulkData data = new BulkData();

                BulkOrder bulkOrder = data.GetBulkOrder(Convert.ToInt32(bulkOrderId), "", "");
                EmailFunctions email = new EmailFunctions();
                var success = email.sendEmail(bulkOrder.CustomerEmail, bulkOrder.CustomerName, email.digitizingPreviewUploaded(bulkOrder.PaymentGuid), "View & Approve Your Stitch Previews", "");

                if (success)
                {
                    data.UpdateBulkOrderReminderApprovalSent(Convert.ToInt32(bulkOrderId));
                    var addBulkOrderLogSuccess = data.AddBulkOrderLog(Convert.ToInt32(bulkOrderId), Convert.ToInt32(Session["UserId"]), "Design Approval Reminder Email Sent Number " + (bulkOrder.ReminderApprovalEmailSent + 1).ToString() + ".");

                    if (bulkOrder.lstDesigns.Any(d => d.Revision))
                    {
                        DesignData designData = new DesignData();
                        designData.UpdateDesignRevisionStatus(bulkOrder.lstDesigns.First(d => d.Revision).Id, "3:AwaitingCustomerApproval");
                    }
                }

                var json = new JavaScriptSerializer().Serialize(success);

                return json;
            }

        }

        public string UpdateBulkOrderBatchId(string bulkOrderId, string batchId)
        {
            BulkData data = new BulkData();
            data.UpdateBulkOrderBatchId(Convert.ToInt32(bulkOrderId), Convert.ToInt32(batchId));
            var addBulkOrderLogSuccess = data.AddBulkOrderLog(Convert.ToInt32(bulkOrderId), Convert.ToInt32(Session["UserId"]), "Bulk Order Added to Batch ID " + batchId);
            return "true";
        }

        public string ReleaseToDigitizer(string bulkOrderId)
        {
            if (Convert.ToInt32(Session["UserID"]) == 1)
            {
                BulkData data = new BulkData();
                data.ReleaseToDigitizer(Convert.ToInt32(bulkOrderId));
                var addBulkOrderLogSuccess = data.AddBulkOrderLog(Convert.ToInt32(bulkOrderId), Convert.ToInt32(Session["UserId"]), "Customer Supplied Design Released to Digitizer");
                return "true";
            }
            else
            {
                return "false";
            }            
        }


        public ActionResult BulkDigitizing()
        {
            if (Convert.ToInt32(Session["UserID"]) > 0)
            {
                if (checkLoggedIn() || checkDigitizer())
                {
                    List<BulkOrder> lstBulkOrders = new List<BulkOrder>();
                    BulkData data = new BulkData();
                    lstBulkOrders = data.GetBulkOrderData("");
                    lstBulkOrders.RemoveAll(bo => !bo.OrderPaid);
                    lstBulkOrders.RemoveAll(bo => !bo.ReleaseToDigitizer);
                    lstBulkOrders.RemoveAll(bo => bo.OrderComplete);
                    return View(lstBulkOrders);
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

        public ActionResult ShipBulkOrderPopup(int bulkOrderId)
        {

            if (Convert.ToInt32(Session["UserID"]) > 0)
            {
                if (checkLoggedIn())
                {
                    BulkOrder bulkOrder = new BulkOrder();
                    BulkData data = new BulkData();
                    bulkOrder = data.GetBulkOrder(bulkOrderId, "", "");

                    bulkOrder.BarcodeImage = "BO-" + bulkOrderId.ToString() + ".jpg";


                    return PartialView("ShipBulkOrderPopup", bulkOrder);
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

        public ActionResult BulkOrderDetailsPopup(int bulkOrderId)
        {
             
            if (Convert.ToInt32(Session["UserID"]) > 0)
            {
                if (checkLoggedIn())
                {
                    BulkOrderDetailsPopupModel model = new BulkOrderDetailsPopupModel();
                    BulkOrder bulkOrder = new BulkOrder();
                    BulkData data = new BulkData();
                    BulkOrderAttachmentData attachmentData = new BulkOrderAttachmentData();
                    bulkOrder = data.GetBulkOrder(bulkOrderId, "", "");

                    bulkOrder.BarcodeImage = "BO-" + bulkOrderId.ToString() + ".jpg";
                    if(!System.IO.File.Exists(HttpRuntime.AppDomainAppPath + "/Images/Barcodes/" + bulkOrder.BarcodeImage))
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
                        Bitmap barcode = barcodeWriter.Write("BO-" + bulkOrderId.ToString());
                        barcode.Save(HttpRuntime.AppDomainAppPath + "/Images/Barcodes/" + bulkOrder.BarcodeImage);
                    } else
                    {
                        //do nothing
                    }

                    model.BulkOrder = bulkOrder;
                    model.lstBulkOrderBatches = data.GetBulkOrderBatches();                    
                    model.BulkOrder.lstAttachments = attachmentData.GetBulkOrderAttachments(bulkOrderId);

                    //model.lstPreviousBulkOrders = data.GetPreviousBulkOrders(bulkOrderId);


                    return PartialView("BulkOrderDetailsPopup", model);
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

        public ActionResult EditBulkOrderPopup(int bulkOrderId)
        {

            if (Convert.ToInt32(Session["UserID"]) > 0)
            {
                if (checkAdminLoggedIn())
                {
                    BulkOrder bulkOrder = new BulkOrder();
                    BulkData data = new BulkData();
                    bulkOrder = data.GetBulkOrder(bulkOrderId, "", "");

                    return PartialView("EditBulkOrderPopup", bulkOrder);
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

        public string UpdateBulkOrder(string items, string customerEmail, string artworkPosition, string bulkOrderId, string orderTotal, string shipToAddress, string shipToCity, string shipToState, string shipToZip)
        {
            List<BulkOrderItem> lstItems = new List<BulkOrderItem>();
            lstItems = Newtonsoft.Json.JsonConvert.DeserializeObject<List<BulkOrderItem>>(items);
            BulkData data = new BulkData();
            BulkOrder bulkOrder = data.GetBulkOrder(Convert.ToInt32(bulkOrderId), "", "");
            foreach(BulkOrderItem item in lstItems)
            {
                if(item.Id == 0)
                {
                    data.AddBulkOrderItem(Convert.ToInt32(bulkOrderId), item.ItemName, Convert.ToInt32(item.ItemQuantity), Convert.ToDecimal(item.ItemCost));
                    var addBulkOrderLogSuccess = data.AddBulkOrderLog(Convert.ToInt32(bulkOrderId), Convert.ToInt32(Session["UserId"]), "Order Item has added: NEW VALUES: ItemName:" + item.ItemName + " : ItemQuantity:" + item.ItemQuantity + " : ItemCost:" + item.ItemCost);
                } else
                {
                    var bulkOrderItem = bulkOrder.lstItems.Find(i => i.Id == item.Id);
                    if(bulkOrderItem != null)
                    {
                        if(bulkOrderItem.ItemName == item.ItemName && bulkOrderItem.ItemQuantity == Convert.ToInt32(item.ItemQuantity) && bulkOrderItem.ItemCost == Convert.ToDecimal(item.ItemCost))
                        {
                            //nothing updated dont call database
                        } else
                        {
                            //an item has been updated..
                            data.UpdateBulkOrderItem(Convert.ToInt32(bulkOrderId), Convert.ToInt32(item.Id), item.ItemName, Convert.ToInt32(item.ItemQuantity), Convert.ToDecimal(item.ItemCost));
                            var addBulkOrderLogSuccess = data.AddBulkOrderLog(Convert.ToInt32(bulkOrderId), Convert.ToInt32(Session["UserId"]), "Order Item has been upadated: PREVIOUS VALUES: ItemName:" + bulkOrderItem.ItemName + " : ItemQuantity:" + bulkOrderItem.ItemQuantity + " : ItemCost:" + bulkOrderItem.ItemCost + " ::::: NEW VALUES: ItemName:" + item.ItemName + " : ItemQuantity:" + item.ItemQuantity + " : ItemCost:" + item.ItemCost);
                        }
                    }
                    
                 }
            }

            if(bulkOrder.ArtworkPosition != artworkPosition)
            {
                var addBulkOrderLogSuccess = data.AddBulkOrderLog(Convert.ToInt32(bulkOrderId), Convert.ToInt32(Session["UserId"]), "Artwork Position Changed FROM >> " + bulkOrder.ArtworkPosition + " TO >> " + artworkPosition);
            }

            data.UpdateBulkOrder(Convert.ToInt32(bulkOrderId), customerEmail, artworkPosition, Convert.ToDecimal(orderTotal));

            if(bulkOrder.ShipToAddress == shipToAddress && bulkOrder.ShipToCity == shipToCity && bulkOrder.ShipToState == bulkOrder.ShipToState && bulkOrder.ShipToZip == shipToZip) {
                //do nothing
            } 
            else
            {
                var updateAddressSuccess = data.UpdateBulkOrderShipTo(Convert.ToInt32(bulkOrderId), shipToAddress, shipToCity, shipToState, shipToZip);
                if (updateAddressSuccess)
                {
                    var addBulkOrderLogSuccess = data.AddBulkOrderLog(Convert.ToInt32(bulkOrderId), Convert.ToInt32(Session["UserId"]), "Ship To Address Changed FROM >> " + bulkOrder.ShipToAddress + " " + bulkOrder.ShipToCity + "," + bulkOrder.ShipToState + " " + bulkOrder.ShipToZip + " TO >> " + shipToAddress + " " + shipToCity + "," + shipToState + " " + shipToZip);
                }
            }

            if(bulkOrder.OrderTotal != Convert.ToDecimal(orderTotal))
            {
                var addBulkOrderLogSuccess = data.AddBulkOrderLog(Convert.ToInt32(bulkOrderId), Convert.ToInt32(Session["UserId"]), "Order Total Changed FROM >> $" + bulkOrder.OrderTotal + " TO >> $" + orderTotal);
            }

            HttpPostedFileBase fileContent = null;
            if (Request.Files.Count > 0)
            {
                fileContent = Request.Files[0];
            }

            if (fileContent != null && fileContent.ContentLength > 0)
            {
                // get a stream
                var stream = fileContent.InputStream;
                // and optionally write the file to disk
                var extension = System.IO.Path.GetExtension(fileContent.FileName);

                var fileName = Guid.NewGuid() + extension;
                var artworkPath = fileName;
                var path = System.IO.Path.Combine(HttpRuntime.AppDomainAppPath + "/Images/BulkOrderArtwork", fileName);

                fileContent.SaveAs(path);

                data.UpdateBulkOrderArtwork(Convert.ToInt32(bulkOrderId), fileName);

                EmailFunctions email = new EmailFunctions();

                var developmentMode = Convert.ToBoolean(ConfigurationManager.AppSettings["DevelopmentMode"]);
                if (developmentMode)
                {
                    email.sendEmail("robertwhamm@yahoo.com", "Dope Custom Designs", "The artwork is now available for order #" + bulkOrderId, "Order #" + bulkOrderId + " Artwork", "digitizing@lidlaunch.com");
                } else
                {
                    email.sendEmail("ronnie.dcd@gmail.com", "Dope Custom Designs", "The artwork is now available for order #" + bulkOrderId, "Order #" + bulkOrderId + " Artwork", "digitizing@lidlaunch.com");
                }
                

                var addBulkOrderLogSuccess = data.AddBulkOrderLog(Convert.ToInt32(bulkOrderId), Convert.ToInt32(Session["UserId"]), "Artwork source has been Added/Updated to the order");
            }

            return "true";
        }

        public ActionResult AddBulkRework(int bulkOrderBatchId, int bulkOrderItemId, string bulkOrderBlankName, int parentBulkOrderId, int parentBulkOrderBatchId, string note, int quantity, int bulkReworkId, bool shipping)
        {
            dynamic model = new ExpandoObject();
            
            model.bulkOrderBatchId = bulkOrderBatchId;
            model.bulkOrderItemId = bulkOrderItemId;
            model.bulkOrderBlankName = bulkOrderBlankName;
            model.parentBulkOrderId = parentBulkOrderId;
            model.parentBulkOrderBatchId = parentBulkOrderBatchId;
            model.quantity = quantity;
            model.bulkReworkId = bulkReworkId;
            model.note = note;
            model.shipping = shipping;            

            return PartialView("AddBulkRework", model);
        }

        public string CreateBulkRework(string bulkOrderBatchId, string bulkOrderItemId, string bulkOrderBlankName, string quantity, string note, string status, string reworkId, string bulkOrderId)
        {
            BulkData data = new BulkData();
            int intBulkReworkId = Convert.ToInt32(reworkId);

            bool missingBlank = false;

            if(Convert.ToInt32(bulkOrderBatchId) > 0)
            {
                missingBlank = true;
            }

            if(intBulkReworkId > 0)
            {
                data.UpdateBulkRework(Convert.ToInt32(quantity), Convert.ToString(note), Convert.ToString(status), intBulkReworkId);
                var addBulkOrderLogSuccess = data.AddBulkOrderLog(Convert.ToInt32(bulkOrderId), Convert.ToInt32(Session["UserId"]), "Bulk Rework Updated ID(" + reworkId + ") : ItemName:" + bulkOrderBlankName + " : Note:" + note + " : Quantity:" + quantity + " : Status:" + status);
            } 
            else
            {
                intBulkReworkId = data.CreateBulkRework(Convert.ToInt32(bulkOrderItemId), Convert.ToInt32(bulkOrderBatchId), Convert.ToInt32(quantity), Convert.ToString(note), Convert.ToBoolean(missingBlank), Convert.ToString(bulkOrderBlankName));
                var addBulkOrderLogSuccess = data.AddBulkOrderLog(Convert.ToInt32(bulkOrderId), Convert.ToInt32(Session["UserId"]), "Bulk Rework Added ID(" + intBulkReworkId.ToString() + ") : ItemName:" + bulkOrderBlankName + " : Note:" + note + " : Quantity:" + quantity + " : Status:" + status);
            }            

            var success = intBulkReworkId > 0;

            return success.ToString();
        }

        public string MarkDesignDeleted(string designId, string bulkOrderId)
        {
            var success = false;
            var json = "";
            if (checkLoggedIn())
            {
                DesignData designData = new DesignData();
                BulkData bulkData = new BulkData();
                success = designData.MarkDesignDeleted(Convert.ToInt32(designId));
                if (success)
                {
                    var addBulkOrderLogSuccess = bulkData.AddBulkOrderLog(Convert.ToInt32(bulkOrderId), Convert.ToInt32(Session["UserId"]), "Design Deleted: Design Id: " + designId);
                }
                json = new JavaScriptSerializer().Serialize(success);
            }            
            return json;
        }

        public string UnapproveDesign(string designId, string bulkOrderId)
        {
            var success = false;
            var json = "";
            if (checkLoggedIn())
            {
                DesignData designData = new DesignData();
                BulkData bulkData = new BulkData();
                success = designData.UnapproveDesign(Convert.ToInt32(designId));
                if (success)
                {
                    var addBulkOrderLogSuccess = bulkData.AddBulkOrderLog(Convert.ToInt32(bulkOrderId), Convert.ToInt32(Session["UserId"]), "Design Unapproved: Design Id: " + designId);
                }
                json = new JavaScriptSerializer().Serialize(success);
            }
            return json;
        }

        public ActionResult AddNote(int bulkOrderId, int bulkOrderItemId, int designId, int parentBulkOrderId, bool revision, string customerAdded, bool shipping)
        {
            dynamic model = new ExpandoObject();
            if(bulkOrderId > 0)
            {
                model.noteType = "bulkOrder";
                model.idVal = bulkOrderId;
                model.parentBulkOrderId = parentBulkOrderId;
                model.customerAdded = customerAdded;
                model.shipping = shipping;                
            }
            if(bulkOrderItemId > 0)
            {
                model.noteType = "bulkOrderItem";
                model.idVal = bulkOrderItemId;
                model.parentBulkOrderId = parentBulkOrderId;
                model.customerAdded = customerAdded;
                model.shipping = shipping;
            }
            if(designId > 0)
            {
                if(revision)
                {
                    model.noteType = "designRevision";
                }
                else
                {
                    model.noteType = "design";
                }                
                model.idVal = designId;
                model.parentBulkOrderId = parentBulkOrderId;
                model.customerAdded = customerAdded;
                model.shipping = shipping;
            }
            
            return PartialView("AddNote", model);
        }

        public ActionResult AddLog(int bulkOrderId)
        {        
            return PartialView("AddLog", bulkOrderId);
        }

        public string AddOrderLogEntry(string bulkOrderId, string logText)
        {
            BulkData data = new BulkData();
            var success = data.AddBulkOrderLog(Convert.ToInt32(bulkOrderId), Convert.ToInt32(Session["UserId"]), "Manual Log Entry: " + logText);
           
            return new JavaScriptSerializer().Serialize(success);
        }

        public string CreateNote(string noteType, string idVal, string parentBulkOrderId, string text, string attachment, bool customerAdded)
        {            
            BulkData data = new BulkData();
            int noteId = 0;

            if(noteType == "bulkOrder")
            {
                noteId = data.CreateNote(Convert.ToInt32(idVal), 0, 0, Convert.ToInt32(parentBulkOrderId), text, attachment, 0, customerAdded);
                var addBulkOrderLogSuccess = data.AddBulkOrderLog(Convert.ToInt32(parentBulkOrderId), Convert.ToInt32(Session["UserId"]), "Bulk Order Note Added : " + text);
            }
            else if (noteType == "bulkOrderItem")
            {
                noteId = data.CreateNote(0, Convert.ToInt32(idVal), 0, Convert.ToInt32(parentBulkOrderId), text, attachment, 0, customerAdded);
                var addBulkOrderLogSuccess = data.AddBulkOrderLog(Convert.ToInt32(parentBulkOrderId), Convert.ToInt32(Session["UserId"]), "Bulk Order Item Note Added : " + text);
            }
            else if (noteType == "design")
            {
                noteId = data.CreateNote(0, 0, Convert.ToInt32(idVal), Convert.ToInt32(parentBulkOrderId), text, attachment, 0, customerAdded);
            }
            else if(noteType == "designRevision")
            {
                var revisionStatus = customerAdded ? "1:Pending" : "5:OutsourcedChangesPending";
                data.AddDigitizingRevision(Convert.ToInt32(idVal), text, customerAdded, revisionStatus);
                var addBulkOrderLogSuccess = data.AddBulkOrderLog(Convert.ToInt32(parentBulkOrderId), Convert.ToInt32(Session["UserId"]), "Digitizing Revision + " + (Convert.ToBoolean(customerAdded) ? "Customer Requested" : "Internally Requested") + " : " + text);
                //BulkData bulkData = new BulkData();
                //BulkOrder bulkOrder = bulkData.GetBulkOrder(Convert.ToInt32(parentBulkOrderId), "", "");
                EmailFunctions email = new EmailFunctions();      
                if(!customerAdded)
                {
                    var developmentMode = Convert.ToBoolean(ConfigurationManager.AppSettings["DevelopmentMode"]);

                    if (developmentMode)
                    {
                        email.sendEmail("digitizing@lidlaunch.com", "LidLaunch", text, parentBulkOrderId + " : Revision Request", "robertwhamm@yahoo.com");
                    } else
                    {
                        email.sendEmail("digitizing@lidlaunch.com", "LidLaunch", text, parentBulkOrderId + " : Revision Request", "ronnie.dcd@gmail.com");
                    }
                    
                }                
            }

            var success = noteId > 0;

            return success.ToString();
        }

        public ActionResult UploadBulkDesign(string designId, string fromDigitizing, string bulkOrderId)
        {
            DesignData data = new DesignData();
            Design design = new Design();

            if(designId != "")
            {
                design = data.GetDesign(Convert.ToInt32(designId));
            } else
            {
                design.Id = 0;
            }

            dynamic model = new ExpandoObject();

            model.Design = design;
            model.FromDigitizing = Convert.ToBoolean(fromDigitizing);
            model.BulkOrderId = Convert.ToInt32(bulkOrderId);



            return PartialView(model);
        }

        public ActionResult SetBulkDesign(string bulkOrderId)
        {
            dynamic model = new ExpandoObject();

            model.BulkOrderId = Convert.ToInt32(bulkOrderId); 

            return PartialView(model);
        }

        public string SetBulkOrderDesign(string bulkOrderId, string designId)
        {
            BulkData data = new BulkData();
            data.UpdateBulkOrderDesign(Convert.ToInt32(bulkOrderId), Convert.ToInt32(designId));
            var addBulkOrderLogSuccess = data.AddBulkOrderLog(Convert.ToInt32(bulkOrderId), Convert.ToInt32(Session["UserId"]), "Bulk Order Design Set To ID: " + designId);
            return "true";
        }

        public string SetBulkOrderDesignAdditional(string bulkOrderId, string designId)
        {
            BulkData data = new BulkData();
            data.UpdateBulkOrderDesignAdditional(Convert.ToInt32(bulkOrderId), Convert.ToInt32(designId));
            var addBulkOrderLogSuccess = data.AddBulkOrderLog(Convert.ToInt32(bulkOrderId), Convert.ToInt32(Session["UserId"]), "Additional Bulk Order Design Added: " + designId);
            return "true";
        }

        public ActionResult BulkSalesReport()
        {
            if (Convert.ToInt32(Session["UserID"]) > 0)
            {
                if (Convert.ToInt32(Session["UserID"]) == 1)
                {
                    DashboardData data = new DashboardData();
                    BulkSalesReport report = data.GetBulkSalesReport(DateTime.Now.AddDays(-30), DateTime.Now);
                    return View(report);
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

        public ActionResult AdminPanel()
        {
            if (Convert.ToInt32(Session["UserID"]) > 0)
            {
                if (checkLoggedIn())
                {
                    return View();
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
        [HttpGet]
        public string GetSalesData(string goon)
        {
            if(goon != null)
            {
                if(goon == "12jiqnfiwqo901nifo1nr1r819hr1nrjkb12ri1lbr129rWIAFJIAF")
                {
                    DashboardData data = new DashboardData();
                    return new JavaScriptSerializer().Serialize(data.GetSalesData());
                }
            }
            return "";
        }

        public ActionResult BulkOrderItemAvailabilty()
        {
            if (Convert.ToInt32(Session["UserID"]) > 0)
            {
                if (checkLoggedIn())
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

        public string UpdateMasterBulkOrderItem(string id, string sm, string lxl, string xlxxl, string osfa, string available)
        {
            bool success = false;
            BulkData data = new BulkData();
            MasterBulkOrderItem item = new MasterBulkOrderItem();

            item.Id = Convert.ToInt32(id);
            item.SMStock = Convert.ToBoolean(sm);
            item.LXLStock = Convert.ToBoolean(lxl);
            item.XLXXLStock = Convert.ToBoolean(xlxxl);
            item.OSFAStock = Convert.ToBoolean(osfa);
            item.Available = Convert.ToBoolean(available);

            success = data.UpdateMasterBulkOrderItem(item);


            return new JavaScriptSerializer().Serialize(success);
        }

        public string UpdateAdminReviewFinished(string bulkOrderId)
        {
            bool success = false;
            BulkData data = new BulkData();

            success = data.UpdateAdminReviewFinished(Convert.ToInt32(bulkOrderId));

            if (success)
            {
                data.AddBulkOrderLog(Convert.ToInt32(bulkOrderId), Convert.ToInt32(Session["UserID"]), "Admin Review/Designer Review marked as complete.");
            }

            return new JavaScriptSerializer().Serialize(success);
        }
        

        public string CreateBulkOrderBatchMissingItem(string itemName, string masterItemId, string batchId)
        {
            int missingItemId = 0;
            BulkData data = new BulkData();

            missingItemId = data.AddBulkOrderBatchMissingItems(Convert.ToInt32(batchId), Convert.ToInt32(masterItemId), itemName, 0, "", false, false, "");


            return new JavaScriptSerializer().Serialize(missingItemId);
        }

        public string UpdateBulkOrderBatchMissingItemsQuantity(string id, string missingQuantity)
        {
            bool success = false;
            BulkData data = new BulkData();

            success = data.UpdateBulkOrderBatchMissingItemsQuantity(Convert.ToInt32(id), Convert.ToInt32(missingQuantity));

            return new JavaScriptSerializer().Serialize(success);
        }

        public string UpdateBulkOrderBatchMissingItems(string id, string orderedFromSource, string ordered, string outOfStock, string trackingNumber)
        {
            bool success = false;
            BulkData data = new BulkData();

            success = data.UpdateBulkOrderBatchMissingItems(Convert.ToInt32(id), orderedFromSource, Convert.ToBoolean(ordered), Convert.ToBoolean(outOfStock), trackingNumber);

            return new JavaScriptSerializer().Serialize(success);
        }

        public string GetAvailableMasterBulkOrderItems(string filter)
        {
            List<MasterBulkOrderItem> lstItems = new List<MasterBulkOrderItem>();
            BulkData data = new BulkData();
            lstItems = data.GetMasterBulkOrderItemsForDropDown(false);
            var returnLiList = "";
            if(filter != "")
            {
                lstItems = lstItems.Where(i => i.FrontEndName.ToLower().Contains(filter) || i.SKU.ToLower().Contains(filter)).ToList();
            }            
            foreach(MasterBulkOrderItem item in lstItems)
            {
                if (item.OSFA)
                {
                    returnLiList += "<li onclick='$(this).closest(\"tr\").find(\".txtBulkOrderItemName\").val(\"" + item.FrontEndName + " - " + item.ItemColor + " - OSFA\"); hideItemsLists();'><img src='../Images/" + item.ThumbnailpreviewImagePath + "'/> <span class='selectItemName'>" + item.FrontEndName + " - " + item.ItemColor + " - OSFA</span></li>";
                }
                if (item.LXL)
                {
                    returnLiList += "<li onclick='$(this).closest(\"tr\").find(\".txtBulkOrderItemName\").val(\"" + item.FrontEndName + " - " + item.ItemColor + " - L/XL\"); hideItemsLists();'><img src='../Images/" + item.ThumbnailpreviewImagePath + "'/> <span class='selectItemName'>" + item.FrontEndName + " - " + item.ItemColor + " - L/XL</span></li>";
                }
                if (item.SM)
                {
                    returnLiList += "<li onclick='$(this).closest(\"tr\").find(\".txtBulkOrderItemName\").val(\"" + item.FrontEndName + " - " + item.ItemColor + " - S/M\"); hideItemsLists();'><img src='../Images/" + item.ThumbnailpreviewImagePath + "'/> <span class='selectItemName'>" + item.FrontEndName + " - " + item.ItemColor + " - S/M</span></li>";
                }
                if (item.XLXXL)
                {
                    returnLiList += "<li onclick='$(this).closest(\"tr\").find(\".txtBulkOrderItemName\").val(\"" + item.FrontEndName + " - " + item.ItemColor + " - XL/XXL\"); hideItemsLists();'><img src='../Images/" + item.ThumbnailpreviewImagePath + "'/> <span class='selectItemName'>" + item.FrontEndName + " - " + item.ItemColor + " - XL/XXL</span></li>";
                }
                if (item.Manufacturer == "LidLaunch")
                {
                    returnLiList += "<li onclick='$(this).closest(\"tr\").find(\".txtBulkOrderItemName\").val(\"" + item.FrontEndName + "\"); hideItemsLists();'><img src='../Images/" + item.ThumbnailpreviewImagePath + "'/> <span class='selectItemName'>" + item.FrontEndName + "</span></li>";
                }
            }

            return returnLiList;
        }

        public string GetBulkOrdersContainingMissingBlank(string masterItemId, string isOSFA, string isSM, string isLXL, string isXLXXL, string missingQuantity, string itemName, string batchId)
        {
            BulkData data = new BulkData();


            List<string> lstBulkOrderIds = data.GetBulkOrdersContainingMissingBlank(Convert.ToInt32(masterItemId), Convert.ToBoolean(isOSFA), Convert.ToBoolean(isSM), Convert.ToBoolean(isLXL), Convert.ToBoolean(isXLXXL), Convert.ToInt32(batchId));

            var returnString = "";
            foreach(String row in lstBulkOrderIds)
            {
                returnString += row + "<br/>";
            }

            return returnString;
        }

    }
}