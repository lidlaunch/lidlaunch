using LidLaunchWebsite.Classes;
using LidLaunchWebsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LidLaunchWebsite.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ProductData productData = new ProductData();
            List<WebsiteProduct> lstWebProds = new List<WebsiteProduct>();

            lstWebProds = productData.GetWebsiteProductsRecent();

            return View(lstWebProds);
        }
        public PartialViewResult Menu()
        {            
            Home home = new Home();
            ProductData productData = new ProductData();

            List<Category> lstCategories = new List<Category>();
            lstCategories = productData.GetCategories();

            home.Categories = lstCategories;

            return PartialView(home);
        }
        
    }
}