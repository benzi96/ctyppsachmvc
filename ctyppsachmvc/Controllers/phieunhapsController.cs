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
                    sach s = db.sach.Find(ct.idsach);
                    if (s.soluongton != null) s.soluongton = s.soluongton + ct.soluong;
                    else s.soluongton = ct.soluong;
                }
                phieunhap.ctpn = ctpn;
                TimeSpan time = DateTime.Now.TimeOfDay;
                phieunhap.ngaynhap = phieunhap.ngaynhap + time;
                db.phieunhap.Add(phieunhap);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.idnxb = new SelectList(db.nxb, "idnxb", "tennxb", phieunhap.idnxb);
            ViewBag.idsach = new SelectList(db.sach, "idsach", "tensach");
            phieunhap.ctpn = ctpn;
            phieunhapviewmodel pnvm = new phieunhapviewmodel();
            pnvm.phieunhap = phieunhap;
            return View(pnvm);
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
            ViewBag.idsach = new SelectList(db.sach, "idsach", "tensach");
            ViewBag.idnxb = new SelectList(db.nxb, "idnxb", "tennxb", phieunhap.idnxb);
            phieunhapviewmodel pnvm = new phieunhapviewmodel();
            pnvm.phieunhap = phieunhap;
            return View(pnvm);
        }

        // POST: phieunhaps/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Prefix = "phieunhap")] phieunhap phieunhap,
                                 [Bind(Prefix = "ct")] ctpn[] ctpn)
        {
            if (ModelState.IsValid)
            {
                int idpn = phieunhap.idpn;
                int idct = 1;

                //thêm chi tiết sửa vào database
                foreach (ctpn ct in ctpn)
                {
                    ct.idpn = idpn;
                    ct.idctpn = idct;
                    idct++;
                    sach s = db.sach.Find(ct.idsach);
                    if (s.soluongton != null) s.soluongton = s.soluongton + ct.soluong;
                    else s.soluongton = ct.soluong;
                }

                //xóa chi tiết cũ trong database
                var ctpncu = db.ctpn.Where(ct => ct.idpn == phieunhap.idpn).ToList();
                foreach(var ct in ctpncu)
                {
                    sach s = db.sach.Find(ct.idsach);
                    int soluonghientai = (int)(s.soluongton - ct.soluong);
                    if (soluonghientai < 0)
                    {
                        ViewBag.idsach = new SelectList(db.sach, "idsach", "tensach");
                        ViewBag.idnxb = new SelectList(db.nxb, "idnxb", "tennxb", phieunhap.idnxb);
                        phieunhapviewmodel pnvm = new phieunhapviewmodel();
                        phieunhap.ctpn = ctpn;
                        pnvm.phieunhap = phieunhap;
                        return View(pnvm);
                    }
                    s.soluongton = soluonghientai;
                    db.ctpn.Remove(ct);
                }
                foreach (ctpn ct in ctpn)
                {
                    db.ctpn.Add(ct);
                }
                db.Entry(phieunhap).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idsach = new SelectList(db.sach, "idsach", "tensach");
            ViewBag.idnxb = new SelectList(db.nxb, "idnxb", "tennxb", phieunhap.idnxb);
            phieunhapviewmodel pnvm1 = new phieunhapviewmodel();
            phieunhap.ctpn = ctpn;
            pnvm1.phieunhap = phieunhap;
            return View(pnvm1);
        }
        /* delele
        //// GET: phieunhaps/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    phieunhap phieunhap = db.phieunhap.Find(id);
        //    if (phieunhap == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(phieunhap);
        //}

        //// POST: phieunhaps/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    phieunhap phieunhap = db.phieunhap.Find(id);
        //    db.phieunhap.Remove(phieunhap);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}
        */

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
