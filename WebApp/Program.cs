using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WebApp.Data;

namespace WebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var env = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();
                var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
                
                try
                {
                    var context = services.GetRequiredService<AppDbContext>();
                    if (context.Database.GetPendingMigrations().Any())
                    {
                        context.Database.Migrate();
                    }
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred seeding the DB.");
                }


                string adminName = "admin";
                string adminPassword;

                if (env.IsProduction())
                {
                    adminPassword = configuration["AdminPassword"] ?? "password";
                }
                else
                {
                    adminPassword = "password";
                }
                
                var userManager = scope.ServiceProvider.GetService<UserManager<IdentityUser>>();
                var user = new IdentityUser(adminName);
                
                userManager.CreateAsync(user, adminPassword).GetAwaiter().GetResult();
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}