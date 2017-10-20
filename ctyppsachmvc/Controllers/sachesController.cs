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
    public class sachesController : Controller
    {
        private ctyppsachEntities db = new ctyppsachEntities();

        // GET: saches
        public ActionResult Index()
        {
            var sach = db.sach.Include(s => s.linhvuc).Include(s => s.nxb);
            return View(sach.ToList());
        }

        // GET: saches/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sach sach = db.sach.Find(id);
            if (sach == null)
            {
                return HttpNotFound();
            }
            return View(sach);
        }

        // GET: saches/Create
        public ActionResult Create()
        {
            ViewBag.idlv = new SelectList(db.linhvuc, "idlv", "tenlv");
            ViewBag.idnxb = new SelectList(db.nxb, "idnxb", "tennxb");
            return View();
        }

        // POST: saches/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idsach,tensach,tacgia,idnxb,idlv,soluong,giaxuat,gianhap")] sach sach)
        {
            if (ModelState.IsValid)
            {
                db.sach.Add(sach);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.idlv = new SelectList(db.linhvuc, "idlv", "tenlv", sach.idlv);
            ViewBag.idnxb = new SelectList(db.nxb, "idnxb", "tennxb", sach.idnxb);
            return View(sach);
        }

        // GET: saches/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sach sach = db.sach.Find(id);
            if (sach == null)
            {
                return HttpNotFound();
            }
            ViewBag.idlv = new SelectList(db.linhvuc, "idlv", "tenlv", sach.idlv);
            ViewBag.idnxb = new SelectList(db.nxb, "idnxb", "tennxb", sach.idnxb);
            return View(sach);
        }

        // POST: saches/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idsach,tensach,tacgia,idnxb,idlv,soluong,giaxuat,gianhap")] sach sach)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sach).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idlv = new SelectList(db.linhvuc, "idlv", "tenlv", sach.idlv);
            ViewBag.idnxb = new SelectList(db.nxb, "idnxb", "tennxb", sach.idnxb);
            return View(sach);
        }

        // GET: saches/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sach sach = db.sach.Find(id);
            if (sach == null)
            {
                return HttpNotFound();
            }
            return View(sach);
        }

        // POST: saches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            sach sach = db.sach.Find(id);
            db.sach.Remove(sach);
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
