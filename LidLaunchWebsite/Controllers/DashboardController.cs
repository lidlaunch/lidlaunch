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
                    Session["PayoutAmmount"] = dashboard.TotalAvailableForPayout;
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

            var payoutId = dashboardData.CreateDesignerPayout(designer.Id, (decimal)Session["PayoutAmmount"], designer.PaypalAddress);

            if (payoutId > 0)
            {                
                EmailFunctions email = new EmailFunctions();
                User user = new User();
                UserData userData = new UserData();
                user = userData.GetUser((Int32)Session["UserID"]);
                var success = email.sendEmail(user.Email, user.FirstName + ' ' + user.LastName, email.payoutEmail(designer.PaypalAddress, Session["PayoutAmmount"].ToString()), "Your Payout Request Has Been Submitted", designer.PaypalAddress);
                Session["PayoutAmmount"] = null;
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
                            email.sendEmail(info.UserEmail, "LidLaunch Designer", email.productDenyEmail(info.ProductName, denyReason, info.ArtSource), "Your Lid Launch Design Was Denied", "");
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

        public string ContactBulkCustomer(string bulkOrderId, string emailType)
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
                            email.sendEmail(info.UserEmail, "LidLaunch Designer", email.productApprovedEmail(info.ProductName, "https://lidlaunch.com/Product?id=" + id), "Your Lid Launch Design Was Approved", "");
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

            var success = email.sendEmail(customerEmail, "Lid Launch Customer", email.requestArtworkEmail(bulkOrderId), "Order #" + bulkOrderId + " Artwork Request", ""); 
            if (success)
            {
                bulkData.UpdateArtworkEmailSent(Convert.ToInt32(bulkOrderId));
                bulkData.CreateNote(Convert.ToInt32(bulkOrderId), 0, 0, Convert.ToInt32(bulkOrderId), "Missing Artwork Email Sent", "", 0, false);
            }

            var json = new JavaScriptSerializer().Serialize(success);

            return json;
        }

        public string SendColorConfirmationEmail(string bulkOrderId, string customerEmail, string noteText)
        {
            BulkData bulkData = new BulkData();
            EmailFunctions email = new EmailFunctions();

            var success = email.sendEmail(customerEmail, "Lid Launch Customer", email.colorConfirmationEmail(bulkOrderId, noteText), "Order #" + bulkOrderId + " Color Confirmation Request", "");
            if (success)
            {
                bulkData.UpdateColorConfirmationEmailSent(Convert.ToInt32(bulkOrderId));
                bulkData.CreateNote(Convert.ToInt32(bulkOrderId), 0, 0, Convert.ToInt32(bulkOrderId), "Color Confirmation Email Sent: " + noteText, "", 0, false);
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
                email.sendEmail(customerEmail, "Lid Launch Customer", email.orderShippedEmail(trackingNumber), "Your Order Is On It's Way!", "");
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
                model.lstBulkOrderBatches = data.GetBulkOrderBatches();
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
                    return "true";
                }
                else
                {
                    data.UpdateBulkOrderPaid(Convert.ToInt32(bulkOrderId), true);
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
                    data.CreateNote(Convert.ToInt32(bulkOrderId), 0, 0, Convert.ToInt32(bulkOrderId), "ORDER HAS BEEN REFUNDED.", "", 0, false);
                }

                var json = new JavaScriptSerializer().Serialize(success);

                return json;
            }

        }

        public string UpdateBulkOrderBatchId(string bulkOrderId, string batchId)
        {
            BulkData data = new BulkData();
            data.UpdateBulkOrderBatchId(Convert.ToInt32(bulkOrderId), Convert.ToInt32(batchId));
            return "true";
        }

        public string ReleaseToDigitizer(string bulkOrderId)
        {
            if (Convert.ToInt32(Session["UserID"]) == 1)
            {
                BulkData data = new BulkData();
                data.ReleaseToDigitizer(Convert.ToInt32(bulkOrderId));
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
                    BulkOrder bulkOrder = new BulkOrder();
                    BulkData data = new BulkData();
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

                    return PartialView("BulkOrderDetailsPopup", bulkOrder);
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

        public string UpdateBulkOrder(string items, string customerEmail, string artworkPosition, string bulkOrderId, string orderTotal)
        {
            List<BulkOrderItem> lstItems = new List<BulkOrderItem>();
            lstItems = Newtonsoft.Json.JsonConvert.DeserializeObject<List<BulkOrderItem>>(items);
            BulkData data = new BulkData();
            foreach(BulkOrderItem item in lstItems)
            {
                if(item.Id == 0)
                {
                    data.AddBulkOrderItem(Convert.ToInt32(bulkOrderId), item.ItemName, Convert.ToInt32(item.ItemQuantity), Convert.ToDecimal(item.ItemCost));
                } else
                {
                    data.UpdateBulkOrderItem(Convert.ToInt32(bulkOrderId), Convert.ToInt32(item.Id), item.ItemName, Convert.ToInt32(item.ItemQuantity), Convert.ToDecimal(item.ItemCost));
                 }
            }

            data.UpdateBulkOrder(Convert.ToInt32(bulkOrderId), customerEmail, artworkPosition, Convert.ToDecimal(orderTotal));

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
                email.sendEmail("ronnie.dcd@gmail.com", "Dope Custom Designs", "The artwork is now available for order #" + bulkOrderId, "Order #" + bulkOrderId + " Artwork", "digitizing@lidlaunch.com");
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

        public string CreateBulkRework(string bulkOrderBatchId, string bulkOrderItemId, string bulkOrderBlankName, string quantity, string note, string status, string reworkId)
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
            } 
            else
            {
                intBulkReworkId = data.CreateBulkRework(Convert.ToInt32(bulkOrderItemId), Convert.ToInt32(bulkOrderBatchId), Convert.ToInt32(quantity), Convert.ToString(note), Convert.ToBoolean(missingBlank), Convert.ToString(bulkOrderBlankName));
            }            

            var success = intBulkReworkId > 0;

            return success.ToString();
        }

        //public string CreateBulkReworkBatch()
        //{
        //    BulkData data = new BulkData();

        //}

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

        public string CreateNote(string noteType, string idVal, string parentBulkOrderId, string text, string attachment, bool customerAdded)
        {            
            BulkData data = new BulkData();
            int noteId = 0;

            if(noteType == "bulkOrder")
            {
                noteId = data.CreateNote(Convert.ToInt32(idVal), 0, 0, Convert.ToInt32(parentBulkOrderId), text, attachment, 0, customerAdded);
            }
            else if (noteType == "bulkOrderItem")
            {
                noteId = data.CreateNote(0, Convert.ToInt32(idVal), 0, Convert.ToInt32(parentBulkOrderId), text, attachment, 0, customerAdded);
            }
            else if (noteType == "design")
            {
                noteId = data.CreateNote(0, 0, Convert.ToInt32(idVal), Convert.ToInt32(parentBulkOrderId), text, attachment, 0, customerAdded);
            }
            else if(noteType == "designRevision")
            {
                data.AddDigitizingRevision(Convert.ToInt32(idVal), text, customerAdded);
                //BulkData bulkData = new BulkData();
                //BulkOrder bulkOrder = bulkData.GetBulkOrder(Convert.ToInt32(parentBulkOrderId), "", "");
                EmailFunctions email = new EmailFunctions();      
                if(!customerAdded)
                {
                    email.sendEmail("digitizing@lidlaunch.com", "Lid Launch", text, parentBulkOrderId + " : Revision Request", "ronnie.dcd@gmail.com");
                }                
            }

            var success = noteId > 0;

            return success.ToString();
        }

        public ActionResult UploadBulkDesign(string designId, string fromDigitizing)
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



            return PartialView(model);
        }

        public ActionResult SetBulkDesign()
        {
            return PartialView();
        }

        public string SetBulkOrderDesign(string bulkOrderId, string designId)
        {
            if (!checkLoggedIn())
            {
                return "false";
            }
            else
            {
                BulkData data = new BulkData();
                data.UpdateBulkOrderDesign(Convert.ToInt32(bulkOrderId), Convert.ToInt32(designId));
                return "true";

            }

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

    }
}