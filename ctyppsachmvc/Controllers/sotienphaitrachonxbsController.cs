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
    public class sotienphaitrachonxbsController : Controller
    {
        private ctyppsachEntities db = new ctyppsachEntities();

        // GET: sotienphaitrachonxbs
        public ActionResult Index(string thoidiem, string idnxb)
        {
            nxbsotienphaitraviewmodel nxbs = new nxbsotienphaitraviewmodel();
            nxbs.dt = DateTime.Now.Date;
            DateTime searchDate;
            int idnxban = 0;
            if (DateTime.TryParse(thoidiem, out searchDate))
            {
                List<nxb> nxbans = new List<nxb>();
                if (int.TryParse(idnxb, out idnxban))
                    nxbans = db.nxb.Where(o => o.idnxb == idnxban).ToList();
                else nxbans = db.nxb.ToList();

                foreach (nxb o in nxbans)
                {
                    decimal tongsotienphaitra = (decimal)db.ctdmsdb.Where(ct => ct.sach.idnxb == o.idnxb && ct.danhmucsachdaban.thoigian > searchDate)
                                                  .Select(ct => ct.soluong * ct.sach.gianhap)
                                                  .DefaultIfEmpty(0)
                                                  .Sum();
                    decimal tongsotiendatra = (decimal)db.phieutratien.Where(ct => ct.idnxb == o.idnxb && ct.ngaytaophieu > searchDate)
                                                  .Select(ct => ct.sotientra)
                                                  .DefaultIfEmpty(0)
                                                  .Sum();

                    o.sotienphaitra = o.sotienphaitra - tongsotienphaitra + tongsotiendatra;
                }
                nxbs.nxb = nxbans;
                return View(nxbs);
            }
            nxbs.nxb = db.nxb.ToList();
            return View(nxbs);
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
