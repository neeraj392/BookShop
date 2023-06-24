using BookShopping_Project.DataAccess.Repository.IRepository;
using BookShopping_Project.Models;
using BookShopping_Project.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace BookShopping_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_User_Admin)]
    public class CoverTypeController : Controller
    {
        private readonly IUnitofwork _Unitofwork;
        public CoverTypeController(IUnitofwork Unitofwork)
        {
            _Unitofwork = Unitofwork;
        }
        public IActionResult Index()
        {
            return View();
        }
        #region APIs
        [HttpGet]
        public IActionResult GetAll()
        {
            return Json(new { data = _Unitofwork.coverType.GetAll()});
        }
        public IActionResult Upsert(int? id)
        {
            CoverType covertype = new CoverType();
            if (id == null)
            {
                return View(covertype);
            }
            else
            {
                var coverlist = _Unitofwork.coverType.Get(id.GetValueOrDefault());
                return View(coverlist);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(CoverType coverType)
        {
            if (coverType == null)
                return NotFound();
            if (ModelState.IsValid)
            {
                if (coverType.Id == 0)
                    _Unitofwork.coverType.add(coverType);
                else
                    _Unitofwork.coverType.update(coverType);
                _Unitofwork.Save();
                return RedirectToAction(nameof(Index));

            }
            return View(coverType);
        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var covrt = _Unitofwork.coverType.Get(id);
            if (covrt == null)
                return Json(new { success = false, message = "Error while delete data!!" });
            _Unitofwork.coverType.Remove(covrt);
            _Unitofwork.Save();
            return Json(new { success = true, message = "Data Deleted Successfully!!" });
        }
        #endregion APIS
    }
}
