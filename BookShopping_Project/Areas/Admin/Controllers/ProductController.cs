using BookShopping_Project.DataAccess.Repository.IRepository;
using BookShopping_Project.Models;
using BookShopping_Project.Models.ViewModels;
using BookShopping_Project.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.IO;
using System.Linq;
namespace BookShopping_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_User_Admin)]
    public class ProductController : Controller
    {
        private readonly IUnitofwork _Unitofwork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitofwork Unitofwork, IWebHostEnvironment webHostEnvironment )
        {
            _Unitofwork = Unitofwork;
            _webHostEnvironment = webHostEnvironment;

        }
        public IActionResult Index()
        {
            return View();
        }
        #region MyRegion
        [HttpGet]
        public IActionResult GetAll()
        {
            var Productlist = _Unitofwork.Product.GetAll(includepropertities: "Category,CoverType");
            return Json(new { data = Productlist });
        }
        public IActionResult Upsert(int? id)
        {
            ProductVM ProductVm = new ProductVM()
            {
                product = new Product(),
                categorylist = _Unitofwork.category.GetAll().Select(cl => new SelectListItem()
                {
                    Text = cl.Name,
                    Value = cl.Id.ToString()
                }),
                covertypelist = _Unitofwork.coverType.GetAll().Select(ct => new SelectListItem()
                {
                    Text = ct.Name,
                    Value = ct.Id.ToString()
                })

            };
            if (id == null)
                return View(ProductVm);
            ProductVm.product = _Unitofwork.Product.Get(id.GetValueOrDefault());
            return View(ProductVm);
        }
        [HttpPost ]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM productVM)
        {
            if (ModelState.IsValid)
            {
                var webrootpath = _webHostEnvironment.WebRootPath;
                var files = HttpContext.Request.Form.Files;
                if (files.Count() > 0)
                {
                    string filename = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(webrootpath, @"Images/Products");
                    var extenssion = Path.GetExtension(files[0].FileName);

                    if (productVM.product.Id != 0)
                    {
                        var imageexist = _Unitofwork.Product.Get(productVM.product.Id).ImageUrl;
                        productVM.product.ImageUrl = imageexist;
                    }
                    if (productVM.product.ImageUrl != null)
                    {
                        var imagepath = Path.Combine(webrootpath, productVM.product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(imagepath))
                        {
                            System.IO.File.Delete(imagepath);
                        }
                    }
                    using (var filestream = new FileStream(Path.Combine(uploads, filename + extenssion), FileMode.Create))
                    {
                        files[0].CopyTo(filestream);
                    }
                    productVM.product.ImageUrl = @"/Images/Products/" + filename + extenssion;
                }
                else
                {
                    if (productVM.product.Id != 0)
                    {
                        var Productindb = _Unitofwork.Product.Get(productVM.product.Id);
                        productVM.product.ImageUrl = Productindb.ImageUrl;
                    }
                }
                if (productVM.product.Id == 0)
                    _Unitofwork.Product.add(productVM.product);
                else
                    _Unitofwork.Product.Update(productVM.product);
                _Unitofwork.Save();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                productVM = new ProductVM
                {
                    categorylist = _Unitofwork.category.GetAll().Select(cl => new SelectListItem()
                    {
                        Text = cl.Name,
                        Value = cl.Id.ToString()


                    }),
                    covertypelist = _Unitofwork.coverType.GetAll().Select(ct => new SelectListItem()
                    {
                        Text = ct.Name,
                        Value = ct.Id.ToString()

                    })
                };
                if (productVM.product.Id != 0)
                {
                    productVM.product = _Unitofwork.Product.Get(productVM.product.Id);
                }
                return View(productVM);
            }
        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var p = _Unitofwork.Product.Get(id);
            if (p == null)
                return Json(new { success = false, message = "error while delete data!!" });
            var webrootpth = _webHostEnvironment.WebRootPath;
            var imagepath = Path.Combine(webrootpth, p.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(imagepath))
            {
                System.IO.File.Delete(imagepath);
            }



            _Unitofwork.Product.Remove(p);
            _Unitofwork.Save();
            return Json(new { success = true, message = "deleted successfully" });


        }
        #endregion
    }
}
