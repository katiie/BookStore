using Bookstore.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Bookstore.Data.Model
{
    public class DatabaseContext : DbContext
    {
        string connectionString = string.Empty;// "data source=./;initial catalog=teststoredb;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework;";

        private IOptions<AppSettingsModel> settings;
        public DatabaseContext()
        {

            IConfigurationBuilder builder = new ConfigurationBuilder();
            builder.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"));

            var root = builder.Build();
            connectionString = root.GetConnectionString("DefaultConnection");

           
           
        }
        //public DatabaseContext() : base() { }

        //public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) {

        //}


        public DbSet<User> Users { get; set; }
        public  DbSet<Book> Books { get; set; }
        public DbSet<Stock> Stocks { get; set; }

        public DbSet<Store> Stores { get; set; }




        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString);
            optionsBuilder.UseLazyLoadingProxies();
        }

    }
 
}
  
