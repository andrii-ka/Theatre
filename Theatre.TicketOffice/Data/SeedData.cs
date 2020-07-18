using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Theatre.TicketOffice.Models;

namespace Theatre.TicketOffice.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                var adminID = await EnsureUser(serviceProvider, "Qwerty1!", "admin@theatre.com");
                await EnsureRole(serviceProvider, adminID, Constants.Roles.Administrator);

                await EnsureUser(serviceProvider, "Qwerty1!", "user@theatre.com");

                SeedDB(context);
            }
        }

        private static async Task<string> EnsureUser(IServiceProvider serviceProvider, string password, string userName)
        {
            var userManager = serviceProvider.GetService<UserManager<IdentityUser>>();

            var user = await userManager.FindByNameAsync(userName);
            if (user == null)
            {
                user = new IdentityUser { UserName = userName };
                await userManager.CreateAsync(user, password);
            }

            return user.Id;
        }

        private static async Task<IdentityResult> EnsureRole(IServiceProvider serviceProvider, string uid, string role)
        {
            IdentityResult result = null;
            var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();

            if (roleManager == null)
            {
                throw new Exception("roleManager null");
            }

            if (!await roleManager.RoleExistsAsync(role))
            {
                result = await roleManager.CreateAsync(new IdentityRole(role));
            }

            var userManager = serviceProvider.GetService<UserManager<IdentityUser>>();

            var user = await userManager.FindByIdAsync(uid);

            if (user == null)
            {
                throw new Exception("The password was probably not strong enough!");
            }

            result = await userManager.AddToRoleAsync(user, role);

            return result;
        }

        public static void SeedDB(ApplicationDbContext context)
        {
            if (context.Shows.Any())
            {
                return;   // DB has already been seeded
            }

            var shows = new[]
            {
                new Show { Name = "Jesus Christ Superstar" },
                new Show { Name = "Hamlet" },
            };
            context.Shows.AddRange(shows);
            context.SaveChanges();

            var showTimes = new[]
            {
                new ShowTime { ShowID = context.Shows.Single(s => s.Name == "Jesus Christ Superstar").ID, StartDateUtc = new DateTime(2020, 08, 01, 15, 0, 0, DateTimeKind.Utc), TicketsTotal = 250 },
                new ShowTime { ShowID = context.Shows.Single(s => s.Name == "Jesus Christ Superstar").ID, StartDateUtc = new DateTime(2020, 08, 02, 15, 0, 0, DateTimeKind.Utc), TicketsTotal = 250 },
                new ShowTime { ShowID = context.Shows.Single(s => s.Name == "Hamlet").ID, StartDateUtc = new DateTime(2020, 08, 01, 18, 0, 0, DateTimeKind.Utc), TicketsTotal = 500 },
                new ShowTime { ShowID = context.Shows.Single(s => s.Name == "Hamlet").ID, StartDateUtc = new DateTime(2020, 08, 02, 18, 0, 0, DateTimeKind.Utc), TicketsTotal = 500 },
            };
            context.ShowTimes.AddRange(showTimes);
            context.SaveChanges();
        }
    }
}
