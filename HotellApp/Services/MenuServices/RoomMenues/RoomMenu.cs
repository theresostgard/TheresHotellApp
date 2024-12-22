using HotellApp.Controllers.RoomController;
using HotellApp.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotellApp.Services.MenuServices.RoomMenues
{
    public class RoomMenu(IRoomController roomController) : IRoomMenu
    {

        private readonly IRoomController _roomController = roomController;

        private readonly string[] _menuItems =
            {
                "Lägg till nytt rum",
                "Visa ett rum",
                "Visa alla rum",
                "Uppdatera rum",
                "Ändra status på rum",
                "Tillbaka till huvudmenyn"
            };


        public void ShowRoomOptions()
        {
            int selectedIndex = 0;
            MenuRenderer.ShowMenu(_menuItems, ref selectedIndex, "Rumsmeny", HandleMenuSelection);
        }

        private void HandleMenuSelection(int selectedIndex)
        {
            string selectedItem = _menuItems[selectedIndex];
            switch (selectedItem)
            {
                case "Lägg till nytt rum":
                    _roomController.CreateRoomController();
                    break;
                case "Visa ett rum":
                    _roomController.ReadRoomController();
                    break;
                case "Visa alla rum":
                    _roomController.ReadAllRoomsController();
                    Console.ReadKey();
                    break;
                case "Uppdatera rum":
                    _roomController.UpdateRoomController();
                    break;
                case "Ändra status på rum":
                    _roomController.DeleteRoomController();
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
