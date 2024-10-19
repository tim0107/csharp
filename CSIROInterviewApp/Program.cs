
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
            builder.Services.AddControllers();
            builder.Services.AddRazorPages();

            builder.Services.AddDbContext<ApplicationDataContext>(options =>
            {
                var connectionString = configuration.GetConnectionString("DBConnection");
                options.UseSqlServer(connectionString);



            });

            builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedEmail = true;
            }
            )
                .AddEntityFrameworkStores<ApplicationDataContext>()
                .AddDefaultTokenProviders();

            var app = builder.Build();



            bool isDevelopment = configuration.GetValue<bool>("EnableDeveloperExceptions") == true;
            if (isDevelopment)
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error.html");

               

                 app.Run();
            }
        }
    }
}
