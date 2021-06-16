using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using LidLaunchWebsite.Classes;
using LidLaunchWebsite.Models;

namespace LidLaunchWebsite.Controllers
{
    public class ExpenseController : Controller
    {
        ExpenseData data = new ExpenseData();
        // GET: Expense
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult EditExpense(int expenseId)
        {
            if (checkAdminLoggedIn())
            {
                return PartialView(data.GetExpense(expenseId));
            } 
            else
            {
                return RedirectToAction("Login", "User");
            }
        }

        public string SaveExpense(string id, string type, string amount, string dateFrom, string dateTo, string title, string description)
        {
            if (checkAdminLoggedIn())
            {
                var success = false;
                var attachmentName = "";
                var attachmentSource = Request.Files["attachment"];
                if (attachmentSource != null && attachmentSource.ContentLength > 0)
                {
                    // get a stream
                    var stream = attachmentSource.InputStream;
                    // and optionally write the file to disk
                    var extension = System.IO.Path.GetExtension(attachmentSource.FileName);
                    var guidName = Guid.NewGuid().ToString();

                    attachmentName = guidName + "-" + Guid.NewGuid().ToString() + extension;

                    var path = System.IO.Path.Combine(HttpRuntime.AppDomainAppPath + "/Images/ExpenseAttachment/", attachmentName);

                    attachmentSource.SaveAs(path);
                }

                if (Convert.ToInt32(id) == 0)
                {
                    var expenseId = data.CreateExpense(type, Convert.ToDecimal(amount), Convert.ToDateTime(dateFrom), Convert.ToDateTime(dateTo), title, description, attachmentName);

                    if (expenseId > 0)
                    {
                        success = true;
                    }
                }
                else
                {
                    success = data.UpdateExpense(Convert.ToInt32(id), type, Convert.ToDecimal(amount), Convert.ToDateTime(dateFrom), Convert.ToDateTime(dateTo), title, description, attachmentName);
                }


                return new JavaScriptSerializer().Serialize(success);
            }
            else
            {
                return "";
            }
        }

        public string DeleteExpense(string id)
        {
            if (checkAdminLoggedIn())
            {
                var success = data.DeleteExpense(Convert.ToInt32(id));

                return new JavaScriptSerializer().Serialize(success);
            }
            else
            {
                return "";
            }            
        }

        public ActionResult ViewExpenses(string dateFrom, string dateTo)
        {
            if (checkAdminLoggedIn())
            {

                List<Expense> expenses = data.GetExpenses(Convert.ToDateTime(dateFrom), Convert.ToDateTime(dateTo));
                expenses = expenses.OrderBy(e => e.DateFrom).ToList();

                return View(expenses);
            }
            else
            {
                return RedirectToAction("Login", "User");
            }
        }

        public bool checkAdminLoggedIn()
        {
            if (Convert.ToInt32(Session["UserID"]) == 1)
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