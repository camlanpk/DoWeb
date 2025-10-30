using DoWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.IO;
using System.Data.Entity;

namespace DoWeb.Controllers
{
    public class LenKhuonEpController : Controller
    {
        private ProjectDBEntities db = new ProjectDBEntities();

        public ActionResult Index()
        {
            ViewBag.KhuonList = new SelectList(db.MayKhuons, "Id", "TenMayKhuon");

            var dskhuonep = db.KHUONEPs
                .Include(p => p.MayKhuon).Include(p => p.MayKhuon1).ToList();
            return View(dskhuonep.ToList());
        }
        public ActionResult SearchMay(string searchString)
        {
            var query = db.MayKhuons.AsQueryable();
            if (string.IsNullOrEmpty(searchString))
            {
                query = query.Where(x => x.TenMayKhuon.Contains(searchString));
            }
            query = query.Where(x => x.Equipment.StartsWith("3"));
            var mayKhuons = query
                .Where(x => x.TenMayKhuon.Contains(searchString))
                .Select(x => new
                {
                    MaMayKhuon = x.Id,
                    x.TenMayKhuon
                })
                .Take(10)
                .ToList();

            return Json(mayKhuons, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SearchKhuon(string searchString)
        {
            var query = db.MayKhuons.AsQueryable();

            if (string.IsNullOrEmpty(searchString))
            {
                query = query.Where(x => x.TenMayKhuon.Contains(searchString));
            }
            query = query.Where(x => x.Equipment.StartsWith("1"));
            var mayKhuons = query
                .Where(x => x.TenMayKhuon.Contains(searchString))
                .Select(x => new
                {
                    MaKhuon = x.Id,
                    x.TenMayKhuon
                })
                .Take(10)
                .ToList();

            return Json(mayKhuons, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SearchNguyenLieu(string searchString)
        {
            var query = db.NGUYENLIEUx.AsQueryable();

            if (string.IsNullOrEmpty(searchString))
            {
                query = query.Where(x => x.TenNguyenLieu.Contains(searchString));
            }

            var nlieu = query.Where(x => x.TenNguyenLieu.Contains(searchString))
                .Select(x => new
                {
                    MaNguyenLieu = x.ID,
                    x.TenNguyenLieu
                })
                .Take(10)
                .ToList();

            return Json(nlieu, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Create()
        {
            return PartialView("_FormLenKhuon", new KHUONEP());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(KHUONEP model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .Where(e => !string.IsNullOrWhiteSpace(e))
                    .ToList();

                return Json(new
                {
                    success = false,
                    errors = ModelState
                        .Where(kvp => kvp.Value.Errors.Any())
                        .ToDictionary(
                            kvp => kvp.Key,
                            kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                        )
                });
            }

            try
            {
                if (model.CHUKYNANGSUAT != null)
                {
                    db.CHUKYNANGSUATs.Add(model.CHUKYNANGSUAT);
                    db.SaveChanges();
                    model.MaCKNS = model.CHUKYNANGSUAT.ID;
                }

                if (model.PHEPHAM != null)
                {
                    db.PHEPHAMs.Add(model.PHEPHAM);
                    db.SaveChanges();
                    model.MaPhePham = model.PHEPHAM.ID;
                }

                if (model.THOIGIANLENKHUON != null)
                {
                    db.THOIGIANLENKHUONs.Add(model.THOIGIANLENKHUON);
                    db.SaveChanges();
                    model.MaThoiGian = model.THOIGIANLENKHUON.ID;
                }

                if (model.TRONGLUONG != null)
                {
                    db.TRONGLUONGs.Add(model.TRONGLUONG);
                    db.SaveChanges();
                    model.MaTrongLuong = model.TRONGLUONG.ID;
                }

                db.KHUONEPs.Add(model);
                db.SaveChanges();

                return Json(new
                {
                    success = true,
                    message = "Tạo mới thành công!",
                    id = model.ID
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = "Lỗi khi tạo mới!",
                    errorDetail = ex.Message
                });
            }

        }

    }
}