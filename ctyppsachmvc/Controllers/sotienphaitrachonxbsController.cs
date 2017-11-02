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
        public ActionResult Index()
        {
            nxbsotienphaitraviewmodel nxbs = new nxbsotienphaitraviewmodel();
            nxbs.nxb = db.nxb.ToList();
            nxbs.dt = DateTime.Now.Date;
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
