
using BookShopping_Project.DataAccess.Repository.IRepository;
using BookShopping_Project.Models;
using BookShopping_Project.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace BookShopping_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_User_Admin + "," + SD.Role_User_Employee)]
    public class CompanyController : Controller
    {
        private readonly IUnitofwork _Unitofwork;
        public CompanyController(IUnitofwork unitofwork)
        {
            _Unitofwork = unitofwork;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Upsert(int?id)
        {
            Company company = new Company();
            if (id == null)
                return View(company);
            company = _Unitofwork.Company.Get(id.GetValueOrDefault());
            return View(company);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Company company)
        {
            if (ModelState.IsValid)
            {
                if (company.Id == 0)
                    _Unitofwork.Company.add(company);
                else
                    _Unitofwork.Company.Update(company);
                _Unitofwork.Save();
                return RedirectToAction(nameof(Index));
            }
            else
                return View();
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            return Json(new { data = _Unitofwork.Company.GetAll() });
        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var CompanyInDb = _Unitofwork.Company.Get(id);
            if (CompanyInDb == null)
                return Json(new { success = false, message = "Something went wrong while delete data " });
            _Unitofwork.Company.Remove(CompanyInDb);
            _Unitofwork.Save();
            return Json(new { success = true, message = "data deleted sucessfully" });
        }
    }
}
