using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DatabaseLayer;

namespace OnlineDiagnosticSystem.Controllers
{
    public class LabAppointStatusController : Controller
    {
        private OnlineDiagnosticLabSystemDbEntities db = new OnlineDiagnosticLabSystemDbEntities();
        // GET: DoctorAppointmentStatus
        public ActionResult PendingAppoint()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }

            var patient = (PatientTable)Session["Patient"];
            var pendingappointment = db.LabAppointTables.Where(d => d.PatientID == patient.PatientID && d.IsComplete == false && d.IsFeeSubmit == false);
            return View(pendingappointment);
        }


        public ActionResult CurrentAppoint()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }

            var patient = (PatientTable)Session["Patient"];
            var currentappointment = db.LabAppointTables.Where(d => d.PatientID == patient.PatientID && d.IsComplete == false && d.IsFeeSubmit == true);
            return View(currentappointment);
        }

        public ActionResult CompleteAppoint()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }

            var patient = (PatientTable)Session["Patient"];
            var completeappointment = db.LabAppointTables.Where(d => d.PatientID == patient.PatientID && d.IsComplete == true && d.IsFeeSubmit == true);
            return View(completeappointment);
        }

        public ActionResult CancelAppoint()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }

            var patient = (PatientTable)Session["Patient"];
            var canceleappointment = db.LabAppointTables.Where(d => d.PatientID == patient.PatientID && (d.IsComplete == false || d.IsFeeSubmit == false )&& d.AppointDate< DateTime.Now);
            return View(canceleappointment);
        }


        public ActionResult AllAppoint()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }

            var patient = (PatientTable)Session["Patient"];
            var allappointment = db.LabAppointTables.Where(d => d.PatientID == patient.PatientID);
            return View(allappointment);
        }
    }
}