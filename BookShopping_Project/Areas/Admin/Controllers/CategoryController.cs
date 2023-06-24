using BookShopping_Project.DataAccess.Repository.IRepository;
using BookShopping_Project.Models;
using BookShopping_Project.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace BookShopping_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
[Authorize(Roles =SD.Role_User_Admin)]
    public class CategoryController : Controller
    {
        private readonly IUnitofwork _unitofwork;
        public CategoryController(IUnitofwork unitofwork)
        {
            _unitofwork = unitofwork;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Upsert(int? id)
        {
            Category category = new Category();
            if (id == null)
            {
                return View(category);
            }
            else
            {
                var Category = _unitofwork.category.Get(id.GetValueOrDefault());
                return View(Category);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Category category)
        {
            if (category == null)
                return NotFound();
            if (ModelState.IsValid)
            {
                if (category.Id == 0)
                    _unitofwork.category.add(category);
                else
                    _unitofwork.category.update(category);
                _unitofwork.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var catdb = _unitofwork.category.Get(id);
            if (catdb == null)
                return Json(new { success = false, message = "error while delete data!" });
            _unitofwork.category.Remove(catdb);
            _unitofwork.Save();
            return Json(new { success = true, message = "data deleted sucessfully!" });
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var categorylist = _unitofwork.category.GetAll();
            return Json(new { data = categorylist });
        }
    }
}
