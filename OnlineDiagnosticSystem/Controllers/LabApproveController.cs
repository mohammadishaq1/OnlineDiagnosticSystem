using DatabaseLayer;
using OnlineDiagnosticSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineDiagnosticSystem.Controllers
{
    public class LabApproveController : Controller
    {
        private OnlineDiagnosticLabSystemDbEntities db = new OnlineDiagnosticLabSystemDbEntities();
        // GET: DoctorApprove
        public ActionResult PendingAppoint()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }

            var lab = (LabTable)Session["Lab"];
            var pendingappointment = db.LabAppointTables.Where(d => d.LabID == lab.LabID && d.IsComplete == false && d.IsFeeSubmit == false);
            return View(pendingappointment);
        }

        public ActionResult CompleteAppointment()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }

            var lab = (LabTable)Session["Lab"];
            var completeappointment = db.LabAppointTables.Where(d => d.LabID == lab.LabID && d.IsComplete == true && d.IsFeeSubmit == true);
            return View(completeappointment);
        }

        public ActionResult CurrentAppointment()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }

            var lab = (LabTable)Session["Lab"];
            var currentappointment = db.LabAppointTables.Where(d => d.LabID == lab.LabID && d.IsComplete == false && d.IsFeeSubmit == true);
            return View(currentappointment);
        }
        public ActionResult AllAppoint()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }

            var lab = (LabTable)Session["Lab"];
            var allappointment = db.LabAppointTables.Where(d => d.LabID == lab.LabID);
            return View(allappointment);
        }

        public ActionResult ChangeStatus(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }

            var appoint = db.LabAppointTables.Find(id);
            ViewBag.LabTimeSlotID = new SelectList(db.LabTimeSlotTables.Where(d => d.LabID == appoint.LabID), "LabTimeSlotID", "Name", appoint.LabTimeSlotID); 
            return View(appoint);
        }
        [HttpPost]
        public ActionResult ChangeStatus(LabAppointTable app)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }

            if (ModelState.IsValid)
            {
                db.Entry(app).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("PendingAppoint");
            }
            ViewBag.LabTimeSlotID = new SelectList(db.LabTimeSlotTables.Where(d => d.LabID == app.LabID), "LabTimeSlotID", "LabTimeSlotID", app.LabTimeSlotID);
            return View(app);
        }

        public ActionResult ProcessAppointment(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            List<PatientAppointmentMV> detaillist = new List<PatientAppointmentMV>();
            var appoint = db.LabAppointTables.Find(id);
            var testdetails = db.LabTestDetailsTables.Where(p => p.LabTestID == appoint.LabTestID);
            foreach (var item in testdetails)
            {
                var details = new PatientAppointmentMV()
                {
                    DetailName = item.Name,
                    LabAppointID = appoint.LabAppointID,
                    LabTestDetailID = item.LabTestDetailID,
                    MaxValue = item.MaxValue,
                    MinValue=item.MinValue,
                    PatientValue = 0
                };
                detaillist.Add(details);
            }
            ViewBag.TestName = appoint.LabTestTable.Name;

            return View(detaillist);
            
        }
    }
}