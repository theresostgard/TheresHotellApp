
using Autofac;
using HotellApp.Controllers.BookingController;
using HotellApp.Controllers.GuestController;
using HotellApp.Controllers.InvoiceController;
using HotellApp.Controllers.RoomController;
using HotellApp.Data;
using HotellApp.Services.BookingServices;
using HotellApp.Services.GuestServices;
using HotellApp.Services.InvoiceServices;
using HotellApp.Services.MenuServices.BookingMenues;
using HotellApp.Services.MenuServices.GuestMenu;
using HotellApp.Services.MenuServices.InvoiceMenues;
using HotellApp.Services.MenuServices.MainMenues;
using HotellApp.Services.MenuServices.RoomMenues;
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
            builder.RegisterType<InvoiceService>().As<IInvoiceService>().InstancePerLifetimeScope();

            // Registrera menyer och controllers
            builder.RegisterType<BookingMenu>().As<IBookingMenu>().InstancePerLifetimeScope();
            builder.RegisterType<GuestMenu>().As<IGuestMenu>().InstancePerLifetimeScope();
            builder.RegisterType<RoomMenu>().As<IRoomMenu>().InstancePerLifetimeScope();
            builder.RegisterType<InvoiceMenu>().As<IInvoiceMenu>().InstancePerLifetimeScope();
            builder.RegisterType<MainMenu>().As<IMainMenu>().InstancePerLifetimeScope();

            // Registrera controllers med InstancePerLifetimeScope för att skapa nya instanser vid behov
            builder.RegisterType<BookingController>().As<IBookingController>().InstancePerLifetimeScope();
            builder.RegisterType<GuestController>().As<IGuestController>().InstancePerLifetimeScope();
            builder.RegisterType<RoomController>().As<IRoomController>().InstancePerLifetimeScope();
            builder.RegisterType<InvoiceController>().As<IInvoiceController>().InstancePerLifetimeScope();
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
