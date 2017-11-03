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
        public ActionResult Index(string thoidiem, string iddl)
        {
            ViewBag.iddl = new SelectList(db.daily, "iddl", "tendl");
            dailycongnoviewmodel dailys = new dailycongnoviewmodel();
            dailys.dt = DateTime.Now.Date;
            DateTime searchDate;
            int iddaily = 0;
            if (DateTime.TryParse(thoidiem, out searchDate))
            {
                List<daily> dls = new List<daily>();
                if (int.TryParse(iddl, out iddaily))
                    dls = db.daily.Where(o => o.iddl == iddaily).ToList();
                else dls = db.daily.ToList();

                foreach (daily o in dls)
                {
                    decimal tonggiaxuat = (decimal)db.ctpx.Where(ct => ct.phieuxuat.iddl == o.iddl && ct.phieuxuat.ngayxuat > searchDate)
                                                  .Select(ct => ct.soluong*ct.sach.giaxuat)
                                                  .DefaultIfEmpty(0)
                                                  .Sum();
                    decimal tongsotientra = (decimal)db.ctdmsdb.Where(ct => ct.danhmucsachdaban.iddl == o.iddl && ct.danhmucsachdaban.thoigian > searchDate)
                                                  .Select(ct => ct.soluong*ct.sach.giaxuat)
                                                  .DefaultIfEmpty(0)
                                                  .Sum();

                    o.congno = o.congno - tonggiaxuat + tongsotientra;
                }
                dailys.daily = dls;
                return View(dailys);
            }
            
            dailys.daily = db.daily.ToList();
            return View(dailys);
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
