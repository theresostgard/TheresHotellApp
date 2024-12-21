using HotellApp.Controllers.BookingController;
using HotellApp.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotellApp.Services.MenuServices.BookingMenues
{
    public class BookingMenu : IBookingMenu
    {

        //private readonly IServiceFactory _serviceFactory;
        //private readonly IBookingService _bookingService;
        private readonly IBookingController _bookingController;

        public BookingMenu(IBookingController bookingController)
        {
            _bookingController = bookingController;
        }


        private readonly string[] _menuItems =
            {
                "Skapa bokning",
                "Visa en bokning",
                "Visa alla bokningar",
                "Uppdatera bokning",
                "Radera bokning",
                "Tillbaka till huvudmenyn"
            };

        //    
        //}
        public void ShowBookingOptions()
        {
            int selectedIndex = 0;
            //_menuManager.ShowMenu();
            MenuRenderer.ShowMenu(_menuItems, ref selectedIndex, "Bokningsmeny", HandleMenuSelection);
        }

        private void HandleMenuSelection(int selectedIndex)
        {
            //var bookingService = _serviceFactory.GetService<IBookingService>();
            string selectedItem = _menuItems[selectedIndex];
            switch (selectedItem)
            {
                case "Skapa bokning":
                    //MenuActionHelper.ExecuteMenuAction<IBookingService>
                    //    (_serviceFactory, service => bookingService.CreateBooking());
                    _bookingController.CreateBookingController();
                    break;
                case "Visa en bokning":
                    _bookingController.ReadBookingController();
                    break;
                case "Visa alla bokningar":
                    Console.Clear();
                    _bookingController.ReadAllBookingsController();
                    //_bookingService.ReadBooking();
                    break;
                case "Uppdatera bokning":
                    Console.Clear();
                    _bookingController.UpdateBookingController();
                    //_bookingService.UpdateBooking();
                    break;
                case "Radera bokning":
                    Console.Clear();
                    _bookingController.DeleteBookingController();
                    //_bookingService.DeleteBooking();
                    break;
                case "Tillbaka till huvudmenyn":
                    //Console.WriteLine("Tillbaka till huvudmenyn...");
                    return;
                default:
                    Console.WriteLine("Ogiltigt val.");
                    break;
            }

            Console.WriteLine("\nTryck på valfri tangent för att fortsätta...");
            Console.ReadKey();

        }


    }
}
