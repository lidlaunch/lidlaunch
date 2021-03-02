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
            BulkData bulkData = new BulkData();
            var totalHats = bulkData.GetTotalHatsCompleted();
            
            return View(totalHats);
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