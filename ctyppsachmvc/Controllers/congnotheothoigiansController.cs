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
    public class congnotheothoigiansController : Controller
    {
        private ctyppsachEntities db = new ctyppsachEntities();

        // GET: congnotheothoigians
        public ActionResult Index()
        {
            var congnotheothoigian = db.congnotheothoigian.Include(c => c.daily);
            return View(congnotheothoigian.ToList());
        }

        // GET: congnotheothoigians/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            congnotheothoigian congnotheothoigian = db.congnotheothoigian.Find(id);
            if (congnotheothoigian == null)
            {
                return HttpNotFound();
            }
            return View(congnotheothoigian);
        }

        // GET: congnotheothoigians/Create
        public ActionResult Create()
        {
            ViewBag.iddl = new SelectList(db.daily, "iddl", "tendl");
            return View();
        }

        // POST: congnotheothoigians/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,iddl,congno,thoidiem")] congnotheothoigian congnotheothoigian)
        {
            if (ModelState.IsValid)
            {
                db.congnotheothoigian.Add(congnotheothoigian);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.iddl = new SelectList(db.daily, "iddl", "tendl", congnotheothoigian.iddl);
            return View(congnotheothoigian);
        }

        // GET: congnotheothoigians/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            congnotheothoigian congnotheothoigian = db.congnotheothoigian.Find(id);
            if (congnotheothoigian == null)
            {
                return HttpNotFound();
            }
            ViewBag.iddl = new SelectList(db.daily, "iddl", "tendl", congnotheothoigian.iddl);
            return View(congnotheothoigian);
        }

        // POST: congnotheothoigians/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,iddl,congno,thoidiem")] congnotheothoigian congnotheothoigian)
        {
            if (ModelState.IsValid)
            {
                db.Entry(congnotheothoigian).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.iddl = new SelectList(db.daily, "iddl", "tendl", congnotheothoigian.iddl);
            return View(congnotheothoigian);
        }

        // GET: congnotheothoigians/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            congnotheothoigian congnotheothoigian = db.congnotheothoigian.Find(id);
            if (congnotheothoigian == null)
            {
                return HttpNotFound();
            }
            return View(congnotheothoigian);
        }

        // POST: congnotheothoigians/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            congnotheothoigian congnotheothoigian = db.congnotheothoigian.Find(id);
            db.congnotheothoigian.Remove(congnotheothoigian);
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
