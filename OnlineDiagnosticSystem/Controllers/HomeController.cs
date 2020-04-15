using DatabaseLayer;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TranscriptMgt;

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
            ViewBag.user = Session["UserName"];
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


                if (user.UserTypeID == 2) //doctor
                {
                    var doc = db.DoctorTables.Where(u => u.UserID == user.UserID).FirstOrDefault();
                    Session["Doctor"] = doc;

                }
                else if (user.UserTypeID == 3) // lab
                {
                    var lab = db.LabTables.Where(u => u.UserID == user.UserID).FirstOrDefault();
                    Session["Lab"] = lab;
                }
                else if (user.UserTypeID == 4) // patient
                {
                    var pat = db.PatientTables.Where(u => u.UserID == user.UserID).FirstOrDefault();
                    Session["Patient"] = pat;
                }



                return View("Index");

            }
            user = db.UserTables.Where(u => u.Email == email && u.Password == password && u.isVerified == false).FirstOrDefault();
            if(user != null)
            {
                ViewBag.Message = "Emal already exisit please enter detials";
                Session["User"] = user;
                if (user.UserTypeID == 2) //doctor
                {
                    var doc = db.DoctorTables.Where(u => u.UserID == user.UserID).FirstOrDefault();
                    if (doc == null)
                    {
                        return RedirectToAction("AddDoctor");
                    }
                    ViewBag.Message = "Account is under review";

                }
                else if (user.UserTypeID == 3) // lab
                {
                    var lab = db.LabTables.Where(u => u.UserID == user.UserID).FirstOrDefault();
                    if (lab == null)
                    {
                        return RedirectToAction("AddLab");
                    }
                    ViewBag.Message = "Account is under review";
                }
                else if (user.UserTypeID == 4) // patient
                {
                    var pat = db.PatientTables.Where(u => u.UserID == user.UserID).FirstOrDefault();
                    if (pat == null)
                    {
                        return RedirectToAction("AddPatient");
                    }
                    ViewBag.Message = "Account is under review";
                }
            }
            else
            {
                ViewBag.message = "Username and password incorrect";
            }
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
            if (user != null)
            {
                if (ModelState.IsValid)
                {
                    var finduser = db.UserTables.Where(u => u.Email == user.Email).FirstOrDefault();
                    if (finduser == null)
                    {
                        finduser = db.UserTables.Where(u => u.Email == user.Email && u.isVerified == false).FirstOrDefault();
                        if (finduser == null)
                        {
                            if (user.UserTypeID == 2) //doctor
                            {
                                user.isVerified = false;
                            }
                            else if (user.UserTypeID == 3) // lab
                            {
                                user.isVerified = false;
                            }
                            else if (user.UserTypeID == 4) // patient
                            {
                                user.isVerified = true;
                            }
                            else if (user.UserTypeID == 1)//admin
                            {
                                user.isVerified = false;
                            }
                            db.UserTables.Add(user);
                            db.SaveChanges();

                            Session["User"] = user;
                            if (user.UserTypeID == 2) //doctor
                            {
                                return RedirectToAction("AddDoctor");
                            }
                            else if (user.UserTypeID == 3) // lab
                            {
                                return RedirectToAction("AddLab");
                            }
                            else if (user.UserTypeID == 4) // patient
                            {
                                return RedirectToAction("AddPatient");
                            }
                            else if (user.UserTypeID == 1) //admin
                            {
                                ViewBag.Message = "Account is under review";
                            }
                        }
                    }
                    else
                    {
                        ViewBag.Message = "Emal already exisit please enter detials";
                        Session["User"] = finduser;
                        if (finduser.UserTypeID == 2) //doctor
                        {
                            var doc = db.DoctorTables.Where(u => u.UserID == finduser.UserID).FirstOrDefault();
                            if (doc == null)
                            {
                                return RedirectToAction("AddDoctor");
                            }
                            ViewBag.Message = "Account is under review";
                            
                        }
                        else if (finduser.UserTypeID == 3) // lab
                        {
                            var lab = db.LabTables.Where(u => u.UserID == finduser.UserID).FirstOrDefault();
                            if (lab == null)
                            {
                                return RedirectToAction("AddLab");
                            }
                            ViewBag.Message = "Account is under review";
                        }
                        else if (finduser.UserTypeID == 4) // patient
                        {
                            var pat = db.PatientTables.Where(u => u.UserID == finduser.UserID).FirstOrDefault();
                            if (pat == null)
                            {
                                return RedirectToAction("AddPatient");
                            }
                            ViewBag.Message = "Account is under review";
                        }
                        

                    }
                    
                    
                }
            }
            else
            {
                ViewBag.Message = "Provide correct detials";
            }
            
            ViewBag.UserTypeID = new SelectList(db.UserTypeTables.Where(u => u.UserTypeID != 1), "UserTypeID", "UserType", "0");
            return View("CreateUser");
        }

        public ActionResult AddDoctor()
        {
            ViewBag.GenderID = new SelectList(db.GenderTables.ToList(), "GenderID", "Name", "0");
            ViewBag.AccountTypeID = new SelectList(db.AccountTypeTables.ToList(), "AccountTypeID", "Name", "0");
            return View();
        }
        [HttpPost]
        public ActionResult AddDoctor(DoctorTable doctor)
        {
            if(Session["User"] != null)
            {
                var user = (UserTable)Session["User"];
                doctor.UserID = user.UserID;
            if (ModelState.IsValid)
            {
                    var finddoctor = db.DoctorTables.Where(d => d.EmailAddress == doctor.EmailAddress).FirstOrDefault();
                    if (finddoctor == null)
                {
                    db.DoctorTables.Add(doctor);
                    db.SaveChanges();

                    if (doctor.LogoFile != null)
                    {
                        var folder = "~/Content/DoctorImages";
                        var file = string.Format("{0}.png", doctor.DoctorID);
                        var response = FileHelpers.UploadPhoto(doctor.LogoFile, folder, file);
                        if (response)
                        {
                            var pic = string.Format("{0}/{1}", folder, file);
                            doctor.Photo = pic;
                            db.Entry(doctor).State = EntityState.Modified;
                            db.SaveChanges();
                            return View("UnderReview");
                        }
                    }
                }
                    else
                    {
                        ViewBag.Message = "Already registered";
                    }

                }
            }
            else
            {
                return RedirectToAction("Login");
            }
            ViewBag.GenderID = new SelectList(db.GenderTables.ToList(), "GenderID", "Name", doctor.GenderID);
            ViewBag.AccountTypeID = new SelectList(db.AccountTypeTables.ToList(), "AccountTypeID", "Name", doctor.AccountTypeID);
            return View(doctor);
        }

        public ActionResult AddLab()
        {
            ViewBag.AccountTypeID = new SelectList(db.AccountTypeTables.ToList(), "AccountTypeID", "Name", "0");
            return View();
        }

        [HttpPost]
        public ActionResult AddLab(LabTable lab)
        {

            if (Session["User"] != null)
            {
                var user = (UserTable)Session["User"];
                lab.UserID = user.UserID;


               
                if (ModelState.IsValid)
                {
                    var findlab = db.LabTables.Where(d => d.EmailAddress == lab.EmailAddress).FirstOrDefault();
                    if (findlab == null)
                    {
                        db.LabTables.Add(lab);
                        db.SaveChanges();

                        if (lab.LogoFile != null)
                        {
                            var folder = "~/Content/LabImages";
                            var file = string.Format("{0}.png", lab.LabID);
                            var response = FileHelpers.UploadPhoto(lab.LogoFile, folder, file);
                            if (response)
                            {
                                var pic = string.Format("{0}/{1}", folder, file);
                                lab.Photo = pic;
                                db.Entry(lab).State = EntityState.Modified;
                                db.SaveChanges();
                                return View("UnderReview");
                            }
                        }
                    }
                    else
                    {
                        ViewBag.Message = "Already registered";
                    }

                }
            }
            else
            {
                return RedirectToAction("Login");
            }
          
            ViewBag.AccountTypeID = new SelectList(db.AccountTypeTables.ToList(), "AccountTypeID", "Name", lab.AccountTypeID);
            return View(lab);
        }

        public ActionResult AddPatient()
        {
            ViewBag.GenderID = new SelectList(db.GenderTables.ToList(), "GenderID", "Name", "0");

            return View();
        }
        [HttpPost]
        public ActionResult AddPatient(PatientTable patient)
        {
            if (Session["User"] != null)
            {

                var user = (UserTable)Session["User"];
                patient.UserID = user.UserID;

                if (ModelState.IsValid)
                {
                    var findpatient = db.PatientTables.Where(d => d.Email == patient.Email).FirstOrDefault();
                    if (findpatient == null)
                    {
                        db.PatientTables.Add(patient);
                        db.SaveChanges();
                        if (patient.LogoFile != null)
                        {
                            var folder = "~/Content/PatientImages";
                            var file = string.Format("{0}.png", patient.PatientID);
                            var response = FileHelpers.UploadPhoto(patient.LogoFile, folder, file);
                            if (response)
                            {
                                var pic = string.Format("{0}/{1}", folder, file);
                                patient.Photo = pic;
                                db.Entry(patient).State = EntityState.Modified;
                                db.SaveChanges();
                                
                            }
                        }
                        return RedirectToAction("Login");
                    }
                    else
                    {
                        ViewBag.Message = "Already registered";
                    }

                }
            }
            else
            {
                return RedirectToAction("Login");
            }
            ViewBag.GenderID = new SelectList(db.GenderTables.ToList(), "GenderID", "Name", patient.PatientID);

            return View(patient);
        }

        public ActionResult UnderReview()
        {

            return View();
        }

        public ActionResult LogoutUser()
        {
            Logout();
            return RedirectToAction("Index");
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