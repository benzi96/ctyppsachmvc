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
        public ActionResult Index(string thoidiem, int idsach)
        {

            DateTime searchDate;

            if (DateTime.TryParse(thoidiem, out searchDate))
            {
                List<sach> sach = new List<sach>();
                var ctpn = db.ctpn.Where(ct => ct.idsach == idsach && ct.phieunhap.ngaynhap > searchDate);
                int soluongnhap = (int)ctpn.Sum(ct => ct.soluong);
                var ctpx = db.ctpx.Where(ct => ct.idsach == idsach && ct.phieuxuat.ngayxuat > searchDate);
                int soluongxuat = (int)ctpx.Sum(ct => ct.soluong);
                int soluongtonhientai = (int)db.sach.Find(idsach).soluongton;
                int soluongtonquakhu = soluongtonhientai + soluongxuat - soluongnhap;

                sach a = db.sach.Find(idsach);
                sach.Add(a);
                return View(sach);
                // do not use .Equals() which can not be converted to SQL
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
