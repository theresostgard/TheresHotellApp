using HotellApp.Graphics;
using HotellApp.Services.MenuServices.BookingMenues;
using HotellApp.Services.MenuServices.GuestMenu;
using HotellApp.Services.MenuServices.InvoiceMenues;
using HotellApp.Services.MenuServices.RoomMenues;
using HotellApp.Services.ServiceFactory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotellApp.Services.MenuServices.MainMenues
{
    public class MainMenu : IMainMenu
    {

        private readonly IServiceFactory _serviceFactory;

        public MainMenu(IServiceFactory serviceFactory)
        {
            _serviceFactory = serviceFactory;
        }


        private readonly string[] _mainMenuItems =
        {
            "Bokning",
             "Gäst",
             "Rum",
             "Faktura",
             "Avsluta"
        };
        public void ShowMainMenu()
        {

            int selectedIndex = 0;
            MenuRenderer.ShowMenu(_mainMenuItems, ref selectedIndex, "Huvudmeny", HandleMenuSelection);
        }

        private void HandleMenuSelection(int selectedIndex)
        {
            string selectedItem = _mainMenuItems[selectedIndex];
            switch (selectedItem)
            {
                case "Bokning":
                    Console.Clear();
                    var bookingMenu = _serviceFactory.Get<IBookingMenu>();
                    bookingMenu.ShowBookingOptions();
                    break;
                case "Gäst":
                    Console.Clear();
                    var customerMenu = _serviceFactory.Get<IGuestMenu>();
                    customerMenu.ShowCustomerOptions();
                    break;

                case "Rum":
                    Console.Clear();
                    var roomMenu = _serviceFactory.Get<IRoomMenu>();
                    roomMenu.ShowRoomOptions();
                    break;

                case "Faktura":
                    Console.Clear();
                    var invoiceMenu = _serviceFactory.Get<IInvoiceMenu>();
                    invoiceMenu.ShowInvoiceOptions();
                    break;
                case "Avsluta":
                    Console.Clear();
                    Console.WriteLine("Avslutar programmet...");
                    Console.ReadKey();
                    Console.Clear();
                    Environment.Exit(0);        //för att få en avslutsscreen eller liknande så behövs väl en metod ovanför denna rad?


                    break;
            }
            Console.ReadKey();
        }
    }
}
