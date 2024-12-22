using HotellApp.Controllers.GuestController;
using HotellApp.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotellApp.Services.MenuServices.GuestMenu
{
    public class GuestMenu : IGuestMenu
    {
        private readonly IGuestController _guestController;
      

        public GuestMenu(IGuestController guestController)
        {
            _guestController = guestController;
        }

        private readonly string[] _menuItems =
        {
                "Skapa ny gäst",
                "Visa en gäst",
                "Visa alla gäster",
                "Uppdatera gäst",
                "Ta bort gäst",
                "Tillbaka till huvudmenyn"
        };



        public void ShowCustomerOptions()
        {
            int selectedIndex = 0;
            MenuRenderer.ShowMenu(_menuItems, ref selectedIndex, "Gästmeny", HandleMenuSelection);
        }

        private void HandleMenuSelection(int selectedIndex)
        {
          
            string selectedItem = _menuItems[selectedIndex];
            switch (selectedItem)
            {
                case "Skapa ny gäst":
                    Console.Clear();
                    _guestController.CreateGuestController();
                    break;
                case "Visa en gäst":
                    Console.Clear();
                    _guestController.ReadGuestController();
                    break;
                case "Visa alla gäster":
                    Console.Clear();
                    _guestController.ReadAllGuestsController();
                    Console.ReadKey();
                    break;
                case "Uppdatera gäst":
                    Console.Clear();
                    _guestController.UpdateGuestController(); 
                    break;
                case "Ta bort gäst":
                    Console.Clear();
                    _guestController.DeleteGuestController();    
                    break;
                case "Tillbaka till huvudmenyn":
                    Console.WriteLine("Tillbaka till huvudmenyn...");
                    return;
                default:
                    Console.WriteLine("Ogiltigt val.");
                    break;
            }
 

        }


    }
}
