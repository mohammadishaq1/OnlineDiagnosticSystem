using DatabaseLayer;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineDiagnosticSystem.Controllers
{
    public class HomeController : Controller
    {
        private OnlineDiagnosticLabSystemDbEntities db = new OnlineDiagnosticLabSystemDbEntities();
        public ActionResult Index()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            return View();
        }
        public ActionResult StartTemplate()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            if(string.IsNullOrEmpty(email) && string.IsNullOrEmpty(password))
            {
                return View("Login");
            }
            var user = db.UserTables.Where(u => u.Email == email && u.Password == password && u.isVerified == true).FirstOrDefault();
            if (user != null)
            {
                Session["UserID"] = user.UserID;
                Session["UserTypeID"] = user.UserTypeID;
                Session["UserName"] = user.UserName;
                Session["Password"] = user.Password;
                Session["Email"] = user.Email;
                Session["ContactNo"] = user.ContactNo;
                Session["Description"] = user.Description;
                Session["isVerified"] = user.isVerified;
                return View("Index");

            }
            ViewBag.message = "Username and password incorrect";
            Logout();
            return View("Login");

        }

        public void Logout()
        {
            Session["UserID"] = string.Empty;
            Session["UserTypeID"] = string.Empty;
            Session["UserName"] = string.Empty;
            Session["Password"] = string.Empty;
            Session["Email"] = string.Empty;
            Session["ContactNo"] = string.Empty;
            Session["Description"] = string.Empty;
            Session["isVerified"] = string.Empty;
          
        }


        public ActionResult ChangePassword()
        {
          
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(string OldPassword, string NewPassword, string ConfirmPassword)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            int? userid = Convert.ToInt32(Session["UserID"].ToString());
            UserTable users = db.UserTables.Find(userid);
            if(users.Password == OldPassword)
            {
                if(NewPassword == ConfirmPassword)
                {
                    users.Password = NewPassword;
                    db.Entry(users).State = EntityState.Modified;
                    db.SaveChanges();
                    ViewBag.message = "Change Successfully";
                    return RedirectToAction("Login", "Home");
                }
                else
                {
                    ViewBag.message = "new and confirm password not match";
                    return View("ChangePassword");
                }
            }
            else
            {
                ViewBag.message = "Old password is wrong";
                return View("ChangePassword");
            }

        }


        public ActionResult CreateUser()
        {

            ViewBag.UserTypeID = new SelectList(db.UserTypeTables.Where(u => u.UserTypeID != 1), "UserTypeID", "UserType", "0");
            return View();
        }


        [HttpPost]
        public ActionResult CreateUser(UserTable user)
        {

            ViewBag.UserTypeID = new SelectList(db.UserTypeTables.Where(u => u.UserTypeID != 1), "UserTypeID", "UserType", "0");
            return View();
        }

        public ActionResult AddDocotor()
        {

            return View();
        }
        [HttpPost]
        public ActionResult AddDocotor(DoctorTable doctor)
        {

            return View();
        }

        public ActionResult AddLab()
        {

            return View();
        }

        [HttpPost]
        public ActionResult AddLab(LabTable lab)
        {

            return View();
        }

        public ActionResult AddPatient()
        {

            return View();
        }
        [HttpPost]
        public ActionResult AddPatient(PatientTable patient)
        {

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}