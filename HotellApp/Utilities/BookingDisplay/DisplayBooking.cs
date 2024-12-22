using HotellApp.Models;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotellApp.Utilities.BookingDisplay
{
    public class DisplayBooking
    {
        public static void DisplayBookingInformation(Booking booking)
        {
            if (booking != null)
            {
                var bookingInfo = new Markup($"[yellow]BokningsNr:[/] [red]{booking.BookingId}[/]\n" +
                                        $"[yellow]GästId:[/] {booking.GuestId}\n" +
                                        $"[yellow]Ankomstdag:[/] {booking.ArrivalDate}\n" +
                                        $"[yellow]Avresedag:[/] {booking.DepartureDate}\n" +
                                        $"[yellow]Rumstyp:[/] {booking.RoomType}\n" +
                                        $"[yellow]Antal gäster:[/] {booking.AmountOfGuests}\n" +
                                        $"[yellow]Antal extrasängar: [/] {booking.AmountOfExtraBeds}\n" +
                                        $"[yellow]Antal rum: [/] {booking.AmountOfRooms}\n");

                // Visa ramat information i en Box
                AnsiConsole.Write(
                    new Panel(bookingInfo)
                        .BorderColor(Color.Green) // Färg på ramen
                        .Header($" Bokningsinformation ") // Lägg till header
                        .Border(BoxBorder.Double) // dubbel ram
                );
            }
            else
            {
                AnsiConsole.MarkupLine("[red]Ingen bokning funnen.[/]");
            }
        }
    }
}
