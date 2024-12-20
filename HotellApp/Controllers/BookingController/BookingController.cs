using HotellApp.Models.Enums;
using HotellApp.Models;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotellApp.Services.BookingServices;
using HotellApp.Controllers.GuestController;
using HotellApp.Services.RoomServices;

namespace HotellApp.Controllers.BookingController
{
    public class BookingController : IBookingController
    {
        private readonly IBookingService _bookingService;
        private readonly IGuestController _guestController;
        private readonly IRoomService _roomService;

        public BookingController(IBookingService bookingService, IGuestController guestController, IRoomService roomService)
        {
            _bookingService = bookingService;
            _guestController = guestController;
            _roomService = roomService;
        }

        public void CreateBookingController()
        {
            var (guestType, guestId) = _guestController.SelectCustomerType();

            if (guestId == null)
            {
                Console.WriteLine("Kundval kunde inte genomföras. Avslutar bokningen.");
                return;
            }

            Console.WriteLine("Ange bokningsdetaljer:");

            DateTime arrivalDate;
            do
            {
                arrivalDate = AnsiConsole.Ask<DateTime>("Ankomstdatum (yyyy-MM-dd):");
                if (arrivalDate < DateTime.Now.Date) // Om ankomstdatumet är tidigare än dagens datum
                {
                    Console.WriteLine("Ankomstdatumet måste vara ett framtida datum. Försök igen.");
                }
            } while (arrivalDate < DateTime.Now.Date); // Loop tills ett giltigt datum anges

            

            DateTime departureDate;
            do
            {
                departureDate = AnsiConsole.Ask<DateTime>("Avresedatum (yyyy-MM-dd):");

                if (departureDate <= arrivalDate)
                {
                    Console.WriteLine("Avresedatumet måste vara efter ankomstdatumet. Försök igen.");
                }
            }
            while (departureDate <= arrivalDate);

            var roomType = AnsiConsole.Prompt(
                new SelectionPrompt<TypeOfRoom>()
                    .Title("Välj rumsyp:")
                    .AddChoices(TypeOfRoom.Single, TypeOfRoom.Double));

            var amountOfGuests = AnsiConsole.Ask<sbyte>("Antal gäster: ");
            int amountOfExtraBeds = 0;
            bool isExtraBedAllowed = false;

            if (roomType == TypeOfRoom.Double)
            {
                var isExtraBedWanted = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("Önskas extrasäng?")
                        .AddChoices("Ja", "Nej")
                ) == "Ja";

                if (isExtraBedWanted)
                {
                    amountOfExtraBeds = AnsiConsole.Ask<int>("Hur många extrasängar önskas?");
                }

                isExtraBedAllowed = true;
            }

            var amountOfRooms = AnsiConsole.Ask<sbyte>("Antal rum: ");

            // Kontrollera tillgänglighet av rummen
            var availableRooms = _roomService.GetAvailableRooms(roomType, arrivalDate, departureDate, amountOfRooms);

            if (availableRooms == null || availableRooms.Count < amountOfRooms)
            {
                Console.WriteLine("Det finns inte tillräckligt med tillgängliga rum för den valda rumstypen och datumintervallet.");
                return;
            }

            // Om rummen är tillgängliga, skapa bokningen
            var booking = new Booking
            {
                GuestId = guestId.Value,
                ArrivalDate = arrivalDate,
                DepartureDate = departureDate,
                RoomType = roomType,
                AmountOfGuests = amountOfGuests,
                AmountOfRooms = amountOfRooms,
                AmountOfExtraBeds = isExtraBedAllowed ? amountOfExtraBeds : 0
            };

            // Skapa bokningen först och få tillgång till BookingId
            _bookingService.CreateBooking(booking);

            // Nu när bokningen är skapad och har ett BookingId, skapa BookingRoom-poster för att koppla rum till bokningen
            var bookingRooms = new List<BookingRoom>();
            foreach (var availableRoomToReserve in availableRooms.Take(amountOfRooms)) // Se till att vi inte tilldelar fler rum än användaren vill ha
            {
                var bookingRoom = new BookingRoom
                {
                    RoomId = availableRoomToReserve.RoomId,
                    BookingId = booking.BookingId  // Koppla bokningen till rummet
                };
                bookingRooms.Add(bookingRoom);
            }

            foreach (var room in availableRooms.Take(amountOfRooms))
            {
                _roomService.ChangeRoomStatusForDateRange(room.RoomId, StatusOfRoom.Reserved, arrivalDate, departureDate);
            }
            // Lägg till de tilldelade rummen i databasen
            _bookingService.AddRoomsToBooking(bookingRooms);

            // Uppdatera rumsstatus till "Ockuperat" för de bokade rummen
            foreach (var room in availableRooms.Take(amountOfRooms))
            {
                _roomService.ChangeRoomStatusForDateRange(room.RoomId, StatusOfRoom.Reserved, arrivalDate, departureDate);
            }


            AnsiConsole.WriteLine($"Ny bokning skapad med bokningsnr {booking.BookingId}.");

        }

        public void ReadAllBookingsController()
        {
            var bookings = _bookingService.GetAllBookings();  // Hämtar alla bokningar

            if (bookings != null && bookings.Any())  // Kontrollera om det finns några bokningar
            {
                foreach (var booking in bookings)
                {
                    // Kontrollera om Booking.Guest inte är null
                    var guestName = booking.Guest != null ? $"{booking.Guest.FirstName} {booking.Guest.LastName}" : "Ingen gäst kopplad";

                    Console.WriteLine($"Bokning ID: {booking.BookingId}\n" +
                                      $"Gästens namn: {guestName}\n" +
                                      $"Ankomstdatum: {booking.ArrivalDate}\n" +
                                      $"Avresedatum: {booking.DepartureDate}\n" +
                                      $"Antal rum: {booking.AmountOfRooms}\n" +
                                      $"Antal gäster: {booking.AmountOfGuests}");

                    bool noRoomConnectedToBooking = booking.BookingRooms == null || !booking.BookingRooms.Any();

                    if (noRoomConnectedToBooking)
                    {
                        Console.WriteLine("Inga rum kopplade till denna bokning.");
                        continue;
                    }
                    foreach (var bookingRoom in booking.BookingRooms)
                    {
                        if (bookingRoom.Room != null)
                        {
                            var room = bookingRoom.Room;
                            Console.WriteLine($"Rum ID: {room.RoomId}, Rumstyp: {room.RoomType}, Storlek: {room.RoomSize} kvm\n" +
                                $"____________________________________________________");
                        }
                        else if(!noRoomConnectedToBooking)
                        {
                            Console.WriteLine("Rum kopplat till bokningen saknas.");
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("Inga bokningar hittades.");
            }
      
        }
        

        public void ReadBookingController()
        {
            ReadAllBookingsController();
            Console.WriteLine("Ska man kunna välja ett specifikt bokningsnr för att visa just den bokningen?");     //StudentDB har exempel jag kan modifiera
        }

        public void UpdateBookingController()
        {

            var id = AnsiConsole.Ask<int>("Ange bokningsnummer för att uppdatera bokning:");
            var booking = new Booking
            {
                BookingId = id,
                ArrivalDate = AnsiConsole.Ask<DateTime>("Nytt ankomstdatum (yyyy-MM-dd):"),
                DepartureDate = AnsiConsole.Ask<DateTime>("Nytt avresedatum (yyyy-MM-dd):"),
                RoomType = AnsiConsole.Ask<TypeOfRoom>("Önskad rumstyp: Enkelrum (1), Dubbelrum (2):"),
                AmountOfGuests = AnsiConsole.Ask<sbyte>("Antal gäster:"),
                AmountOfRooms = AnsiConsole.Ask<sbyte>("Antal rum:")
            };

            _bookingService.UpdateBooking(id, booking);
        }

        public void DeleteBookingController()
        {
            //visa lista med bokningar och datum så man vet vad man kan välja 
            var id = AnsiConsole.Ask<int>("Ange bokningsnummer för den bokning du vill radera:");

            var confirm = AnsiConsole.Confirm("Är du säker på att du vill radera bokningen?");
            if (!confirm)
            {
                Console.WriteLine("Radering avbruten.");
                return;
            }

            // Kalla på servicen för att försöka ta bort bokningen
            var result = _bookingService.DeleteBooking(id);

            // Visa resultatet till användaren
            AnsiConsole.MarkupLine($"[yellow]{result}[/]");
        }

    }
}
