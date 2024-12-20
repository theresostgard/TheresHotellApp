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
            AnsiConsole.WriteLine("Skapa nytt rum:\n\n");
            // Välj rumstyp
            var roomType = AnsiConsole.Prompt(
                new SelectionPrompt<TypeOfRoom>()
                    .Title("Välj rumsyp:")
                    .AddChoices(TypeOfRoom.Single, TypeOfRoom.Double));

            // Fråga om rummets storlek
            var newRoomSize = AnsiConsole.Prompt(
                new TextPrompt<int>("Ange rummets storlek i kvm:")
                    .Validate(size =>
                    {
                        return size > 0
                            ? ValidationResult.Success()
                            : ValidationResult.Error("[red]Rummets storlek måste vara större än 0.[/]");

                    }));

            var isExtraBedAllowed = ShouldAllowExtraBed(newRoomSize, roomType);

            // Fråga om antal extrasängar om det är tillåtet
            var amountOfExtraBeds = isExtraBedAllowed
                ? AnsiConsole.Prompt(
                    new SelectionPrompt<AmountOfExtraBedsAllowedInRoom>()
                        .Title("Hur många extrasängar ska tillåtas?")
                        .AddChoices(AmountOfExtraBedsAllowedInRoom.One, AmountOfExtraBedsAllowedInRoom.Two))
                : AmountOfExtraBedsAllowedInRoom.None;
            // Skapa rummet och lägg till det via tjänsten
            var room = new Room
            {
                RoomType = roomType,
                RoomSize = newRoomSize,
                IsExtraBedAllowed = isExtraBedAllowed,
                AmountOfExtraBeds = amountOfExtraBeds
            };
            _roomService.CreateRoom(room);

            AnsiConsole.WriteLine($"Nytt rum skapades med rumsnr: {room.RoomID}\n" +
                $"Rumstyp: {room.RoomType}\nStorlek: {room.RoomSize} kvm.");
        }

        public bool ShouldAllowExtraBed(int roomSize, TypeOfRoom roomType)
        {
            if (roomSize > 15 && roomType == TypeOfRoom.Double)
            {
                return AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("Rummet är tillräckligt stort för att ha en extrasäng. Vill du tillåta det?")
                        .AddChoices("1 = Ja", "2 = Nej")) == "1 = Ja";
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
                        new SelectionPrompt<string>()
                        .Title("Vill du se information om ett annat rum?")
                        .AddChoices("Ja", "Nej"));

                if (continueOption == "Nej")
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
         
            var roomId = AnsiConsole.Ask<int>("Ange rumsnummer för rummet du vill uppdatera:");

            var currentRoom = _roomService.ReadRoom(roomId); 

            // Hämta nya detaljer om rummet
            var updatedRoom = GetRoomDetailsFromUser(currentRoom);
            
            // Uppdatera rummet via tjänsten
            _roomService.UpdateRoom(roomId, updatedRoom);
            AnsiConsole.WriteLine($"Rummet med ID {roomId} har uppdaterats.");
        }

        public void DeleteRoomController()
        {
            //få in en lista med rumsnummer och status på rummen
            var roomId = AnsiConsole.Ask<int>("Ange rumsnummer för det rum du vill ändra status på: ");
            var newStatus = AnsiConsole.Prompt(
                new SelectionPrompt<StatusOfRoom>()
                .Title("Ändra status på rummet: Välj den nya statusen på rummet: ")
                .AddChoices(StatusOfRoom.Active, StatusOfRoom.InActive));
                _roomService.DeleteRoom(roomId, newStatus);
            AnsiConsole.WriteLine($"Statusen för rum {roomId} har ändrats till {newStatus}.");
        }

        public Room GetRoomDetailsFromUser(Room currentRoom)
        {
           
            var roomType = AnsiConsole.Prompt(
                new SelectionPrompt<TypeOfRoom>()
                    .Title($"Nuvarande rumstyp: {currentRoom.RoomType}. Välj ny rumstyp:")
                    .AddChoices(TypeOfRoom.Single, TypeOfRoom.Double));

            
            var roomSize = AnsiConsole.Ask<int>($"Nuvarande storlek: {currentRoom.RoomSize} kvm. Ange ny storlek:");

            
            bool canHaveExtraBed = roomSize >= 15;

            
            var isExtraBedAllowed = false;
            if (canHaveExtraBed)
            {
                isExtraBedAllowed = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                          .Title($"Nuvarande status för extrasäng: {(currentRoom?.IsExtraBedAllowed
                          .GetValueOrDefault() ?? false ? "Ja" : "Nej")}. Tillåta extrasäng?")
                        .AddChoices("Ja", "Nej")) == "Ja";
            }
            else
            {
                
                AnsiConsole.WriteLine("Rummet är för litet för att tillåta extrasäng.");
            }

            
            var amountOfExtraBeds = isExtraBedAllowed
                ? AnsiConsole.Prompt(
                    new SelectionPrompt<AmountOfExtraBedsAllowedInRoom>()
                        .Title($"Nuvarande antal extrasängar: {currentRoom?.AmountOfExtraBeds
                        .ToString() ?? "0"}. Hur många extrasängar?")
                        .AddChoices(AmountOfExtraBedsAllowedInRoom.One, AmountOfExtraBedsAllowedInRoom.Two))
                : AmountOfExtraBedsAllowedInRoom.None;

            
            return new Room
            {
                RoomType = roomType,
                RoomSize = roomSize,
                IsExtraBedAllowed = isExtraBedAllowed,
                AmountOfExtraBeds = amountOfExtraBeds
            };
        }
    }
}
