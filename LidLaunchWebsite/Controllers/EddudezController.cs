using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LidLaunchWebsite.Controllers
{
    public class EddudezController : Controller
    {
        // GET: Eddudez
        public ActionResult Index()
        {
            return Redirect("https://lidlaunch.com/Product?id=415");
        }
    }
}