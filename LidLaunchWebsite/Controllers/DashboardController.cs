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
            if (Convert.ToInt32(Session["UserID"]) > 0)
            {
                if (Convert.ToInt32(Session["UserID"]) == 1)
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
            else
            {
                return RedirectToAction("Login", "User", null);
            }

        }
        public ActionResult BatchOrder(string batchId)
        {
            if (Convert.ToInt32(Session["UserID"]) > 0)
            {
                if (Convert.ToInt32(Session["UserID"]) == 1)
                {
                    DashboardData data = new DashboardData();
                    BatchDashboard dashboard = data.GetBatch(Convert.ToInt32(batchId));
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
        public ActionResult CreateBatch()
        {
            if (Convert.ToInt32(Session["UserID"]) > 0)
            {
                if (Convert.ToInt32(Session["UserID"]) == 1)
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
            else
            {
                return RedirectToAction("Login", "User", null);
            }

        }
        public ActionResult PendingProducts()
        {
            if (Convert.ToInt32(Session["UserID"]) > 0)
            {
                if (Convert.ToInt32(Session["UserID"]) == 1)
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
            else
            {
                return RedirectToAction("Login", "User", null);
            }

        }

        public ActionResult NeedsDigitizing()
        {
            if (Convert.ToInt32(Session["UserID"]) > 0)
            {
                if (Convert.ToInt32(Session["UserID"]) == 1)
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
            else
            {
                return RedirectToAction("Login", "User", null);
            }

        }
        public ActionResult HatManager()
        {
            if (Convert.ToInt32(Session["UserID"]) > 0)
            {
                if (Convert.ToInt32(Session["UserID"]) == 1)
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
            else
            {
                return RedirectToAction("Login", "User", null);
            }

        }
        public ActionResult HatTypeEdit(int hatTypeId = 0)
        {
            if (Convert.ToInt32(Session["UserID"]) > 0)
            {
                if (Convert.ToInt32(Session["UserID"]) == 1)
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
                        var path = Path.Combine(Server.MapPath("~/Images/HatAssets/"), fileName);

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
                        var path = Path.Combine(Server.MapPath("~/Images/HatAssets/"), fileName);

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
            if (Convert.ToInt32(id) > 0)
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


        public string ApproveProduct(string id)
        {
            bool success = false;
            if (Convert.ToInt32(Session["UserID"]) > 0)
            {
                if (Convert.ToInt32(Session["UserID"]) == 1)
                {
                    ProductData prodData = new ProductData();                    

                    success = prodData.ApproveProduct(Convert.ToInt32(id));                    
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


        public bool checkLoggedIn()
        {            
            if (Convert.ToInt32(Session["UserID"]) == 1 || Convert.ToInt32(Session["UserID"]) == 643)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public string UploadDigitizedFile(string designId)
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
                        var extension = Path.GetExtension(fileContent.FileName);
                        
                        var fileName = Guid.NewGuid().ToString() + extension;
                        var path = Path.Combine(Server.MapPath("~/Images/DesignImages/Digitizing/DST"), fileName);

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
        public string UpdateDesignDigitizedPreview(string designId)
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
                        var extension = Path.GetExtension(fileContent.FileName);

                        var fileName = Guid.NewGuid().ToString() + extension;
                        var path = Path.Combine(Server.MapPath("~/Images/DesignImages/Digitizing/Preview"), fileName);

                        fileContent.SaveAs(path);
                        returnValue = fileName;

                        //update design digitized file in database
                        DesignData data = new DesignData();
                        var success = data.UpdateDesignDigitizedPreview(Convert.ToInt32(designId), fileName);
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
        public string UpdateDesignDigitizedInfoImage(string designId)
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
                        var extension = Path.GetExtension(fileContent.FileName);

                        var fileName = Guid.NewGuid().ToString() + extension;
                        var path = Path.Combine(Server.MapPath("~/Images/DesignImages/Digitizing/Info"), fileName);

                        fileContent.SaveAs(path);
                        returnValue = fileName;

                        //update design digitized file in database
                        DesignData data = new DesignData();
                        var success = data.UpdateDesignDigitizedInfoImage(Convert.ToInt32(designId), fileName);
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


        public ActionResult ViewBulkOrders()
        {

            if (!checkLoggedIn())
            {
                return RedirectToAction("Index", "Home", null);
            }
            else
            {
                List<BulkOrder> lstBulkOrders = new List<BulkOrder>();
                BulkData data = new BulkData();
                lstBulkOrders = data.GetBulkOrderData();
                lstBulkOrders = lstBulkOrders.OrderByDescending(bo => bo.OrderPaid).ToList();
                return View(lstBulkOrders);
            }
        }

        public string UpdateBulkOrderPaid(string bulkOrderId, string orderPaid)
        {

            if (!checkLoggedIn())
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

        public ActionResult BulkDigitizing()
        {
            if (Convert.ToInt32(Session["UserID"]) > 0)
            {
                if (Convert.ToInt32(Session["UserID"]) == 1)
                {
                    List<BulkOrder> lstBulkOrders = new List<BulkOrder>();
                    BulkData data = new BulkData();
                    lstBulkOrders = data.GetBulkOrderData();
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

    }
}