using BookShopping_Project.DataAccess.Repository.IRepository;
using BookShopping_Project.Models;
using BookShopping_Project.Models.ViewModels;
using BookShopping_Project.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;


namespace BookShopping_Project.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CartController : Controller
    {
        private readonly IEmailSender _emailSender;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUnitofwork _Unitofwork;
        public CartController(IUnitofwork unitofwork, UserManager<IdentityUser> userManager, IEmailSender emailSender)
        {
            _Unitofwork = unitofwork;
            _userManager = userManager;
            _emailSender = emailSender;
        }
        [BindProperty]
        public ShoppingCartVM shoppingCartVM { get; set; }
        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if (claim == null)
            {
                shoppingCartVM = new ShoppingCartVM()
                {
                    Listcart = new List<ShoppingCart>()
                };
            }
            shoppingCartVM = new ShoppingCartVM()
            {
                orderHeader = new OrderHeader(),
                Listcart = _Unitofwork.ShoppingCart.GetAll(u => u.ApplicationUserId == claim.Value,
                includepropertities: "Product")
            };
            shoppingCartVM.orderHeader.OrderTotal = 0;
            shoppingCartVM.orderHeader.ApplicationUser = _Unitofwork.ApplicationUser.FirstorDefault
                (u => u.Id == claim.Value, includeproperties: "company");

            foreach (var list in shoppingCartVM.Listcart)
            {
                list.Price = SD.GetPriceBasedOnQuantity(list.Count, list.Product.Price, list.Product.Price50,
                    list.Product.Price100);
                shoppingCartVM.orderHeader.OrderTotal += (list.Price * list.Count);
                list.Product.Description = SD.ConvertToRawHtml(list.Product.Description);
                if (list.Product.Description.Length > 100)
                {
                    list.Product.Description = list.Product.Description.Substring(0, 99) + "...";
                }
            }
            return View(shoppingCartVM);

        }
        public IActionResult plus(int cartId)
        {
            var cart = _Unitofwork.ShoppingCart.FirstorDefault(sc => sc.Id == cartId);
            cart.Count += 1;
            _Unitofwork.Save();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult minus(int cartId)
        {
            var cart = _Unitofwork.ShoppingCart.FirstorDefault(sc => sc.Id == cartId);
            cart.Count -= 1;
            _Unitofwork.Save();
            return RedirectToAction(nameof(Index));

        }
        public IActionResult Remove(int cartId)
        {
            var cart = _Unitofwork.ShoppingCart.FirstorDefault(sc => sc.Id == cartId);
            _Unitofwork.ShoppingCart.Remove(cart);
            _Unitofwork.Save();
            //session update
            var count = _Unitofwork.ShoppingCart.GetAll(sc => sc.ApplicationUserId == cart.ApplicationUserId).ToList().Count;
            HttpContext.Session.SetInt32(SD.Ss_Session, count);

            return RedirectToAction(nameof(Index));
        }
        public IActionResult Summary()
        {
            var claimidentity = (ClaimsIdentity)User.Identity;
            var claim = claimidentity.FindFirst(ClaimTypes.NameIdentifier);
            shoppingCartVM = new ShoppingCartVM()
            {
                orderHeader = new OrderHeader(),
                Listcart = _Unitofwork.ShoppingCart.GetAll(sc => sc.ApplicationUserId == claim.Value,
                includepropertities: "Product")
            };
            shoppingCartVM.orderHeader.ApplicationUser = _Unitofwork.ApplicationUser.FirstorDefault
                (u => u.Id == claim.Value, includeproperties: "company");
            foreach (var list in shoppingCartVM.Listcart)
            {
                list.Price = SD.GetPriceBasedOnQuantity(list.Count, list.Product.Price, list.Product.Price50,
                    list.Product.Price100);
                shoppingCartVM.orderHeader.OrderTotal += (list.Price * list.Count);
                list.Product.Description = SD.ConvertToRawHtml(list.Product.Description);
            }
            shoppingCartVM.orderHeader.Name = shoppingCartVM.orderHeader.ApplicationUser.Name;
            shoppingCartVM.orderHeader.PhoneNumber = shoppingCartVM.orderHeader.ApplicationUser.PhoneNumber;
            shoppingCartVM.orderHeader.PostalCode = shoppingCartVM.orderHeader.ApplicationUser.PostalCode;
            shoppingCartVM.orderHeader.StreetAddress = shoppingCartVM.orderHeader.ApplicationUser.StreetAddress;
            shoppingCartVM.orderHeader.City = shoppingCartVM.orderHeader.ApplicationUser.City;
            shoppingCartVM.orderHeader.State = shoppingCartVM.orderHeader.ApplicationUser.State;

            return View(shoppingCartVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Summary")]
        public IActionResult SummaryPost(string stripeToken)
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);
            shoppingCartVM.orderHeader.ApplicationUser = _Unitofwork.ApplicationUser.FirstorDefault
                (u => u.Id == claim.Value, includeproperties: "company");
            shoppingCartVM.Listcart = _Unitofwork.ShoppingCart.GetAll(
                sc => sc.ApplicationUserId == claim.Value,
                includepropertities: "Product");
            shoppingCartVM.orderHeader.PaymentStatus = SD.PaymentStatusPending;
            shoppingCartVM.orderHeader.OrderStatus = SD.StatusPending;
            shoppingCartVM.orderHeader.OrderDate = DateTime.Now;
            shoppingCartVM.orderHeader.ApplicationUserId = claim.Value;
            _Unitofwork.OrderHeader.add(shoppingCartVM.orderHeader);
            _Unitofwork.Save();
            
            foreach (var item in shoppingCartVM.Listcart)
            {
                item.Price = SD.GetPriceBasedOnQuantity(item.Count,
                    item.Product.Price, item.Product.Price50, item.Product.Price100);
                OrderDetail orderDetail = new OrderDetail()
                {
                    Productid = item.ProductId,
                    Orderid = shoppingCartVM.orderHeader.id,
                    price = item.Price,
                    Count = item.Count
                };
                shoppingCartVM.orderHeader.OrderTotal += orderDetail.price * orderDetail.Count;
                _Unitofwork.OrderDetail.add(orderDetail);
                _Unitofwork.Save();
                
            }
            _Unitofwork.ShoppingCart.RemoveRange(shoppingCartVM.Listcart);
            //_Unitofwork.Save();
            //HttpContext.Session.SetInt32(SD.Ss_Session, 0);



            #region Stripe Payment
            if (stripeToken == null)
            {
                shoppingCartVM.orderHeader.PaymentDueDate = DateTime.Now.AddDays(30);
                shoppingCartVM.orderHeader.PaymentStatus = SD.PaymentStatusDelayPayment;
                shoppingCartVM.orderHeader.OrderStatus = SD.PaymentStatusApproved;
            }
            else
            {
                var options = new ChargeCreateOptions()                 //payment Process start
                {
                    Amount = Convert.ToInt32(shoppingCartVM.orderHeader.OrderTotal),
                    Currency = "inr",
                    Description = "order id:" + shoppingCartVM.orderHeader.id,
                    Source = stripeToken
                };
                var service = new ChargeService();     //balance  start cut from account
                Charge charge = service.Create(options);
                if (charge.BalanceTransactionId == null)
                    shoppingCartVM.orderHeader.PaymentStatus = SD.PaymentStatusRejected;
                else
                    shoppingCartVM.orderHeader.Transactionid = charge.BalanceTransactionId;
                if (charge.Status.ToLower() == "succeeded")
                {
                    shoppingCartVM.orderHeader.PaymentStatus = SD.PaymentStatusApproved;
                    shoppingCartVM.orderHeader.OrderStatus = SD.StatusApproved;
                    shoppingCartVM.orderHeader.PaymentDate = DateTime.Now;
                }
            }
            _Unitofwork.Save();
            HttpContext.Session.SetInt32(SD.Ss_Session, 0);
            #endregion
            return RedirectToAction("OrderConfirmation", "cart",
                new { id = shoppingCartVM.orderHeader.id });
        }
        public IActionResult orderConfirmation(int id)
        {
            return View(id);
        }

    }
}