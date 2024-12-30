
using Autofac;
using HotellApp.Controllers.BookingController;
using HotellApp.Controllers.GuestController;
using HotellApp.Controllers.RoomController;
using HotellApp.Data;
using HotellApp.Services.BookingServices;
using HotellApp.Services.GuestServices;
using HotellApp.Services.RoomServices;
using HotellApp.Services.ServiceFactorys;
using HotellApp.Services.GuestServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.ComponentModel;
using IContainer = Autofac.IContainer;
using HotellApp.Services.ServiceFactory;
using HotellApp.Utilities.ListDisplay;
using HotellApp.Utilities.DisplayGuest;
using HotellApp.Utilities.BookingDisplay;
using HotellApp.Utilities.RoomDisplay;
using HotellApp.Controllers.BookingCreationController;
using HotellApp.Utilities.GuestDisplay;
using HotellApp.Controllers.GuestInputValidatingControllers;
using HotellApp.Controllers.MenuServices.BookingMenues;
using HotellApp.Controllers.MenuServices.GuestMenues;
using HotellApp.Controllers.MenuServices.MainMenues;
using HotellApp.Controllers.MenuServices.RoomMenues;
using HotellApp.Controllers.MenuControllers.BookingMenues;
using HotellApp.Controllers.MenuControllers.GuestMenues;
using HotellApp.Controllers.MenuControllers.MainMenues;
using HotellApp.Controllers.MenuControllers.RoomMenues;


namespace HotellApp.Services
{
    public static class ContainerConfig
    {
        public static IContainer Configure()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<ApplicationDbContext>().AsSelf().InstancePerLifetimeScope();

            builder.RegisterType<GuestService>().As<IGuestService>().InstancePerLifetimeScope();
            builder.RegisterType<BookingService>().As<IBookingService>().InstancePerLifetimeScope();
            builder.RegisterType<RoomService>().As<IRoomService>().InstancePerLifetimeScope();

            // Registrera menyer och controllers
            builder.RegisterType<BookingMenu>().As<IBookingMenu>().InstancePerLifetimeScope();
            builder.RegisterType<GuestMenu>().As<IGuestMenu>().InstancePerLifetimeScope();
            builder.RegisterType<RoomMenu>().As<IRoomMenu>().InstancePerLifetimeScope();
            builder.RegisterType<MainMenu>().As<IMainMenu>().InstancePerLifetimeScope();

            // Registrera controllers med InstancePerLifetimeScope för att skapa nya instanser vid behov
            builder.RegisterType<BookingController>().As<IBookingController>().InstancePerLifetimeScope();
            builder.RegisterType<GuestController>().As<IGuestController>().InstancePerLifetimeScope();
            builder.RegisterType<RoomController>().As<IRoomController>().InstancePerLifetimeScope();
            builder.RegisterType<BookingCreationController>().As<IBookingCreationController>().InstancePerLifetimeScope();
            builder.RegisterType<GuestInputValidatingController>().As<IGuestInputValidatingController>().InstancePerLifetimeScope();
            // Registrera DataInitializer för att hantera seedning och migrering
            builder.RegisterType<DataInitializer>().As<IDataInitializer>().SingleInstance();

            // Registrera ServiceFactory
            builder.RegisterType<HotellApp.Services.ServiceFactorys.ServiceFactory>().As<IServiceFactory>().InstancePerLifetimeScope();

            //Utilities
            builder.RegisterType<DisplayLists>().As<IDisplayLists>().InstancePerLifetimeScope();
            builder.RegisterType<DisplayBooking>().As<IDisplayBooking>().InstancePerLifetimeScope();
            builder.RegisterType<DisplayRoom>().As<IDisplayRoom>().InstancePerLifetimeScope();
            builder.RegisterType<DisplayGuest>().As<IDisplayGuest>().InstancePerLifetimeScope();

           

            var container = builder.Build();

            return container;
        }
    }
}
