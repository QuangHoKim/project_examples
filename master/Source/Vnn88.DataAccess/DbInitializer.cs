using System;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Vnn88.Common.Infrastructure;
using Vnn88.DataAccess.Models;

namespace Vnn88.DataAccess
{
    /// <summary>
    /// the class to seed date to database server when application start up.
    /// </summary>
    public static class DbInitializer
    {
        public static void Initialize(AppDbContext context, IConfiguration configuration)
        {
            if (!((RelationalDatabaseCreator) context.Database.GetService<IDatabaseCreator>()).Exists())
            {
                context.Database.EnsureCreated();
                // Look for any accounts.
                if (!context.Users.Any())
                {
                    var account = new Users
                    {
                        UserName = configuration[Constants.Settings.DefaultUsername],
                        Password = Encryptor.CalculateHash(configuration[Constants.Settings.DefaultPassword]),
                        Role = (int)Role.Admin,
                        CreateDate = DateTime.Now,
                        ExpireDate = DateTime.Now.AddYears(100)
                    };
                    context.Users.Add(account);
                    context.SaveChanges();
                }
            }
        }
    }
}
