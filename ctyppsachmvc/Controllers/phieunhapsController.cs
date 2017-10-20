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
    public class phieunhapsController : Controller
    {
        private ctyppsachEntities db = new ctyppsachEntities();

        // GET: phieunhaps
        public ActionResult Index()
        {
            var phieunhap = db.phieunhap.Include(p => p.nxb);
            return View(phieunhap.ToList());
        }

        // GET: phieunhaps/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            phieunhap phieunhap = db.phieunhap.Find(id);
            if (phieunhap == null)
            {
                return HttpNotFound();
            }
            return View(phieunhap);
        }

        // GET: phieunhaps/Create
        public ActionResult Create()
        {
            ViewBag.idnxb = new SelectList(db.nxb, "idnxb", "tennxb");
            ViewBag.idsach = new SelectList(db.sach, "idsach", "tensach");
            return View();
        }

        // POST: phieunhaps/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Prefix = "phieunhap")] phieunhap phieunhap, 
                                   [Bind(Prefix="ct")] ctpn[] ctpn)
        {
            if (ModelState.IsValid)
            {
                int idpn = 1;
                if (db.phieunhap.Any())
                     idpn = db.phieunhap.Max(o => o.idpn) + 1;
                int idct = 1;
                foreach (ctpn ct in ctpn)
                {
                    ct.idpn = idpn;
                    ct.idctpn = idct;
                    idct++;
                    tonkho tk = new tonkho();
                    tk.thoidiem = DateTime.Now;

                    //lấy tồn kho mới nhất của sách này
                    tonkho tkht = db.tonkho.OrderByDescending(o => o.idtk).FirstOrDefault(o => o.idsach == (int)ct.idsach);
                    
                    if (tkht != null)
                    {
                        tk.idsach = (int)ct.idsach;
                        tk.soluongton = tkht.soluongton + ct.soluong;
                    }
                    else
                    {
                        tk.idsach = (int)ct.idsach;
                        tk.soluongton = ct.soluong;
                    }
                    db.tonkho.Add(tk);
                }
                phieunhap.ctpn = ctpn;
                db.phieunhap.Add(phieunhap);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.idnxb = new SelectList(db.nxb, "idnxb", "tennxb", phieunhap.idnxb);
            return View(phieunhap);
        }

        // GET: phieunhaps/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            phieunhap phieunhap = db.phieunhap.Find(id);
            if (phieunhap == null)
            {
                return HttpNotFound();
            }
            ViewBag.idnxb = new SelectList(db.nxb, "idnxb", "tennxb", phieunhap.idnxb);
            return View(phieunhap);
        }

        // POST: phieunhaps/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idpn,idnxb,nguoigiaosach,ngaynhap,nguoivietphieu")] phieunhap phieunhap)
        {
            if (ModelState.IsValid)
            {
                db.Entry(phieunhap).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idnxb = new SelectList(db.nxb, "idnxb", "tennxb", phieunhap.idnxb);
            return View(phieunhap);
        }

        // GET: phieunhaps/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            phieunhap phieunhap = db.phieunhap.Find(id);
            if (phieunhap == null)
            {
                return HttpNotFound();
            }
            return View(phieunhap);
        }

        // POST: phieunhaps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            phieunhap phieunhap = db.phieunhap.Find(id);
            db.phieunhap.Remove(phieunhap);
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
