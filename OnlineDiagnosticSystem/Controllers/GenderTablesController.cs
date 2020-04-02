using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DatabaseLayer;

namespace OnlineDiagnosticSystem.Controllers
{
    public class GenderTablesController : Controller
    {
        private OnlineDiagnosticLabSystemDbEntities db = new OnlineDiagnosticLabSystemDbEntities();

        // GET: GenderTables
        public ActionResult Index()
        {
            return View(db.GenderTables.ToList());
        }

        // GET: GenderTables/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GenderTable genderTable = db.GenderTables.Find(id);
            if (genderTable == null)
            {
                return HttpNotFound();
            }
            return View(genderTable);
        }

        // GET: GenderTables/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: GenderTables/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "GenderID,Name")] GenderTable genderTable)
        {
            if (ModelState.IsValid)
            {
                db.GenderTables.Add(genderTable);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(genderTable);
        }

        // GET: GenderTables/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GenderTable genderTable = db.GenderTables.Find(id);
            if (genderTable == null)
            {
                return HttpNotFound();
            }
            return View(genderTable);
        }

        // POST: GenderTables/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "GenderID,Name")] GenderTable genderTable)
        {
            if (ModelState.IsValid)
            {
                db.Entry(genderTable).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(genderTable);
        }

        // GET: GenderTables/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GenderTable genderTable = db.GenderTables.Find(id);
            if (genderTable == null)
            {
                return HttpNotFound();
            }
            return View(genderTable);
        }

        // POST: GenderTables/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            GenderTable genderTable = db.GenderTables.Find(id);
            db.GenderTables.Remove(genderTable);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
