using Autofac;
using HotellApp.Controllers.MenuServices.MainMenues;
using HotellApp.Data;
using HotellApp.Services;
using HotellApp.Utilities.Screens;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotellApp
{
    public class Application
    {
        private readonly Autofac.IContainer _container;

        public Application(Autofac.IContainer container)
        {
            _container = container;
        }

        public void Run()
        {
            Console.Title = "TheresHotell";

            StartScreen.Print();

            var builder = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.json", true, true);
            var config = builder.Build();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>();
            var connectionString = config.GetConnectionString("DefaultConnection");
            options.UseSqlServer(connectionString);

            using (var dbContext = new ApplicationDbContext(options.Options))
            {
                var dataInitializer = new DataInitializer();
                dataInitializer.MigrateAndSeed(dbContext); 
            }

            
            using (var scope = _container.BeginLifetimeScope())  
            {

                var mainMenu = scope.Resolve<IMainMenu>();  
                mainMenu.ShowMainMenu();  
            }

        }
    }
}
