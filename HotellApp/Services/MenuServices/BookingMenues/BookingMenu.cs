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

        public void ShowBookingOptions()
        {
            int selectedIndex = 0;
      
            MenuRenderer.ShowMenu(_menuItems, ref selectedIndex, "Bokningsmeny", HandleMenuSelection);
        }

        private void HandleMenuSelection(int selectedIndex)
        {

            string selectedItem = _menuItems[selectedIndex];
            switch (selectedItem)
            {
                case "Skapa bokning":
                    _bookingController.CreateBookingController();
                    break;
                case "Visa en bokning":
                    _bookingController.ReadBookingController();
                    break;
                case "Visa alla bokningar":
                    Console.Clear();
                    _bookingController.ReadAllBookingsController();
    
                    break;
                case "Uppdatera bokning":
                    Console.Clear();
                    _bookingController.UpdateBookingController();

                    break;
                case "Radera bokning":
                    Console.Clear();
                    _bookingController.DeleteBookingController();
   
                    break;
                case "Tillbaka till huvudmenyn":
     
                    return;
                default:
                    Console.WriteLine("Ogiltigt val.");
                    break;
            }

        }


    }
}
