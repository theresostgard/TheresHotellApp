using HotellApp.Models.Enums;
using HotellApp.Models;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotellApp.Services.BookingServices;

namespace HotellApp.Controllers.BookingController
{
    public class BookingController : IBookingController
    {
        private readonly IBookingService _bookingService;

        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        public void CreateBookingController()
        {
            Console.WriteLine("Ange bokningsdetaljer:");

            var booking = new Booking
            {
                //här behöver kundens namn hämtas på något sätt
                ArrivalDate = AnsiConsole.Ask<DateTime>("Ankomstdatum (yyyy-MM-dd):"),
                DepartureDate = AnsiConsole.Ask<DateTime>("Avresedatum (yyyy-MM-dd):"),
                RoomType = AnsiConsole.Ask<TypeOfRoom>("Rumstyp: (enkel(1), dubbel(2)"),
                AmountOfGuests = AnsiConsole.Ask<sbyte>("Antal gäster: "),
                AmountOfRooms = AnsiConsole.Ask<sbyte>("Antal rum:")

            };

            _bookingService.CreateBooking(booking);
            AnsiConsole.WriteLine("Ny bokning skapad.");
        }

        public void ReadAllBookingsController()
        {
            var bookings = _bookingService.GetAllBookings();
            //if (bookings.Count == 0)
            //{
            //    Console.WriteLine("Inga bokningar hittades.");
            //}
            //else
            //{
            //    foreach (var booking in bookings)
            //    {
            //        if (booking.Customer != null)
            //        {


            //            Console.WriteLine("_________________________________________________________________\n");
            //            Console.WriteLine($"Bokningsnummer: {booking.BookingId}\n" +
            //                $"Kundens namn: {booking.FirstName}, {booking.LastName}\n " +
            //                $"Ankomstdatum: {booking.ArrivalDate}\n" +
            //                $"Avresedatum: {booking.DepartureDate}\n" +
            //                $"Rumstyp: {booking.RoomType}\n" +
            //                $"Antal gäster: {booking.AmountOfGuests}, antal rum: {booking.AmountOfRooms}\n" +
            //                $"Kontaktuppgift: {booking.PhoneNumber}");
            //            Console.WriteLine("\n_________________________________________________________________\n");
            //        }
            //        else
            //        {
            //            Console.WriteLine($"Bokningsnummer {booking.BookingId} har ingen kopplad kund.");
            //        }
            //    }
            //}
        }

        public void ReadBookingController()
        {
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
