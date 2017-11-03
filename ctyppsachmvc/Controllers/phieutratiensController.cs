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
    public class phieutratiensController : Controller
    {
        private ctyppsachEntities db = new ctyppsachEntities();

        // GET: phieutratiens
        public ActionResult Index()
        {
            var phieutratien = db.phieutratien.Include(p => p.nxb);
            return View(phieutratien.ToList());
        }

        // GET: phieutratiens/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            phieutratien phieutratien = db.phieutratien.Find(id);
            if (phieutratien == null)
            {
                return HttpNotFound();
            }
            return View(phieutratien);
        }

        // GET: phieutratiens/Create
        public ActionResult Create()
        {
            ViewBag.idnxb = new SelectList(db.nxb, "idnxb", "tennxb");
            return View();
        }

        // POST: phieutratiens/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idptt,idnxb,sotientra,tinhtrang")] phieutratien phieutratien)
        {
            if (ModelState.IsValid)
            {
                nxb n = db.nxb.Find(phieutratien.idnxb);
                n.sotienphaitra -= phieutratien.sotientra;
                db.phieutratien.Add(phieutratien);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.idnxb = new SelectList(db.nxb, "idnxb", "tennxb", phieutratien.idnxb);
            return View(phieutratien);
        }

        // GET: phieutratiens/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            phieutratien phieutratien = db.phieutratien.Find(id);
            if (phieutratien == null)
            {
                return HttpNotFound();
            }
            ViewBag.idnxb = new SelectList(db.nxb, "idnxb", "tennxb", phieutratien.idnxb);
            return View(phieutratien);
        }

        // POST: phieutratiens/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idptt,idnxb,sotientra,tinhtrang")] phieutratien phieutratien)
        {
            if (ModelState.IsValid)
            {
                phieutratien pttcu = db.phieutratien.Find(phieutratien.idptt);
                db.Entry(pttcu).State = EntityState.Detached;

                nxb n = db.nxb.Find(phieutratien.idnxb);
                n.sotienphaitra = n.sotienphaitra - phieutratien.sotientra + pttcu.sotientra;

                db.Entry(phieutratien).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idnxb = new SelectList(db.nxb, "idnxb", "tennxb", phieutratien.idnxb);
            return View(phieutratien);
        }

        // GET: phieutratiens/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            phieutratien phieutratien = db.phieutratien.Find(id);
            if (phieutratien == null)
            {
                return HttpNotFound();
            }
            return View(phieutratien);
        }

        // POST: phieutratiens/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            phieutratien phieutratien = db.phieutratien.Find(id);
            db.phieutratien.Remove(phieutratien);
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
