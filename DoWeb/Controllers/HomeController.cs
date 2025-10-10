using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.Entity;
using DoWeb.Models;
using System.IO;
using System.Net;

namespace DoWeb.Controllers
{
    public class HomeController : Controller
    {
        private ProjectDBEntities db = new ProjectDBEntities();

        public ActionResult Index()
        {
            var dskhuonep = db.KHUONEPs
                .Include(p => p.MayKhuon).Include(p => p.MayKhuon1)
                .ToList();
            return View(dskhuonep);
        }
        public ActionResult SearchMayKhuon(string searchString)
        {
            if (string.IsNullOrEmpty(searchString))
            {
                return Json(new List<object>(), JsonRequestBehavior.AllowGet);
            }

            var mayKhuons = db.MayKhuons
                .Where(x => x.TenMayKhuon.Contains(searchString))
                .Select(x => new
                {
                    x.Id,
                    x.TenMayKhuon
                })
                .ToList();

            return Json(mayKhuons, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FormPhieuLenKhuon(KHUONEP khuongep)
        {
            ViewBag.TenKhuonLen = new SelectList(db.MayKhuons, "MaMayKhuon", "ID");
            if (ModelState.IsValid)
            {
                db.KHUONEPs.Add(khuongep);
                db.SaveChanges();
            }
            return View(khuongep);

        }
        public ActionResult Create()
        {
            var khuons = db.MayKhuons
                .Where(m => m.Equipment.StartsWith("1"))
                .Select(m => new { m.Id, m.TenMayKhuon })
                .ToList();

            var mays = db.MayKhuons
                .Where(m => m.Equipment.StartsWith("3"))
                .Select(m => new { m.Id, m.TenMayKhuon })
                .ToList();

            var nlieus = db.NGUYENLIEUx
                .Select(m => new { m.ID, m.TenNguyenLieu})
                .ToList();

            ViewBag.Nlieulist = new SelectList(nlieus, "ID", "TenNguyenLieu");
            ViewBag.KhuonList = new SelectList(khuons, "Id", "TenMayKhuon");
            ViewBag.MayList = new SelectList(mays, "Id", "TenMayKhuon");
            return PartialView("test",new KHUONEP());
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}