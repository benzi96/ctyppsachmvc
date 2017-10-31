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
                List<tonkho> tks = new List<tonkho>();
                DateTime tomorrowdate = searchDate.AddDays(1);
                var tonkhoes = db.tonkho.Include(c => c.sach)
                    .OrderByDescending(o => o.idtk)
                    .FirstOrDefault(h => h.thoidiem <= tomorrowdate);

                tks.Add(tonkhoes);
                return View(tks);
                // do not use .Equals() which can not be converted to SQL
            }

            var sach = db.sach.Include(t => t.nxb);
            return View(sach.ToList());
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
