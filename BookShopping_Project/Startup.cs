using BookShopping_Project.DataAccess.Data;
using BookShopping_Project.DataAccess.Repository;
using BookShopping_Project.DataAccess.Repository.IRepository;
using BookShopping_Project.Utility;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Stripe;

namespace BookShopping_Project
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDatabaseDeveloperPageExceptionFilter();

            //services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            //    .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddIdentity<IdentityUser, IdentityRole>().AddDefaultTokenProviders().AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddRazorPages();
            services.AddControllersWithViews();
            services.AddScoped<IEmailSender, EmailSender>();
            
            services.AddScoped<IUnitofwork, Unitofwork>();
            services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));
            services.Configure<StripeSetting>(Configuration.GetSection("Stripe"));
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = $"/Identity/Account/Login";
                options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
                options.LogoutPath = $"/Identity/Account/Logout";

            });
           
            services.AddSession(options =>
            {
                options.IdleTimeout = System.TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
     

            services.AddAuthentication().AddGoogle(options =>
            {
                options.ClientId = "389726946907-j027uhovipn6bi9nr7k4bj0b4k8bbvn2.apps.googleusercontent.com";
                options.ClientSecret = "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";

            });
            services.AddAuthentication().AddFacebook(options =>
            {
                options.AppId = "5738497752833497";
                options.AppSecret = "0d72ee470f23d8ced26a7158667f6280";
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            StripeConfiguration.ApiKey = Configuration.GetSection("Stripe")["SecretKey"];
            app.UseSession();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
