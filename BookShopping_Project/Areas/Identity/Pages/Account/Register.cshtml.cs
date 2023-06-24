using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using BookShopping_Project.DataAccess.Repository.IRepository;
using BookShopping_Project.Models;
using BookShopping_Project.Utility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace BookShopping_Project.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUnitofwork _unitofwork;


        public RegisterModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
              RoleManager<IdentityRole> roleManager,
              IUnitofwork unitofwork
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _roleManager = roleManager;
            _unitofwork = unitofwork;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            public string Name { get; set; }
            [Display(Name = "Street Address")]
            public string StreetAddress { get; set; }
            public string City { get; set; }
            public string State { get; set; }
            [Display(Name = "Postal Code")]
            public string PostalCode { get; set; }
            [Display(Name = "Phone Number")]
            public string PhoneNumber { get; set; }
            [Display(Name = "Company")]
            public int? CompanyId { get; set; }
            public string Role { get; set; }

            //Roles for comapny

            public IEnumerable<SelectListItem> Companylist { get; set; }
            public IEnumerable<SelectListItem> Rolelist { get; set; }

        }

        public async Task OnGetAsync(string returnUrl = null)
        {

            Input = new InputModel()
            {
                Companylist = _unitofwork.Company.GetAll().Select(cl => new SelectListItem()
                {
                    Text = cl.Name,
                    Value = cl.Id.ToString()

                }),

                Rolelist = _roleManager.Roles.Where(r => r.Name != SD.Role_User_Admin).Select(x => x.Name).Select(rl => new SelectListItem()
                {
                    Text = rl,
                    Value = rl
                }),
            };
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                // var user = new IdentityUser { UserName = Input.Email, Email = Input.Email };
                var user = new ApplicationUser()
                {
                    Name = Input.Name,
                    Email = Input.Email,
                    UserName = Input.Email,
                    StreetAddress = Input.StreetAddress,
                    City = Input.City,
                    State = Input.State,
                    PostalCode = Input.PostalCode,
                    PhoneNumber = Input.PhoneNumber,
                    CompanyId = Input.CompanyId,
                    Role = Input.Role
                };
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");
                    //Check for roles

                    if (!await _roleManager.RoleExistsAsync(SD.Role_User_Admin))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(SD.Role_User_Admin));
                    }
                    if (!await _roleManager.RoleExistsAsync(SD.Role_User_Company))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(SD.Role_User_Company));
                    }

                    if (!await _roleManager.RoleExistsAsync(SD.Role_User_Employee))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(SD.Role_User_Employee));
                    }
                    if (!await _roleManager.RoleExistsAsync(SD.Role_User_Individual))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(SD.Role_User_Individual));
                    }

                    //add role to admin


                   //await _userManager.AddToRoleAsync(user, SD.Role_User_Admin);

                    if (user.Role == null)
                    {
                        await _userManager.AddToRoleAsync(user, SD.Role_User_Individual);
                    }
                    else
                    {
                        if (user.CompanyId > 0)
                        {
                            await _userManager.AddToRoleAsync(user, SD.Role_User_Company);
                        }
                        else
                        {
                            await _userManager.AddToRoleAsync(user, user.Role);
                        }
                    }

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        if(user.Role==null)
                        {
                            await _signInManager.SignInAsync(user, isPersistent: false);
                            return LocalRedirect(returnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "User", new { Area = "Admin" });
                        }
                      
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
