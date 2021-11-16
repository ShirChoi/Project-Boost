using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectBoost {
    public class Program {
        public static async Task Main(string[] args) {
            var host = CreateHostBuilder(args).Build();
            //new UserManager<User>().CreateAsync()
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
            try {
                //IdentityUser
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

                await ContextHelper.Seeding(roleManager);

                logger.LogInformation("Migrate successfull");
            } catch(Exception ex) {
                logger.LogError(ex.Message);
            }

            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
