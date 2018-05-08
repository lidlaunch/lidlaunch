using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LidLaunchWebsite.Controllers
{
    public class TermsController : Controller
    {
        // GET: Terms
        public ActionResult TermsAndConditions()
        {
            return View();
        }
        public ActionResult TermsOfSalesAndDelivery()
        {
            return View();
        }
        public ActionResult Returns()
        {
            return View();
        }
        public ActionResult Privacy()
        {
            return View();
        }
        public ActionResult DesignerPayouts()
        {
            return View();
        }
    }
}