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
    public class danhmucsachdabansController : Controller
    {
        private ctyppsachEntities db = new ctyppsachEntities();

        // GET: danhmucsachdabans
        public ActionResult Index()
        {
            var danhmucsachdaban = db.danhmucsachdaban.Include(d => d.daily);
            return View(danhmucsachdaban.ToList());
        }
        [HttpPost]
        public ActionResult Index(string iddmsdb)
        {
            int id;
            if (int.TryParse(iddmsdb, out id))
            {
                edittratien(id);
            }
            var danhmucsachdaban = db.danhmucsachdaban.Include(d => d.daily);
            return View(danhmucsachdaban.ToList());
        }

        // GET: danhmucsachdabans/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            danhmucsachdaban danhmucsachdaban = db.danhmucsachdaban.Find(id);
            if (danhmucsachdaban == null)
            {
                return HttpNotFound();
            }
            return View(danhmucsachdaban);
        }

        // GET: danhmucsachdabans/Create
        public ActionResult Create()
        {
            ViewBag.iddl = new SelectList(db.daily, "iddl", "tendl");
            ViewBag.idsach = new SelectList(db.sach, "idsach", "tensach");
            return View();
        }

        // POST: danhmucsachdabans/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Prefix ="danhmucsachdaban")] danhmucsachdaban danhmucsachdaban,
                                   [Bind(Prefix = "ct")] ctdmsdb[] ctdmsdb)
        {
            danhmucsachdabanviewmodel dmvm = new danhmucsachdabanviewmodel();
            if (ModelState.IsValid)
            {
                danhmucsachdaban.tinhtrang = "Waiting";
                danhmucsachdaban.sotiendathanhtoan = 0;
                int iddmsdb = 1;
                if (db.danhmucsachdaban.Any())
                    iddmsdb = db.danhmucsachdaban.Max(o => o.iddmsdb) + 1;
                int idct = 1;
                decimal tongtien = 0;
                foreach (ctdmsdb ct in ctdmsdb)
                {
                    ct.iddmsdb = iddmsdb;
                    ct.idctdmsdb = idct;
                    idct++;

                    hangtoncuadaily htdl = db.hangtoncuadaily.FirstOrDefault(o => o.iddl == danhmucsachdaban.iddl && o.idsach == ct.idsach);
                    //kiem tra xem so luong sach đã bán có lớn hơn số lượng sách xuất cho dai ly do
                    if (htdl != null && htdl.soluongchuaban >= ct.soluong)
                    {
                        htdl.soluongchuaban = htdl.soluongchuaban - ct.soluong;
                        db.Entry(htdl).State = EntityState.Modified;
                    }
                    else
                    {
                        ModelState.AddModelError("", "số sách đã bán lớn hơn số sách xuất cho đại lý");
                        ViewBag.idsach = new SelectList(db.sach, "idsach", "tensach");
                        ViewBag.iddl = new SelectList(db.daily, "iddl", "tendl", danhmucsachdaban.iddl);
                        dmvm.danhmucsachdaban = danhmucsachdaban;
                        return View(dmvm);
                    }
                    tongtien += (decimal)(ct.soluong * htdl.sach.giaxuat);

                    //tim nxb cua cuon sach da duoc ban nay
                    sach s = db.sach.Find(ct.idsach);
                    nxb n = db.nxb.Find(s.idnxb);
                    n.sotienphaitra += ct.soluong * s.gianhap;
                }
                danhmucsachdaban.sotienconno = tongtien;
                danhmucsachdaban.ctdmsdb = ctdmsdb;
                db.danhmucsachdaban.Add(danhmucsachdaban);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idsach = new SelectList(db.sach, "idsach", "tensach");
            ViewBag.iddl = new SelectList(db.daily, "iddl", "tendl", danhmucsachdaban.iddl);
            dmvm.danhmucsachdaban = danhmucsachdaban;
            return View(dmvm);
        }

        // GET: danhmucsachdabans/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            danhmucsachdaban danhmucsachdaban = db.danhmucsachdaban.Find(id);
            if (danhmucsachdaban == null)
            {
                return HttpNotFound();
            }
            
            ViewBag.idsach = new SelectList(db.sach, "idsach", "tensach");
            danhmucsachdabanviewmodel dmvm = new danhmucsachdabanviewmodel();

            ViewBag.iddl = new SelectList(db.daily, "iddl", "tendl", danhmucsachdaban.iddl);
            dmvm.danhmucsachdaban = danhmucsachdaban;
            return View(dmvm);
        }

        // POST: danhmucsachdabans/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Prefix = "danhmucsachdaban")] danhmucsachdaban danhmucsachdaban,
                                 [Bind(Prefix = "ct")] ctdmsdb[] ctdmsdb)
        {
            ViewBag.iddl = new SelectList(db.daily, "iddl", "tendl", danhmucsachdaban.iddl);
            ViewBag.idsach = new SelectList(db.sach, "idsach", "tensach");
            danhmucsachdabanviewmodel dmvm = new danhmucsachdabanviewmodel();

            if (ModelState.IsValid)
            {
                int iddmsdb = danhmucsachdaban.iddmsdb;
                int idct = 1;
                //xoa chi tiet cu trong database table hangtoncuadaily
                var ctcudmsdb = db.ctdmsdb.Where(o => o.iddmsdb == danhmucsachdaban.iddmsdb);
                foreach (ctdmsdb ct in ctcudmsdb)
                {
                    hangtoncuadaily ht = db.hangtoncuadaily.FirstOrDefault(o => o.iddl == danhmucsachdaban.iddl && o.idsach == ct.idsach);
                    int hangtondaily = (int)(ht.soluongchuaban + ct.soluong);
                }

                //thêm chi tiết sửa vào database table hangtoncuadaily
                foreach (ctdmsdb ct in ctdmsdb)
                {
                    ct.iddmsdb = iddmsdb;
                    ct.iddmsdb = idct;
                    idct++;
                    hangtoncuadaily ht = db.hangtoncuadaily.FirstOrDefault(o => o.iddl == danhmucsachdaban.iddl && o.idsach == ct.idsach);
                    ht.soluongchuaban = (int)(ht.soluongchuaban - ct.soluong);
                    if (ht.soluongchuaban < 0)
                    {
                        danhmucsachdaban.ctdmsdb = ctdmsdb;
                        dmvm.danhmucsachdaban = danhmucsachdaban;
                        return View(dmvm);
                    }
                    nxb n = db.nxb.Find(ct.sach.idnxb);
                    n.sotienphaitra += ct.soluong * ct.sach.gianhap;
                }
                foreach (ctdmsdb ct in ctcudmsdb)
                {
                    nxb n = db.nxb.Find(ct.sach.idnxb);
                    n.sotienphaitra -= ct.soluong * ct.sach.gianhap;
                    if (n.sotienphaitra < 0)
                    {
                        danhmucsachdaban.ctdmsdb = ctdmsdb;
                        dmvm.danhmucsachdaban = danhmucsachdaban;
                        return View(dmvm);
                    }
                    db.ctdmsdb.Remove(ct);
                }

                db.Entry(danhmucsachdaban).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            danhmucsachdaban.ctdmsdb = ctdmsdb;
            dmvm.danhmucsachdaban = danhmucsachdaban;
            return View(dmvm);
        }

        private void edittratien(int iddmsdb)
        {
            danhmucsachdaban dm = db.danhmucsachdaban.Find(iddmsdb);
            dm.sotiendathanhtoan = db.ctdmsdb.Where(ct => ct.iddmsdb == iddmsdb).Select(ct => ct.soluong * ct.sach.giaxuat).DefaultIfEmpty(0).Sum();
            dm.sotienconno = 0;
            dm.tinhtrang = "Completed";
            db.SaveChanges();
        }
        //// GET: danhmucsachdabans/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    danhmucsachdaban danhmucsachdaban = db.danhmucsachdaban.Find(id);
        //    if (danhmucsachdaban == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(danhmucsachdaban);
        //}

        //// POST: danhmucsachdabans/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    danhmucsachdaban danhmucsachdaban = db.danhmucsachdaban.Find(id);
        //    db.danhmucsachdaban.Remove(danhmucsachdaban);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

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
