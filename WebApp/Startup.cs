using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebApp.Data;
using WebApp.Domain;

namespace WebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        
        public static string DockerHostMachineIpAddress => Dns.GetHostAddresses(new Uri("http://docker.for.win.localhost").Host)[0].ToString();

        public void ConfigureServices(IServiceCollection services)
        {
            var assembly = typeof(Startup).Assembly.GetName().Name;

            var server = Configuration["DBServer"] ?? "localhost";
            var port = Configuration["DBPort"] ?? "14333";
            var user = Configuration["DBUser"] ?? "SA";
            var password = Configuration["DBPassword"] ?? "Password2020";
            var database = Configuration["DBDatabase"] ?? "Portofolio";

            var connection = $"Server={server},14330;Initial Catalog=Portofolio;User ID =SA;Password=Password2020";
            
            services.AddDbContext<AppDbContext>(builder =>
            {
                builder.UseSqlServer(connection, b =>
                {
                    b.MigrationsAssembly("WebApp");
                });
            });
            
            
            // for identity
            services.AddDbContext<IdentityDbContext>(builder =>
            {
                builder.UseInMemoryDatabase("Dev");
            });

            services.AddIdentity<IdentityUser, IdentityRole>(options =>
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequiredLength = 6;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                })
                .AddEntityFrameworkStores<IdentityDbContext>()
                .AddDefaultTokenProviders();
            

            services.AddAuthentication("CookieAuth")
                .AddCookie("CookieAuth", config =>
                {
                    config.Cookie.Name = "meindertcookie";
                    config.LoginPath = "/account/login";
                });

            services.AddTransient<IImageProcessingService, ImageProcessingService>();
            
            services.AddControllersWithViews();
            
         }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            
            app.UseRouting();

            app.UseAuthentication();
            
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}