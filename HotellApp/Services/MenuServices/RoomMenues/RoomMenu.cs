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
        //private readonly IServiceFactory _serviceFactory;
        //private readonly IRoomService _roomService;
        //private readonly MenuManager _menuManager;
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


        //_menuManager = new MenuManager(menuItems, HandleMenuSelection);
        //}

        public void ShowRoomOptions()
        {
            int selectedIndex = 0;
            MenuRenderer.ShowMenu(_menuItems, ref selectedIndex, "Rumsmeny", HandleMenuSelection);
            //_menuManager.ShowMenu();
        }

        private void HandleMenuSelection(int selectedIndex)
        {

            //var roomService = _serviceFactory.GetService<IRoomService>();
            string selectedItem = _menuItems[selectedIndex];
            switch (selectedItem)
            {
                case "Lägg till nytt rum":
                    //Console.Clear();
                    //_roomService.CreateRoom();
                    //MenuActionHelper.ExecuteMenuAction<IRoomService>
                    //(_serviceFactory, service => roomService.CreateRoom());
                    _roomController.CreateRoomController();
                    break;
                case "Visa ett rum":
                    //Console.Clear();
                    _roomController.ReadRoomController();
                    //_roomService.ReadRoom();
                    break;
                case "Visa alla rum":
                    _roomController.ReadAllRoomsController();
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

            Console.WriteLine("\nTryck på valfri tangent för att fortsätta...");
            Console.ReadKey();

        }

    }
}
