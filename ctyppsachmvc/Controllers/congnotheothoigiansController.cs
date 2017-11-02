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
        public ActionResult Index()
        {
            dailycongnoviewmodel dailys = new dailycongnoviewmodel();
            dailys.dt = DateTime.Now.Date;
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
