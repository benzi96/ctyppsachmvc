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
    public class doanhthuController : Controller
    {
        private ctyppsachEntities db = new ctyppsachEntities();

        // GET: doanhthu
        public ActionResult Index(DateTime startdate, DateTime enddate)
        {
            doanhthuviewmodel dtvm = new doanhthuviewmodel();
            dtvm.startdate = startdate;
            dtvm.enddate = enddate;
            dtvm.ctdt = db.ctdmsdb
                        .Include(c => c.danhmucsachdaban)
                        .Include(c => c.sach)
                        .Where(o => o.danhmucsachdaban.thoigian > startdate && o.danhmucsachdaban.thoigian < enddate) //tim trong danh sach sach da ban trong khoang thoi gian
                        .Select(c => new chitietdoanhthu(c)).ToList();
            dtvm.doanhthu = tinhdoanhthu(dtvm.ctdt);
            return View(dtvm);
        }

        private decimal tinhdoanhthu(List<chitietdoanhthu> ctdt)
        {
            decimal tongdoanhthu = 0;
            foreach(chitietdoanhthu ct in ctdt)
            {
                tongdoanhthu += ct.loinhuan;
            }
            return tongdoanhthu;
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
