using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LidLaunchWebsite.Models;
using LidLaunchWebsite.Classes;
using System.Web.Script.Serialization;

namespace LidLaunchWebsite.Controllers
{
    public class DesignerController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            if(Convert.ToInt32(Session["UserID"]) > 0)
            {
                if (Convert.ToInt32(Session["DesignerID"]) > 0)
                {
                    return RedirectToAction("Index", "Home", null);

                } else
                {
                    return View();
                }
                    
            } else
            {
                return RedirectToAction("Login", "User", null);
            }
        }
        public ActionResult DesignerProfile(string Id)
        {
            DesignerData designerData = new DesignerData();
            Designer designer = new Designer();
            designer = designerData.GetDesignerByDesignerId(Convert.ToInt32(Id));
            return View(designer);
        }
        public string CreateDesigner(string shopName, string paypalAddress, string street, string city, string state, string zip, string phone)
        {
            DesignerData designerData = new DesignerData();
            var designerId = designerData.CreateDesigner(shopName,paypalAddress,street,city,state,zip,phone,Convert.ToInt32(Session["UserID"].ToString()));
            var json = new JavaScriptSerializer().Serialize(designerId);
            Session["DesignerID"] = designerId;
            return json;
        }
        public string UpdateDesigner(string shopName, string paypalAddress, string street, string city, string state, string zip, string phone)
        {
            DesignerData designerData = new DesignerData();
            var success = designerData.UpdateDesigner(shopName, paypalAddress, street, city, state, zip, phone, Convert.ToInt32(Session["DesignerID"].ToString()));
            var json = new JavaScriptSerializer().Serialize(success);
            return json;
        }
        public string DeleteDesigner(string designerId)
        {
            DesignerData designerData = new DesignerData();
            var success = designerData.DeleteDesigner(Convert.ToInt32(designerId));
            var json = new JavaScriptSerializer().Serialize(success);
            return json;
        }     
    }
}