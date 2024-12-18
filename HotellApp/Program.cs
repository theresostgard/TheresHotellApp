using Autofac;
using Autofac.Extensions.DependencyInjection; // Detta behövs för att använda Autofac med .NET Core DI
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using HotellApp.Data;
using Microsoft.EntityFrameworkCore;
using HotellApp.Services;

namespace HotellApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var container = ContainerConfig.Configure();
            var app = new Application(container);
            app.Run();

        }
    }
}
