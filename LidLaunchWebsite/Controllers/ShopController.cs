using LidLaunchWebsite.Classes;
using LidLaunchWebsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LidLaunchWebsite.Controllers
{
    public class ShopController : Controller
    {
        public ActionResult Index(string search, string category)
        {
            ProductData productData = new ProductData();
            List<WebsiteProduct> lstWebProds = new List<WebsiteProduct>();

            lstWebProds = productData.GetWebsiteProducts(Convert.ToString(search), Convert.ToInt32(category));

            return View(lstWebProds);

        }
    }
}