
using CSIROInterviewApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;



namespace CSIROInterviewApp
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);
            IConfigurationRoot configuration = new ConfigurationBuilder()
                                                .AddJsonFile("./config.json")
                                                .Build();
            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();
            builder.Services.AddAuthentication("MyCookieAuth").AddCookie("MyCookieAuth", option =>
            {
                option.Cookie.Name = "MyCookieAuth";
                option.LoginPath = "/Account/Login?role=user";
                option.AccessDeniedPath = "/Account/AccessDenied";
            });

            builder.Services.AddAuthorization(option =>
            {
                option.AddPolicy("Admin", policy =>
                {
                    policy.RequireClaim("Role", "Admin");
                });
                option.AddPolicy("User", policy =>
                {
                    policy.RequireClaim("Role", "User");
                });
            });

            builder.Services.AddDbContext<ApplicationDataContext>(options =>
            {
                var connectionString = configuration.GetConnectionString("DBConnection");
                options.UseSqlServer(connectionString);
            });

            //builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
            //{
            //    options.SignIn.RequireConfirmedEmail = true;
            //}
            //)
            //    .AddEntityFrameworkStores<ApplicationDataContext>()
            //    .AddDefaultTokenProviders();

            var app = builder.Build();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            bool isDevelopment = configuration.GetValue<bool>("EnableDeveloperExceptions");
            if (isDevelopment)
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error.html");
            }

            app.Run();
        }
    }
}
