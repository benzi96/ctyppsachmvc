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
            ViewBag.idsach = new SelectList(db.sach, "idsach", "tensach");
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
                    tonkho tk = new tonkho();
                    tk.thoidiem = DateTime.Now;
                    tonkho tkht = db.tonkho.OrderByDescending(o => o.idtk).FirstOrDefault(o => o.idsach == (int)ct.idsach);
                    //kiem tra xem cuon sach du so luong de xuat ko
                    if (tkht != null && tkht.soluongton > ct.soluong)
                    {
                        tk.idsach = (int)ct.idsach;
                        tk.soluongton = tkht.soluongton - ct.soluong;
                        db.tonkho.Add(tk);
                    }
                    else
                    {
                        ModelState.AddModelError("", "Không đủ số lượng hoặc chưa nhập sách về");
                        ViewBag.iddl = new SelectList(db.daily, "iddl", "tendl", phieuxuat.iddl);
                        ViewBag.idsach = new SelectList(db.sach, "idsach", "tensach");
                        return View();
                    }

                    //cap nhat so sach da gui cho dai ly
                    hangtoncuadaily htdl = db.hangtoncuadaily.FirstOrDefault(o => o.iddl == phieuxuat.iddl && o.idsach == ct.idsach);
                    if(htdl != null)
                    {
                        htdl.soluongchuaban = htdl.soluongchuaban + ct.soluong;
                        db.Entry(htdl).State = EntityState.Modified;
                    }
                    else
                    {
                        htdl = new hangtoncuadaily();
                        htdl.iddl = phieuxuat.iddl;
                        htdl.idsach = ct.idsach;
                        htdl.soluongchuaban = ct.soluong;
                        db.hangtoncuadaily.Add(htdl);
                    }
                }
                phieuxuat.ctpx = ctpx;
                db.phieuxuat.Add(phieuxuat);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.iddl = new SelectList(db.daily, "iddl", "tendl", phieuxuat.iddl);
            return View(phieuxuat);
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
            ViewBag.iddl = new SelectList(db.daily, "iddl", "tendl", phieuxuat.iddl);
            return View(phieuxuat);
        }

        // POST: phieuxuats/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idpx,iddl,nguoinhan,ngayxuat")] phieuxuat phieuxuat)
        {
            if (ModelState.IsValid)
            {
                db.Entry(phieuxuat).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.iddl = new SelectList(db.daily, "iddl", "tendl", phieuxuat.iddl);
            return View(phieuxuat);
        }

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
