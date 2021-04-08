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
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        } 

        public ActionResult Logout()
        {
            Session["UserID"] = null;
            Session["DesignerID"] = null;
            return RedirectToAction("Index", "Home", null);
        }

        public ActionResult ResetPassword(string ResetCode)
        {
            if(ResetCode == null || ResetCode == "")
            {
                return RedirectToAction("Index", "Home", null);
            } else
            {
                Session["ResetCode"] = ResetCode;
                return View();
            }            
        }

        public string ResetPasswordProcess(string email, string password)
        {
            var success = false;
            if (Session["ResetCode"] == null || (string)Session["ResetCode"] == "")
            {
                //do nothing
            } else
            {
                UserData userData = new UserData();
                success = userData.UpdatePassword(email, (string)Session["ResetCode"], password);
            }            
            var json = new JavaScriptSerializer().Serialize(success);
            Session["ResetCode"] = null;
            return json;
        }

        public string CreateUser(string firstName, string lastName, string middleInitial, string email, string password)
        {
            UserData userData = new UserData();
            var userId = userData.CreateUser(firstName, lastName, middleInitial, email, password);
            if(userId > 0)
            {
                EmailFunctions emailFunc = new EmailFunctions();
                bool emailSuccess = emailFunc.sendEmail(email, firstName + " "  + lastName, emailFunc.welcomeEmail, "Welcome to LidLaunch, " + firstName + "!", "");
            }
            var json = new JavaScriptSerializer().Serialize(userId);
            Session["UserID"] = userId;
            return json;
        }
        public string UpdateUser(string firstName, string lastName, string middleInitial, string email, string password, string userId)
        {
            UserData userData = new UserData();
            var success = userData.UpdateUser(firstName, lastName, middleInitial, email, password, Convert.ToInt32(userId));
            var json = new JavaScriptSerializer().Serialize(success);
            return json;
        }
        public string DeleteUser(string userId)
        {
            UserData userData = new UserData();
            var success = userData.DeleteUser(Convert.ToInt32(userId));
            var json = new JavaScriptSerializer().Serialize(success);
            return json;
        }
        public string GetUsers()
        {
            UserData data = new UserData();
            List<User> lstUsers = new List<User>();
            lstUsers = data.GetUsers();
            var json = new JavaScriptSerializer().Serialize(lstUsers);
            return json;
        }
        public string GetUser(string userId)
        {
            UserData data = new UserData();
            User user = new User();
            user = data.GetUser(Convert.ToInt32(userId));
            var json = new JavaScriptSerializer().Serialize(user);
            return json;
        }
        public string LoginUser(string email, string password)
        {
            UserData data = new UserData();
            User user = new User();
            user = data.LoginUser(email,password);
            var json = new JavaScriptSerializer().Serialize(user);
            DesignerData designerData = new DesignerData();
            Designer designer = new Designer();
            designer = designerData.GetDesigner(user.Id);
            Session["UserID"] = user.Id;
            Session["UserEmail"] = user.Email;
            Session["DesignerID"] = designer.Id;
            Logger.Log("User Logged In");
            return json;
        }
        public string SendPasswordResetEmail(string email)
        {
            UserData data = new UserData();
            var resetCode = Guid.NewGuid().ToString();
            var codeExpiration = DateTime.Now.AddHours(2);
            var success = data.SetUserPasswordResetInfo(email, resetCode, codeExpiration);
            if (success)
            {
                EmailFunctions emailFunc = new EmailFunctions();
                success = emailFunc.sendEmail(email, "LidLaunch User", emailFunc.passwordReset(resetCode), "Reset Your LidLaunch Password", "");
            }
            var json = new JavaScriptSerializer().Serialize(success);
            return json;
        }
    }
}