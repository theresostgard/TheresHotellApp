﻿using HotellApp.Models.Enums;
using HotellApp.Models;
using HotellApp.Services.RoomServices;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotellApp.Utilities.ListDisplay;
using HotellApp.Utilities.RoomDisplay;

namespace HotellApp.Controllers.RoomController
{
    public class RoomController : IRoomController
    {
        private readonly IRoomService _roomService;
        private readonly IDisplayLists _displayLists;
        private readonly IDisplayRoom _displayRoom;

        public RoomController(IRoomService roomService, 
            IDisplayLists displayLists, 
            IDisplayRoom displayRoom)
        {
            _roomService = roomService;
            _displayLists = displayLists;
            _displayRoom = displayRoom;
        }
        public void CreateRoomController()
        {
            Console.Clear();
            AnsiConsole.WriteLine("Skapa nytt rum:\n\n");
            var roomType = AnsiConsole.Prompt(
                new SelectionPrompt<TypeOfRoom>()
                    .Title("Välj rumsyp:")
                    .AddChoices(TypeOfRoom.Single, TypeOfRoom.Double));

            var newRoomSize = AnsiConsole.Prompt(
                new TextPrompt<int>("Ange rummets storlek i kvm:")
                    .Validate(size =>
                    {
                        return size > 0
                            ? ValidationResult.Success()
                            : ValidationResult.Error("[red]Rummets storlek måste vara större än 0.[/]");

                    }));

            var isExtraBedAllowed = ShouldAllowExtraBed(newRoomSize, roomType);

            var amountOfExtraBeds = AmountOfExtraBedsAllowedInRoom.None;

            if (isExtraBedAllowed)
            {
                amountOfExtraBeds = newRoomSize > 30
                    ? AnsiConsole.Prompt(
                        new SelectionPrompt<AmountOfExtraBedsAllowedInRoom>()
                            .Title("Hur många extrasängar ska tillåtas?")
                            .AddChoices(AmountOfExtraBedsAllowedInRoom.One, AmountOfExtraBedsAllowedInRoom.Two))
                    : AmountOfExtraBedsAllowedInRoom.One; 
            }

            var room = new Room
            {
                RoomType = roomType,
                RoomSize = newRoomSize,
                IsExtraBedAllowed = isExtraBedAllowed,
                AmountOfExtraBeds = amountOfExtraBeds
            };
            _roomService.CreateRoom(room);

            AnsiConsole.WriteLine($"Nytt rum skapades med rumsnr: {room.RoomId}\n" +
                $"Rumstyp: {room.RoomType}\nStorlek: {room.RoomSize} kvm.");
            Console.ReadKey();
        }

        public bool ShouldAllowExtraBed(int roomSize, TypeOfRoom roomType)
        {
            if (roomType == TypeOfRoom.Double)
            {
                if (roomSize > 15 && roomSize < 30)
                {
                    return AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                            .Title("Rummet är tillräckligt stort för att ha en extrasäng. Vill du tillåta det?")
                            .AddChoices("Ja", "Nej")) == "1 = Ja";
                }
                else if (roomSize > 30)
                {
                    return true; 
                }
            }
            return false; 
        }

        public void ReadRoomController()
        {
            bool IsContinuingReading = true;

            while (IsContinuingReading)
            {
                Console.Clear();
                _displayLists.DisplayRooms();

                var roomID = AnsiConsole.Prompt(
                new TextPrompt<int>("Ange Rummets ID: "));

                Console.Clear();
                var room = _roomService.ReadRoom(roomID);

                if (room != null)
                {
                  _displayRoom.DisplayRoomInformation(room);
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
                    IsContinuingReading = false;  
                }
            }
        }

        public void ReadAllRoomsController()
        {
            Console.Clear();
            var rooms = _roomService.GetAllRooms();
            if (rooms == null || rooms.Count == 0)
            {
                AnsiConsole.MarkupLine("[red]Inga rum hittades![/]");
                return;
            }
            _displayRoom.Pagination(rooms);
        }

        public void UpdateRoomController()
        {
            Console.Clear();

            bool continueUpdatingRooms = true;

            while (continueUpdatingRooms) 
            {
                Console.Clear();
                _displayLists.DisplayRooms();
                
                var roomId = AnsiConsole.Ask<int>("Ange rumsnummer för rummet du vill uppdatera:");

                var currentRoom = _roomService.ReadRoom(roomId);

                if (currentRoom == null)
                {
                    AnsiConsole.MarkupLine("[red]Inget rum med det rumsnumret kunde hittas![/]");
                    if (!AnsiConsole.Confirm("Vill du försöka igen?"))
                    {
                        return;
                    }
                }
                else
                {
                    var updatedRoom = GetRoomDetailsFromUser(currentRoom);
                    _roomService.UpdateRoom(roomId, updatedRoom);
                    AnsiConsole.MarkupLine($"[green]Rummet med ID {roomId} har uppdaterats.[/]");
                }
                continueUpdatingRooms = AnsiConsole.Confirm("Vill du uppdatera ett till rum?");
            }
        }
        public void ChangeStatusOnRoomController()
        {
            Console.Clear();

            bool continueChangingStatus = true;

            while (continueChangingStatus) 
            {
                Console.Clear();
                _displayLists.DisplayRooms();
                var roomId = AnsiConsole.Ask<int>("Ange rumsnummer för det rum du vill ändra status på: ");

                var currentRoom = _roomService.ReadRoom(roomId);

                if (currentRoom == null)
                {

                    AnsiConsole.MarkupLine("[red]Inget rum med det rumsnumret kunde hittas![/]");
                    if (!AnsiConsole.Confirm("Vill du försöka igen?"))
                    {
                        return;
                    }
                }
                else
                {
                    var newStatus = AnsiConsole.Prompt(
                        new SelectionPrompt<StatusOfRoom>()
                            .Title("Välj den nya statusen på rummet: ")
                            .AddChoices(StatusOfRoom.Active, StatusOfRoom.InActive, StatusOfRoom.UnderMaintenance));

                    var result = _roomService.ChangeStatusOnRoom(roomId, newStatus); 

                    if (result)
                    {
                        AnsiConsole.MarkupLine($"Statusen för rum [green]{roomId}[/] har ändrats till [green]{newStatus}[/].");
                        Console.ReadKey();
                    }
                    else
                    {
                        AnsiConsole.WriteLine("Statusändringen misslyckades.");
                        Console.ReadKey();
                    }
                }

                continueChangingStatus = AnsiConsole.Confirm("Vill du ändra status på ett till rum?");
            }
        }


        public Room GetRoomDetailsFromUser(Room currentRoom)
        {
            Console.Clear();
           
            var roomType = AnsiConsole.Prompt(
                new SelectionPrompt<TypeOfRoom>()
                    .Title($"Nuvarande rumstyp: {currentRoom.RoomType}. Välj ny rumstyp:")
                    .AddChoices(TypeOfRoom.Single, TypeOfRoom.Double));

            
            var roomSize = AnsiConsole.Ask<int>($"Nuvarande storlek: {currentRoom.RoomSize} kvm.\nAnge ny storlek:");


            bool canHaveExtraBed = roomSize >= 15 && currentRoom?.RoomType == TypeOfRoom.Double; 

            var isExtraBedAllowed = false;

            if (canHaveExtraBed)
            {
                isExtraBedAllowed = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title($"Nuvarande status för extrasäng: {(currentRoom?.IsExtraBedAllowed.GetValueOrDefault() ?? false ? "Ja" : "Nej")}. Tillåta extrasäng?")
                        .AddChoices("Ja", "Nej")) == "Ja";
            }
            else
            {
                if (currentRoom?.RoomType != TypeOfRoom.Double)
                {
                    AnsiConsole.WriteLine("Extrasäng är endast tillåtet för dubbelrum.");
                }
                else
                {
                    AnsiConsole.WriteLine("Rummet är för litet för att tillåta extrasäng.");
                }
            }

            AmountOfExtraBedsAllowedInRoom amountOfExtraBeds = AmountOfExtraBedsAllowedInRoom.None;
            if (isExtraBedAllowed)
            {
                if (roomSize >= 15 && roomSize <= 30)
                {
                    amountOfExtraBeds = AnsiConsole.Prompt(
                        new SelectionPrompt<AmountOfExtraBedsAllowedInRoom>()
                            .Title($"Nuvarande antal extrasängar: {currentRoom?.AmountOfExtraBeds
                            .ToString() ?? "0"}. Hur många extrasängar?")
                            .AddChoices(AmountOfExtraBedsAllowedInRoom.One));
                }
                else if (roomSize > 30)
                {
                    amountOfExtraBeds = AnsiConsole.Prompt(
                        new SelectionPrompt<AmountOfExtraBedsAllowedInRoom>()
                            .Title($"Nuvarande antal extrasängar: {currentRoom?.AmountOfExtraBeds
                            .ToString() ?? "0"}. Hur många extrasängar?")
                            .AddChoices(AmountOfExtraBedsAllowedInRoom.One, AmountOfExtraBedsAllowedInRoom.Two));
                }
                else
                {
                    AnsiConsole.WriteLine("Rummet är för litet för att tillåta två extrasängar.");
                }
            }

            AnsiConsole.MarkupLine($"[yellow]Nuvarande pris per natt: {currentRoom.PricePerNight} kr[/]");

            decimal pricePerNight = AnsiConsole.Prompt(
                new TextPrompt<decimal>("Ange det nya priset per natt för rummet:")
                    .Validate(value =>
                    {
                        if (value <= 0)
                        {
                            return ValidationResult.Error("Priset måste vara ett positivt tal.");
                        }
                        return ValidationResult.Success();
                    })
            );


            return new Room
            {
                RoomType = roomType,
                RoomSize = roomSize,
                IsExtraBedAllowed = isExtraBedAllowed,
                AmountOfExtraBeds = amountOfExtraBeds,
                PricePerNight = pricePerNight
            };
        }
        public List<Room> CheckRoomAvailability(TypeOfRoom roomType, DateTime arrivalDate, DateTime departureDate, int amountOfRooms)
        {
            var availableRooms = _roomService.GetAvailableRooms(roomType, arrivalDate, departureDate, (sbyte)amountOfRooms);
            return availableRooms;
        }

        public void ChangeRoomStatusForBooking(List<Room> rooms, DateTime arrivalDate, DateTime departureDate)
        {
            foreach (var room in rooms)
            {
                _roomService.ChangeRoomStatusForDateRange(room.RoomId, StatusOfRoom.Reserved, arrivalDate, departureDate);
            }
        }

       

    }
}
