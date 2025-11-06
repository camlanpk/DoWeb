using DoWeb.Helpers;
using DoWeb.Models;
using Helpers.linq.Dynamic;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace DoWeb.Controllers
{
    public class LenKhuonEpController : Controller
    {
        private ProjectDBEntities db = new ProjectDBEntities();

        public ActionResult Index()
        {
            ViewBag.KhuonList = new SelectList(db.MayKhuons, "Id", "TenMayKhuon");

            var dskhuonep = db.KHUONEPs
                .Include(p => p.MayKhuon).Include(p => p.MayKhuon1).Include(p => p.MayKhuon2).ToList();
            return View(dskhuonep.ToList());
        }

        // list khuon
        [HttpGet]
        public ActionResult LayDanhSachListKhuonSelect2(string searchTerm = "", int? pageSize = 5, int? pageNumber = 1)
        {
            //string Return = null;
            try
            {
                var lst = db.MayKhuons.AsQueryable()?.Where(x => x.Equipment.StartsWith("1")).ToList();

                if (lst == null)
                    lst = new List<MayKhuon>();

                if (!string.IsNullOrEmpty(searchTerm))
                {
                    lst = lst.Where(x => x.Equipment.Like(searchTerm) || x.TenMayKhuon.Like(searchTerm)).ToList();
                }

                var select2pagedResult = new Select2PagedResult();

                select2pagedResult.Total = lst.Count;
                select2pagedResult.Results = lst.Select(x => new Select2OptionModel { id = x.Id, text = x.TenMayKhuon, textextra = $"({x.Equipment})" })
                    .Skip((pageNumber
                    .GetValueOrDefault(1) - 1) * pageSize
                    .GetValueOrDefault(1))
                    .Take(pageSize
                    .GetValueOrDefault(1))
                    .ToList();

                return Json(select2pagedResult, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex) { }
            return new EmptyResult();
        }

        [HttpGet]
        public ActionResult ListMaySelect2(string searchTerm = "", int? pageSize = 5, int? pageNumber = 1)
        {
            try
            {
                var list = db.MayKhuons.AsQueryable()?.Where(x => x.Equipment.StartsWith("3")).ToList();

                if (list == null) list = new List<MayKhuon>();

                if (!string.IsNullOrEmpty(searchTerm))
                {
                    list = list.Where(x => x.Equipment.Like(searchTerm) || x.TenMayKhuon.Like(searchTerm)).ToList();
                }

                var select2pagedResult = new Select2PagedResult();

                select2pagedResult.Total = list.Count;

                select2pagedResult.Results = list.Select(x => new Select2OptionModel { id = x.Id, text = x.TenMayKhuon, textextra = $"({x.Equipment})" })
                    .Skip((pageNumber
                    .GetValueOrDefault(1) - 1) * pageSize
                    .GetValueOrDefault(1))
                    .Take(pageSize
                    .GetValueOrDefault(1))
                    .ToList();

                return Json(select2pagedResult, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex) { }
            return new EmptyResult();
        }

        [HttpGet]
        public ActionResult ListNguyenLieuSelect2(string searchTerm = "", int? pageSize = 5, int? pageNumber = 1)
        {
            try
            {
                var list = db.NGUYENLIEUx.AsQueryable()?.ToList();

                if (list == null) list = new List<NGUYENLIEU>();

                if (!string.IsNullOrEmpty(searchTerm))
                {
                    list = list.Where(x => x.TenNguyenLieu.Like(searchTerm) || x.TenNguyenLieu.Like(searchTerm)).ToList();
                }

                var select2pagedResult = new Select2PagedResult();

                select2pagedResult.Total = list.Count;

                select2pagedResult.Results = list.Select(x => new Select2OptionModel { id = x.ID, text = x.TenNguyenLieu, textextra = $"({x.TiLePheTron}%)" })
                    .Skip((pageNumber
                    .GetValueOrDefault(1) - 1) * pageSize
                    .GetValueOrDefault(1))
                    .Take(pageSize
                    .GetValueOrDefault(1))
                    .ToList();

                return Json(select2pagedResult, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex) { }
            return new EmptyResult();
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