using DoWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoWeb.Controllers
{
    public class MayKhuonsController : Controller
    {
        private ProjectDBEntities db = new ProjectDBEntities();

        // GET: MayKhuons
        public ActionResult Index()
        {
            return View();
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
        public ActionResult FormLenKhuon()
        {
            return View();
        }
    }
}