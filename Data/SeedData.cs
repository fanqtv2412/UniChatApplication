using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MySqlX.XDevAPI.Common;
using UniChatApplication.Daos;
using UniChatApplication.Models;

namespace UniChatApplication.Data
{
    public class SeedData
    {

        public static void MigrateDB(IServiceProvider serviceProvider)
        {
            int i = 0;
            while (i < 5)
            {
                try
                {
                    using var scope = serviceProvider.CreateScope();
                    using var dbContext = scope.ServiceProvider.GetService<UniChatDbContext>();
                    dbContext.Database.EnsureCreated();
                    i = 5;
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.ToString());
                    i++;
                    Thread.Sleep(2000);
                    Console.WriteLine($"============ Retry time {i} ============");
                }
            }
        }
        /// <summary>
        /// InitialAdminAccount
        /// </summary>
        /// <param name="serviceProvider"></param>
        public static void InitialAdminAccount(IServiceProvider serviceProvider)
        {
            UniChatDbContext context =
                new UniChatDbContext(serviceProvider
                        .GetRequiredService<DbContextOptions<UniChatDbContext>>(
                        ));

            if (context.AdminProfile.Any()) return;

            List<AdminProfile> admins =
                new List<AdminProfile>()
                {
                    new AdminProfile()
                    {
                        FullName = "Admin",
                        Email = "admin@unichat.com",
                        Phone = "0987499512",
                        Gender = true,
                        Account = AccountDAOs.CreateAccount("admin", "12345678", 3)
                    }
                };

            context.AddRange(admins);
            context.SaveChanges();
        }

        public static void InitStudentAccount(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            using var dbContext = scope.ServiceProvider.GetService<UniChatDbContext>();

        }

        /// <summary>
        /// ResetDataServer
        /// </summary>
        /// <param name="serviceProvider"></param>
        public static void ResetDataServer(IServiceProvider serviceProvider)
        {

            UniChatDbContext context =
                new UniChatDbContext(serviceProvider
                        .GetRequiredService<DbContextOptions<UniChatDbContext>>(
                        ));

            // Reset Login Cookie
            List<LoginCookie> cookies = new List<LoginCookie>();
            foreach (LoginCookie item in context.LoginCookies)
            {
                if (item.ExpirationTime <= DateTime.Now)
                {
                    cookies.Add(item);
                }
            }
            context.LoginCookies.RemoveRange(cookies);
            context.SaveChanges();
        }

    }
}
