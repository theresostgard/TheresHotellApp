using HotellApp.Models;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotellApp.Utilities.BookingDisplay
{
    public class DisplayBooking : IDisplayBooking
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

        public Table CreateBookingTable(Booking booking, string tableTitle, Color borderColor)
        {
            var table = new Table()
                .BorderColor(borderColor)
                .Border(TableBorder.Double);
            table.AddColumn(new TableColumn("Egenskap").Width(20));
            table.AddColumn(new TableColumn("Information").Width(30));

            table.AddRow("[yellow]Boknings-ID[/]", booking.BookingId.ToString());
            table.AddRow("[yellow]Ankomstdatum[/]", booking.ArrivalDate.ToString("yyyy-MM-dd"));
            table.AddRow("[yellow]Avresedatum[/]", booking.DepartureDate.ToString("yyyy-MM-dd"));
            table.AddRow("[yellow]Rumstyp[/]", booking.RoomType.ToString());
            table.AddRow("[yellow]Antal gäster[/]", booking.AmountOfGuests.ToString());
            table.AddRow("[yellow]Antal rum[/]", booking.AmountOfRooms.ToString());

            return table;
        }

        public static void Pagination(List<Booking> bookings)
        {
            int pageSize = 10; // Antal bokningar per sida
            int currentPage = 0;
            int totalPages = (int)Math.Ceiling(bookings.Count / (double)pageSize);

            while (true)
            {
                Console.Clear();

                // Hämta bokningar för aktuell sida
                var bookingsToDisplay = bookings.Skip(currentPage * pageSize).Take(pageSize).ToList();

                // Rendera bokningar på aktuell sida
                foreach (var booking in bookingsToDisplay)
                {
                    var guestName = booking.Guest != null ? $"{booking.Guest.FirstName} {booking.Guest.LastName}" : "Ingen gäst kopplad";
                    DisplayBooking.DisplayBookingInformation(booking);
                }

                // Visa sidinformation och navigering
                AnsiConsole.MarkupLine($"\nSida [yellow]{currentPage + 1}[/] av [green]{totalPages}[/]");
                AnsiConsole.MarkupLine("[blue]◄[/] Föregående sida   [blue]►[/] Nästa sida");
                AnsiConsole.MarkupLine("[red]Esc[/]: Avsluta");

                var key = Console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.RightArrow:
                        if (currentPage < totalPages - 1) currentPage++;
                        break;
                    case ConsoleKey.LeftArrow:
                        if (currentPage > 0) currentPage--;
                        break;
                    case ConsoleKey.Escape:
                        return; // Avsluta programmet
                }
            }
        }

        public static void RenderTable(List<Booking> bookings, int currentPage, int pageSize)
        {
            var table = new Table();
            table.AddColumn("[Green]Boknings-ID[/]");
            table.AddColumn("[Green]Gästnamn[/]");
            table.AddColumn("[Green]Ankomstdatum[/]");
            table.AddColumn("[Green]Avresedatum[/]");
            table.AddColumn("[Green]Rumstyp[/]");

            int start = currentPage * pageSize;
            int end = Math.Min(start + pageSize, bookings.Count);

            for (int i = start; i < end; i++)
            {
                var booking = bookings[i];
                var guestName = booking.Guest != null ? $"{booking.Guest.FirstName} {booking.Guest.LastName}" : "Ingen gäst kopplad";
                table.AddRow(
                    booking.BookingId.ToString(),
                    guestName,
                    booking.ArrivalDate.ToString("yyyy-MM-dd"),
                    booking.DepartureDate.ToString("yyyy-MM-dd"),
                    booking.RoomType.ToString()
                );
            }

            AnsiConsole.Write(table);
        }
    }
}
