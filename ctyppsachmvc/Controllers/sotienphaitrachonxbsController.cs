using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ctyppsachmvc.Models;

namespace ctyppsachmvc.Controllers
{
    public class sotienphaitrachonxbsController : Controller
    {
        private ctyppsachEntities db = new ctyppsachEntities();

        // GET: sotienphaitrachonxbs
        public ActionResult Index()
        {
            var sotienphaitrachonxb = db.sotienphaitrachonxb.Include(s => s.nxb);
            return View(sotienphaitrachonxb.ToList());
        }

        // GET: sotienphaitrachonxbs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sotienphaitrachonxb sotienphaitrachonxb = db.sotienphaitrachonxb.Find(id);
            if (sotienphaitrachonxb == null)
            {
                return HttpNotFound();
            }
            return View(sotienphaitrachonxb);
        }

        // GET: sotienphaitrachonxbs/Create
        public ActionResult Create()
        {
            ViewBag.idnxb = new SelectList(db.nxb, "idnxb", "tennxb");
            return View();
        }

        // POST: sotienphaitrachonxbs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,idnxb,sotienphaitra")] sotienphaitrachonxb sotienphaitrachonxb)
        {
            if (ModelState.IsValid)
            {
                db.sotienphaitrachonxb.Add(sotienphaitrachonxb);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.idnxb = new SelectList(db.nxb, "idnxb", "tennxb", sotienphaitrachonxb.idnxb);
            return View(sotienphaitrachonxb);
        }

        // GET: sotienphaitrachonxbs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sotienphaitrachonxb sotienphaitrachonxb = db.sotienphaitrachonxb.Find(id);
            if (sotienphaitrachonxb == null)
            {
                return HttpNotFound();
            }
            ViewBag.idnxb = new SelectList(db.nxb, "idnxb", "tennxb", sotienphaitrachonxb.idnxb);
            return View(sotienphaitrachonxb);
        }

        // POST: sotienphaitrachonxbs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,idnxb,sotienphaitra")] sotienphaitrachonxb sotienphaitrachonxb)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sotienphaitrachonxb).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idnxb = new SelectList(db.nxb, "idnxb", "tennxb", sotienphaitrachonxb.idnxb);
            return View(sotienphaitrachonxb);
        }

        // GET: sotienphaitrachonxbs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sotienphaitrachonxb sotienphaitrachonxb = db.sotienphaitrachonxb.Find(id);
            if (sotienphaitrachonxb == null)
            {
                return HttpNotFound();
            }
            return View(sotienphaitrachonxb);
        }

        // POST: sotienphaitrachonxbs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            sotienphaitrachonxb sotienphaitrachonxb = db.sotienphaitrachonxb.Find(id);
            db.sotienphaitrachonxb.Remove(sotienphaitrachonxb);
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
