using HotellApp.Models.Enums;
using HotellApp.Models;
using HotellApp.Services.RoomServices;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotellApp.Controllers.RoomController
{
    public class RoomController : IRoomController
    {
        private readonly IRoomService _roomService;

        public RoomController(IRoomService roomService)
        {
            _roomService = roomService;
        }
        public void CreateRoomController()
        {
            var newRoomId = AnsiConsole.Prompt(
            new TextPrompt<int>("Ange det nya rummets nummer:"));

            var roomType = AnsiConsole.Prompt(
                new TextPrompt<TypeOfRoom>("Ange om rummet är ett dubbelrum(2) eller enkelrum(1):"));

            var newRoomSize = AnsiConsole.Prompt(
                new TextPrompt<int>("Ange rummets storlek i kvm:"));

            var isExtraBedAllowed = ShouldAllowExtraBed(newRoomSize, roomType);

            var amountOfExtraBeds = isExtraBedAllowed
                ? AnsiConsole.Prompt(new TextPrompt<AmountOfExtraBedsAllowedInRoom>("Hur många extrasängar ska tillåtas? (1 eller 2)"))
                : AmountOfExtraBedsAllowedInRoom.None;

            Console.WriteLine($"Nytt rum med RumsID {newRoomId} är registrerat i systemet.");

            var room = new Room
            {
                RoomID = newRoomId,
                RoomType = roomType,
                RoomSize = newRoomSize,
                IsExtraBedAllowed = isExtraBedAllowed,
                AmountOfExtraBeds = amountOfExtraBeds
            };
            _roomService.CreateRoom(room);
            AnsiConsole.WriteLine("Nytt rum skapat.");
        }

        private bool ShouldAllowExtraBed(int roomSize, TypeOfRoom roomType)
        {
            if (roomSize > 15 && roomType == TypeOfRoom.Double)
            {
                return AnsiConsole.Prompt(
                    new TextPrompt<bool>("Rummet är tillräckligt stort för att kunna ha extrasäng, vill du tillåta det i rummet? (true/false):"));
            }
            return false;
        }

        public void ReadRoomController()
        {
            bool IsContinuingReading = true;

            while (IsContinuingReading)
            {
                Console.Clear();
                // var allRooms = _roomService.Rooms;

                var roomID = AnsiConsole.Prompt(
                new TextPrompt<int>("Ange Rummets ID: "));

                var room = _roomService.ReadRoom(roomID);

                if (room != null)
                {
                    AnsiConsole.WriteLine($"Rumsinfo:\n" +
                        $"ID: {room.RoomID}\n" +
                        $"Rumstyp: {room.RoomType}\n" +
                        $"Rumsstorlek: {room.RoomSize}\n" +
                        $"Extrasängar tillåtet?: {room.IsExtraBedAllowed}\n" +
                        $"Hur många extrasängar är tillåtet i rummet: {room.AmountOfExtraBeds}\n");
                }
                else
                {
                    AnsiConsole.WriteLine($"Inget rum med rumsnr {roomID} kunde hittas.");
                }

                var continueOption = AnsiConsole.Prompt(
                new TextPrompt<bool>("Vill du se information om ett annat rum? (true för ja, false för nej)"));

                if (!continueOption)
                {
                    IsContinuingReading = false;  // Stäng av loopen om användaren inte vill fortsätta
                }
            }
        }

        public void ReadAllRoomsController()
        {
            Console.Clear();
            var rooms = _roomService.GetAllRooms();
            if (rooms.Count == 0)
            {
                Console.WriteLine("Inga rum hittades.");
            }
            else
            {
                foreach (var room in rooms)
                {
                    Console.WriteLine("_________________________________________________________________\n");
                    Console.WriteLine($"Rumsnummer: {room.RoomID}\n" +
                        $"Rumstyp: {room.RoomType}\n " +
                        $"Rumsstorlek: {room.RoomSize}\n" +
                        $"Tillåtet med extrasäng? {room.IsExtraBedAllowed}\n" +
                        $"Antal extrasängar (om tillåtet): {room.AmountOfExtraBeds}\n" +
                        $"Rummets status: {room.Status}");
                    Console.WriteLine("\n_________________________________________________________________\n");
                }
            }
        }

        public void UpdateRoomController()
        {

            //om man inte anger att extrasäng är tillåtet ska man inte få frågan om antal, kolla upp detta!
            var roomId = AnsiConsole.Ask<int>("Ange rumsnummer för rummet du vill uppdatera:");
            var room = new Room
            {
                RoomID = roomId,
                RoomSize = AnsiConsole.Ask<int>("Ange rummet storlek: "),
                RoomType = AnsiConsole.Ask<TypeOfRoom>("Är rummet ett enkelrum (1) eller dubbelrum (2)?"),
                IsExtraBedAllowed = AnsiConsole.Ask<bool>("Är det tillåtet med extrasäng i rummet? (true/false)"),
                AmountOfExtraBeds = AnsiConsole.Ask<AmountOfExtraBedsAllowedInRoom>("Hur många extrasängar är tillåtet? (1/2)")
            };

            _roomService.UpdateRoom(roomId, room);
        }

        public void DeleteRoomController()
        {
            var roomId = AnsiConsole.Ask<int>("Ange rumsnummer för det rum du vill ändra status på: ");
            _roomService.DeleteRoom(roomId);
        }
    }
}
