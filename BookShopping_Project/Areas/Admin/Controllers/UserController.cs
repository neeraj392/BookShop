
using BookShopping_Project.DataAccess.Data;
using BookShopping_Project.Models;
using BookShopping_Project.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace BookShopping_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_User_Admin+","+SD.Role_User_Employee)]
 
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;
        public UserController(ApplicationDbContext context)
        {
            _context = context;

        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var UserList = _context.ApplicationUsers.Include(c => c.company).ToList();     //Data from Asp.netUser Table
            var roles = _context.Roles.ToList();                                          //Data From AspRole Table
            var UserRole = _context.UserRoles.ToList();                                 //Data From AspNetUserRole Table
            foreach (var User in UserList)
            {
                var roleid = UserRole.FirstOrDefault(u => u.UserId == User.Id).RoleId;
                User.Role = roles.FirstOrDefault(r => r.Id == roleid).Name;
                if (User.company == null)
                {
                    User.company = new Company()
                    {
                        Name = ""
                    };
                }

            }
            if (!User.IsInRole(SD.Role_User_Admin))
            {
                var userAdmin = UserList.FirstOrDefault(u => u.Role == SD.Role_User_Admin);
                UserList.Remove(userAdmin);
            }
            return Json(new { data = UserList });
        }

        [HttpPost]
        public IActionResult LockUnlock([FromBody] string id)
        {
            bool isLocked = false;
            var userindb = _context.ApplicationUsers.FirstOrDefault(u => u.Id == id);
            if (userindb == null)
                return Json(new { success = false, message = "error while lock/unlock User" });
            if (userindb != null && userindb.LockoutEnd > DateTime.Now)
            {
                userindb.LockoutEnd = DateTime.Now;
                isLocked = false;
            }
            else
            {
                userindb.LockoutEnd = DateTime.Now.AddYears(10);
                isLocked = true;
            }
            _context.SaveChanges();
            return Json(new { success = true, message = isLocked == true ? "user succesfully Locked" : "User Succesfully Unlocked" });
        }


    }
}
