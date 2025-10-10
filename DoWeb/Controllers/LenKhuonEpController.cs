using DoWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoWeb.Controllers
{
    public class LenKhuonEpController : Controller
    {
        private ProjectDBEntities db = new ProjectDBEntities();

        public ActionResult Index()
        {
            var dskhuonep = db.KHUONEPs.ToList();
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
                    x.TenMayKhuon,
                    x.Equipment
                })
                .ToList();

            return Json(mayKhuons, JsonRequestBehavior.AllowGet);
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
                .Select(m => new { m.ID, m.TenNguyenLieu })
                .ToList();

            ViewBag.Nlieulist = new SelectList(nlieus, "ID", "TenNguyenLieu");
            ViewBag.KhuonList = new SelectList(khuons, "Id", "TenMayKhuon");
            ViewBag.MayList = new SelectList(mays, "Id", "TenMayKhuon");
            return PartialView("_FormLenKhuon", new KHUONEP());
        }
    }
}