using HotellApp.Models.Enums;
using HotellApp.Models;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotellApp.Services.BookingServices;
using HotellApp.Services.RoomServices;
using Microsoft.EntityFrameworkCore;

namespace HotellApp.Controllers.BookingCreationController
{
    public class BookingCreationController : IBookingCreationController
    {
        private readonly IRoomService _roomService;
        private readonly IBookingService _bookingService;

        public BookingCreationController(IRoomService roomService, IBookingService bookingService)
        {
            _roomService = roomService;
            _bookingService = bookingService;   
        }
        public DateTime GetValidArrivalDate()
        {
            DateTime arrivalDate;
            do
            {
                arrivalDate = AnsiConsole.Ask<DateTime>("Ankomstdatum (yyyy-MM-dd):");
                if (arrivalDate < DateTime.Now.Date)
                {
                    Console.WriteLine("Ankomstdatumet måste vara ett framtida datum. Försök igen.");
                }
            } while (arrivalDate < DateTime.Now.Date);
            return arrivalDate;
        }

        public DateTime GetValidDepartureDate(DateTime arrivalDate)
        {
            DateTime departureDate;
            do
            {
                departureDate = AnsiConsole.Ask<DateTime>("Avresedatum (yyyy-MM-dd):");
                if (departureDate <= arrivalDate)
                {
                    Console.WriteLine("Avresedatumet måste vara efter ankomstdatumet. Försök igen.");
                }
            } while (departureDate <= arrivalDate);
            return departureDate;
        }

        public (TypeOfRoom roomType, sbyte amountOfGuests, sbyte amountOfRooms, int amountOfExtraBeds) GetRoomDetails()
        {
            var roomType = AnsiConsole.Prompt(
                new SelectionPrompt<TypeOfRoom>()
                    .Title("Välj rumsyp:")
                    .AddChoices(TypeOfRoom.Single, TypeOfRoom.Double));

            var amountOfGuests = AnsiConsole.Ask<sbyte>("Antal gäster: ");
            var amountOfRooms = AnsiConsole.Ask<sbyte>("Antal rum: ");

            var amountOfExtraBeds = 0;

            // Fråga om extrasängar om rummet är av typen Double
            if (roomType == TypeOfRoom.Double)
            {
                var isExtraBedWanted = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("Önskas extrasäng?")
                        .AddChoices("Ja", "Nej")
                ) == "Ja";

                if (isExtraBedWanted)
                {
                    amountOfExtraBeds = AnsiConsole.Prompt(
                         new SelectionPrompt<int>()
                         .Title("Hur många extrasängar önskas?")
                         .AddChoices(1, 2));
                }
            }
            return (roomType, amountOfGuests, amountOfRooms, amountOfExtraBeds);
        }


        public List<Room> CheckRoomAvailability(TypeOfRoom roomType,
            DateTime arrivalDate,
            DateTime departureDate,
            sbyte amountOfRooms)
        {
            var availableRooms = _roomService.GetAvailableRooms(
                roomType,
                arrivalDate,
                departureDate,
                amountOfRooms);

            if (availableRooms == null || availableRooms.Count < amountOfRooms)
            {
                Console.WriteLine("Det finns inte tillräckligt med tillgängliga rum för den valda rumstypen och datumintervallet.");
                return null;
            }
            return availableRooms;
        }

        public List<Room> FilterRoomsForExtraBeds(List<Room> availableRooms, bool isExtraBedAllowed)
        {
            if (isExtraBedAllowed)
            {
                availableRooms = availableRooms
                    .Where(r => r.RoomSize >= 15)
                    .ToList();
            }
            return availableRooms;
        }

        public void ShowAvailableRooms(List<Room> availableRooms)
        {
            var table = new Table();
            table.AddColumn("Rum #");
            table.AddColumn("Typ");
            //table.AddColumn("Status");
            table.AddColumn("Storlek");

            foreach (var room in availableRooms.Where(r => r.Status == StatusOfRoom.Active))
            {
                table.AddRow(room.RoomId.ToString(),
                    room.RoomType.ToString(),
                    //room.Status.ToString(),
                    room.RoomSize.ToString());
            }

            AnsiConsole.Write(table);
        }
        public List<Room> SelectRooms(List<Room> availableRooms, sbyte amountOfRooms)
        {
            var activeRooms = availableRooms.Where(r => r.Status == StatusOfRoom.Active).ToList();
            var selectedRooms = AnsiConsole.Prompt(
            new MultiSelectionPrompt<Room>()
             .Title($"Välj {amountOfRooms} rum att boka:")
             .AddChoices(activeRooms)
             .UseConverter(room => $"# {room.RoomId} ({room.RoomType})"))
             .ToList();

            // Kontrollera att användaren har valt exakt det antal rum
            if (selectedRooms.Count != amountOfRooms)
            {
                Console.Clear();
                return new List<Room>();  // Retur av en tom lista för att indikera ett ogiltigt val
            }

            return selectedRooms;  // Retur av de valda rummen om antalet matchar
        }

       
        public Booking CreateBooking(int guestId,
            DateTime arrivalDate,
            DateTime departureDate,
            TypeOfRoom roomType,
            sbyte amountOfGuests,
            int amountOfRooms)
        {
            var booking = new Booking
            {
                GuestId = guestId,
                ArrivalDate = arrivalDate,
                DepartureDate = departureDate,
                RoomType = roomType,
                AmountOfGuests = amountOfGuests,
                AmountOfRooms = (sbyte)amountOfRooms
            };

            _bookingService.CreateBooking(booking);
            return booking;
        }
        public void AssignRoomsToBooking(List<Room> selectedRooms,
            Booking booking,
            DateTime arrivalDate,
            DateTime departureDate)
        {
            var bookingRooms = selectedRooms.Select(room => new BookingRoom
            {
                RoomId = room.RoomId,
                BookingId = booking.BookingId
            }).ToList();

            _bookingService.AddRoomsToBooking(bookingRooms);

            foreach (var room in selectedRooms)
            {
                _roomService.ChangeRoomStatusForDateRange(room.RoomId,
                    StatusOfRoom.Reserved,
                    arrivalDate,
                    departureDate);
            }
        }
    }
}
