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
    public class tonkhoesController : Controller
    {
        private ctyppsachEntities db = new ctyppsachEntities();

        // GET: tonkhoes
        public ActionResult Index(string thoidiem, string ids)
        {
            ViewBag.idsach = new SelectList(db.sach, "idsach", "tensach");
            DateTime searchDate;
            int idsach = 0;
            if (DateTime.TryParse(thoidiem, out searchDate))
            {
                List<sach> saches = new List<sach>();
                if (int.TryParse(ids, out idsach))
                    saches = db.sach.Where(o => o.idsach == idsach).Include(t => t.nxb).ToList();
                else saches = db.sach.Include(t => t.nxb).ToList();

                foreach (sach o in saches)
                {
                    int soluongnhap = (int)db.ctpn.Where(ct => ct.idsach == o.idsach && ct.phieunhap.ngaynhap > searchDate)
                                                  .Select(ct => ct.soluong)
                                                  .DefaultIfEmpty(0)
                                                  .Sum();
                    int soluongxuat = (int)db.ctpx.Where(ct => ct.idsach == o.idsach && ct.phieuxuat.ngayxuat > searchDate)
                                                  .Select(ct => ct.soluong)
                                                  .DefaultIfEmpty(0)
                                                  .Sum();

                    o.soluongton = o.soluongton + soluongxuat - soluongnhap;
                }
                return View(saches);
            }
            
            var s = db.sach.Include(t => t.nxb);
            return View(s.ToList());
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
