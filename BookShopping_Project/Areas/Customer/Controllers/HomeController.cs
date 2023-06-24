using BookShopping_Project.DataAccess.Repository.IRepository;
using BookShopping_Project.Models;
using BookShopping_Project.Models.ViewModels;
using BookShopping_Project.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;

namespace BookShopping_Project.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitofwork _Unitofwork;

        public HomeController(ILogger<HomeController> logger,IUnitofwork Unitofwork)
        {
            _logger = logger;
            _Unitofwork = Unitofwork;
        
        }

        public IActionResult Index()
        {
            var ClaimsIdentity = (ClaimsIdentity)(User.Identity);
            var claim = ClaimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (claim != null)
            {
                var count = _Unitofwork.ShoppingCart.GetAll(sc => sc.ApplicationUserId == claim.Value).ToList().Count;
                HttpContext.Session.SetInt32(SD.Ss_Session, count);
            }
            var productlist = _Unitofwork.Product.GetAll(includepropertities: "Category,CoverType");
            return View(productlist);
        }
        public IActionResult Details(int id)
        {
            var ProductInDb = _Unitofwork.Product.FirstorDefault(p => p.Id == id, 
                includeproperties: "Category,CoverType");
            var ShoppingCart = new ShoppingCart()
            {
                Product = ProductInDb,
                ProductId = ProductInDb.Id
            };
            return View(ShoppingCart);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Details(ShoppingCart shoppingCart)
        {
            shoppingCart.Id = 0;

            if (ModelState.IsValid)
            {
                var claimIdentity = (ClaimsIdentity)(User.Identity);
                var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);
                shoppingCart.ApplicationUserId = claim.Value;
                var ShoppingCartFromDb = _Unitofwork.ShoppingCart.FirstorDefault
                    (s => s.ApplicationUserId == claim.Value && s.ProductId == shoppingCart.ProductId,
                    includeproperties: "Product");
                if (ShoppingCartFromDb == null)
                    _Unitofwork.ShoppingCart.add(shoppingCart);
                else
                    ShoppingCartFromDb.Count += shoppingCart.Count;
                _Unitofwork.Save();

                //session start
                var count = _Unitofwork.ShoppingCart.GetAll(sc => sc.ApplicationUserId == claim.Value).ToList().Count;
                HttpContext.Session.SetInt32(SD.Ss_Session, count);



                return RedirectToAction(nameof(Index));
            }
            else
            {
                var ProductInDb = _Unitofwork.Product.FirstorDefault(
                  p => p.Id == shoppingCart.ProductId, includeproperties: "Category,CoverType");
                var shoppingcart = new ShoppingCart()
                {
                    Product = ProductInDb,
                    ProductId = ProductInDb.Id
                };
                return View(shoppingCart);
            }
               
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
