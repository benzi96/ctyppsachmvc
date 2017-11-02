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
    public class phieuxuatsController : Controller
    {
        private ctyppsachEntities db = new ctyppsachEntities();

        // GET: phieuxuats
        public ActionResult Index()
        {
            var phieuxuat = db.phieuxuat.Include(p => p.daily);
            return View(phieuxuat.ToList());
        }

        // GET: phieuxuats/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            phieuxuat phieuxuat = db.phieuxuat.Find(id);
            if (phieuxuat == null)
            {
                return HttpNotFound();
            }
            return View(phieuxuat);
        }

        // GET: phieuxuats/Create
        public ActionResult Create()
        {
            ViewBag.iddl = new SelectList(db.daily, "iddl", "tendl");
            ViewBag.idsach = new SelectList(db.sach.Where(s => s.soluongton != null), "idsach", "tensach");
            return View();
        }

        // POST: phieuxuats/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Prefix ="phieuxuat")] phieuxuat phieuxuat,
                                   [Bind(Prefix = "ct")] ctpx[] ctpx)
        {
            if (ModelState.IsValid)
            {
                decimal tongtien = 0;
                int idpx = 1;
                if (db.phieuxuat.Any())
                    idpx = db.phieuxuat.Max(o => o.idpx) + 1;
                int idct = 1;
                foreach (ctpx ct in ctpx)
                {
                    ct.idpx = idpx;
                    ct.idctpx = idct;
                    idct++;

                    //cap nhat ton kho hien tai
                    sach a = db.sach.Find(ct.idsach);

                    //kiem tra xem cuon sach du so luong de xuat ko
                    if (a.soluongton > ct.soluong) a.soluongton = a.soluongton - ct.soluong;
                    else
                    {
                        ModelState.AddModelError("", "Không đủ số lượng hoặc chưa nhập sách về");
                        phieuxuat.ctpx = ctpx;
                        return re(phieuxuat);
                    }

                    //cap nhat so sach da gui cho dai ly
                    hangtoncuadaily htdl = db.hangtoncuadaily.FirstOrDefault(o => o.iddl == phieuxuat.iddl && o.idsach == ct.idsach);
                    if(htdl != null && htdl.soluongchuaban != null)
                    {
                        htdl.soluongchuaban = htdl.soluongchuaban + ct.soluong;
                    }
                    else
                    {
                        htdl = new hangtoncuadaily();
                        htdl.iddl = phieuxuat.iddl;
                        htdl.idsach = ct.idsach;
                        htdl.soluongchuaban = ct.soluong;
                        db.hangtoncuadaily.Add(htdl);
                    }
                    tongtien += (decimal)(ct.soluong * db.sach.Find(ct.idsach).giaxuat);
                }

                //cap nhat cong no
                congnotheothoigian cn = new congnotheothoigian();
                cn.iddl = phieuxuat.iddl;
                cn.thoidiem = (DateTime)phieuxuat.ngayxuat;

                congnotheothoigian cnht = db.congnotheothoigian.OrderByDescending(o => o.thoidiem).FirstOrDefault(o => o.iddl == phieuxuat.iddl);
                if (tongtien > cnht.congno)
                {
                    phieuxuat.ctpx = ctpx;
                    return re(phieuxuat);
                }
                
                if (cnht != null && cnht.congno != null) cn.congno = cnht.congno + tongtien;
                else cn.congno = tongtien;

                db.congnotheothoigian.Add(cn);

                phieuxuat.ctpx = ctpx;
                db.phieuxuat.Add(phieuxuat);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            phieuxuat.ctpx = ctpx;
            return re(phieuxuat);
        }

        // GET: phieuxuats/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            phieuxuat phieuxuat = db.phieuxuat.Find(id);
            if (phieuxuat == null)
            {
                return HttpNotFound();
            }
            return re(phieuxuat);
        }

        // POST: phieuxuats/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Prefix = "phieuxuat")] phieuxuat phieuxuat,
                                 [Bind(Prefix = "ct")] ctpx[] ctpx)
        {
            if (ModelState.IsValid)
            {
                int idpx = phieuxuat.idpx;
                int idct = 1;

                //xóa chi tiết cũ trong database table sach
                //tinh tong tien cu
                int tongtiencu = 0;
                var ctpxcu = db.ctpx.Where(ct => ct.idpx == phieuxuat.idpx).ToList();
                foreach (var ct in ctpxcu)
                {
                    sach s = db.sach.Find(ct.idsach);
                    int soluonghientai = (int)(s.soluongton + ct.soluong);
                    s.soluongton = soluonghientai;
                    tongtiencu += (int)(ct.soluong * s.giaxuat);
                }
                //thêm chi tiết sửa vào database table hangtoncuadaily
                foreach (ctpx ct in ctpx)
                {
                    ct.idpx = idpx;
                    ct.idctpx = idct;
                    idct++;
                    hangtoncuadaily ht = db.hangtoncuadaily.FirstOrDefault(o => o.iddl == phieuxuat.iddl && o.idsach == ct.idsach);
                    ht.soluongchuaban = (int)(ht.soluongchuaban + ct.soluong);
                }
                //xoa chi tiet cu trong database table hangtoncuadaily
                foreach (ctpx ct in ctpxcu)
                {
                    hangtoncuadaily ht = db.hangtoncuadaily.FirstOrDefault(o => o.iddl == phieuxuat.iddl && o.idsach == ct.idsach);
                    int hangtondaily = (int)(ht.soluongchuaban - ct.soluong);
                    if (hangtondaily < 0)
                    {
                        ModelState.AddModelError("", "so sach nay da duoc ban" + hangtondaily);
                        phieuxuat.ctpx = ctpx;
                        return re(phieuxuat);
                    }
                    db.ctpx.Remove(ct);
                }
                //tinh tong tien moi
                int tongtien = 0;
                //thêm chi tiết sửa vào database table sach
                foreach (ctpx ct in ctpx)
                {
                    sach s = db.sach.Find(ct.idsach);
                    if (s.soluongton != null) s.soluongton = s.soluongton - ct.soluong;
                    if (s.soluongton < 0)
                    {
                        ModelState.AddModelError("", "so luong hien tai khong du de xuat " + s.soluongton +" vui long kiem tra lai");
                        phieuxuat.ctpx = ctpx;
                        return re(phieuxuat);
                    }
                    tongtien += (int)(ct.soluong * s.giaxuat);
                }

                foreach (ctpx ct in ctpx)
                {
                    db.ctpx.Add(ct);
                }

                
                db.Entry(phieuxuat).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            phieuxuat.ctpx = ctpx;
            return re(phieuxuat);
        }

        /* delete
        // GET: phieuxuats/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            phieuxuat phieuxuat = db.phieuxuat.Find(id);
            if (phieuxuat == null)
            {
                return HttpNotFound();
            }
            return View(phieuxuat);
        }

        // POST: phieuxuats/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            phieuxuat phieuxuat = db.phieuxuat.Find(id);
            db.phieuxuat.Remove(phieuxuat);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        */

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private ActionResult re(phieuxuat px)
        {
            ViewBag.iddl = new SelectList(db.daily, "iddl", "tendl", px.iddl);
            ViewBag.idsach = new SelectList(db.sach.Where(s => s.soluongton != null), "idsach", "tensach");
            phieuxuatviewmodel pxvm1 = new phieuxuatviewmodel();
            pxvm1.phieuxuat = px;
            return View(pxvm1);
        }
    }
}
