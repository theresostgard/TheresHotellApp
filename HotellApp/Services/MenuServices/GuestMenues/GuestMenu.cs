using HotellApp.Controllers.GuestController;
using HotellApp.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotellApp.Services.MenuServices.GuestMenu
{
    public class GuestMenu(IGuestController guestController) : IGuestMenu
    {
        private readonly IGuestController _guestController;
        //private readonly IServiceFactory _serviceFactory;
        //private readonly MenuManager _menuManager;


        private readonly string[] _menuItems =
        {
                "Skapa kund",
                "Visa en kund",
                "Visa alla kunder",
                "Uppdatera kund",
                "Ta bort kund",
                "Tillbaka till huvudmenyn"
        };



        public void ShowCustomerOptions()
        {
            int selectedIndex = 0;
            MenuRenderer.ShowMenu(_menuItems, ref selectedIndex, "Kundmeny", HandleMenuSelection);
        }

        private void HandleMenuSelection(int selectedIndex)
        {
            //var customerService = _serviceFactory.GetService<ICustomerService>();
            string selectedItem = _menuItems[selectedIndex];
            switch (selectedItem)
            {
                case "Skapa kund":
                    Console.Clear();
                    _guestController.CreateGuestController();
                    Console.ReadKey();
                    break;
                case "Visa en kund":
                    Console.Clear();
                    _guestController.ReadGuestController();
                    Console.ReadKey();
                    break;
                case "Visa alla kunder":
                    Console.Clear();
                    _guestController.ReadAllGuestsController();
                    Console.ReadKey();
                    break;
                case "Uppdatera kund":
                    Console.Clear();
                    _guestController.UpdateGuestController();
                    Console.ReadKey();
                    break;
                case "Ta bort kund":
                    Console.Clear();
                    _guestController.DeleteGuestController();
                    Console.ReadKey();
                    break;
                case "Tillbaka till huvudmenyn":
                    Console.WriteLine("Tillbaka till huvudmenyn...");
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
