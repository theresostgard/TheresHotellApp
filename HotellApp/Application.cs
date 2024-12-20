using Autofac;
using HotellApp.Data;
using HotellApp.Services;
using HotellApp.Services.MenuServices.MainMenues;
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
           
            var builder = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.json", true, true);
            var config = builder.Build();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>();
            var connectionString = config.GetConnectionString("DefaultConnection");
            options.UseSqlServer(connectionString);

            using (var dbContext = new ApplicationDbContext(options.Options))
            {
                var dataInitiaizer = new DataInitializer();
                dataInitiaizer.MigrateAndSeed(dbContext); 
            }

            
            using (var scope = _container.BeginLifetimeScope())  // Börja ett nytt scope för DI
            {
                // Lösa MainMenu från container via DI
                var mainMenu = scope.Resolve<IMainMenu>();  // Löser MainMenu via DI
                mainMenu.ShowMainMenu();  // Visar huvudmenyn
            }

        }
    }
}
