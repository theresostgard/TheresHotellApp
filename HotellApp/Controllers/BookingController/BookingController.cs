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
            
            var arrivalDate = AnsiConsole.Ask<DateTime>("Ankomstdatum (yyyy-MM-dd):");
            var departureDate = AnsiConsole.Ask<DateTime>("Avresedatum (yyyy-MM-dd):");

            var roomType = AnsiConsole.Prompt(
                new SelectionPrompt<TypeOfRoom>()
                    .Title("Välj rumsyp:")
                    .AddChoices(TypeOfRoom.Single, TypeOfRoom.Double));

            var amountOfGuests = AnsiConsole.Ask<sbyte>("Antal gäster: ");
            int amountOfExtraBeds = 0;
            bool isExtraBedAllowed = false;

            if (roomType == TypeOfRoom.Double)
            {
                // Fråga om extrasäng om det är ett dubbelrum
                var isExtraBedWanted = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("Önskas extrasäng?")
                        .AddChoices("Ja", "Nej")
                ) == "Ja";

                if (isExtraBedWanted)
                {
                    amountOfExtraBeds = AnsiConsole.Ask<int>
                        ("Hur många extrasängar önskas?");
                }

                isExtraBedAllowed = true;
            }

            var amountOfRooms = AnsiConsole.Ask<sbyte>("Antal rum: ");

            // Kontrollera tillgänglighet av rummen
            var availableRooms = _roomService.GetAvailableRooms(roomType, arrivalDate, departureDate, amountOfRooms);

            if (!_bookingService.TryGetAvailableRoomsForBooking(roomType, arrivalDate, departureDate, amountOfRooms, out availableRooms))
            {
                Console.WriteLine("Inga tillgängliga rum för den valda rumstypen och datumintervallet.");
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

            var bookingRooms = new List<BookingRoom>();
            foreach (var availableRoomToReserve in availableRooms)
            {
                var bookingRoom = new BookingRoom
                {
                    RoomId = availableRoomToReserve.RoomId,
                    BookingId = booking.BookingId,  // Koppla bokningen till rummet
                    Room = availableRoomToReserve // Om du vill att rummet ska vara med i objektet
                };
                bookingRooms.Add(bookingRoom);
            }
                _bookingService.CreateBooking(booking, availableRooms);

            AnsiConsole.WriteLine("Ny bokning skapad och rumsstatus uppdaterad.");

        }

        public void ReadAllBookingsController()
        {
            var bookings = _bookingService.GetAllBookings();
            if (bookings.Count == 0)
            {
                Console.WriteLine("Inga bokningar hittades.");
            }
            else
            {
                foreach (var booking in bookings)
                {
                    if (booking.BookingId != null)
                    {


                        Console.WriteLine("_________________________________________________________________\n");
                        Console.WriteLine($"Bokningsnummer: {booking.BookingId}\n" +
                            //$"Kundens namn: {booking.FirstName}, {booking.LastName}\n " +
                            $"Ankomstdatum: {booking.ArrivalDate}\n" +
                            $"Avresedatum: {booking.DepartureDate}\n" +
                            $"Rumstyp: {booking.RoomType}\n" +
                            $"Antal gäster: {booking.AmountOfGuests}, antal rum: {booking.AmountOfRooms}\n"); //+
                           // $"Kontaktuppgift: {booking.PhoneNumber}");
                        Console.WriteLine("\n_________________________________________________________________\n");
                    }
                    else
                    {
                        Console.WriteLine($"Bokningsnummer {booking.BookingId} har ingen kopplad kund.");
                    }
                }
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
            var id = AnsiConsole.Ask<int>("Ange bokningsnummer för den bokning du vill radera:");
            _bookingService.DeleteBooking(id);
        }

    }
}
