using HotellApp.Models;
using HotellApp.Models.Enums;
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


        public void DisplayBookingInformation(Booking booking)
        {
            var table = CreateBookingTable(booking, "Bokningsinformation", Color.Green);
            AnsiConsole.Write(table);
        }

 
        public void Pagination(List<Booking> bookings)
        {
            var pageSize = 2; // Antal bokningar per sida
            var totalPages = (int)Math.Ceiling(bookings.Count / (double)pageSize);
            var currentPage = 1;

            while (true)
            {
                Console.Clear();
                AnsiConsole.MarkupLine($"[bold yellow]Sida {currentPage}/{totalPages}[/]");

                var bookingsToDisplay = bookings
                    .Skip((currentPage - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                foreach (var booking in bookingsToDisplay)
                {
                    DisplayBookingInformation(booking);
                }

                AnsiConsole.MarkupLine("[green]Använd pil-tangenter för att navigera. Tryck på [/][red]Esc[/] [green]för att avsluta.[/]");
                var key = Console.ReadKey(true).Key;

                if (key == ConsoleKey.RightArrow)
                {
                    if (currentPage < totalPages)
                    {
                        currentPage++;
                    }
                    else
                    {
                        currentPage = 1; // Börja om på första sidan
                    }
                }
                else if (key == ConsoleKey.LeftArrow && currentPage > 1)
                {
                    currentPage--;
                }
                else if (key == ConsoleKey.Escape)
                {
                    break; // Avsluta programmet
                }
            }
        }

        public void DisplayRoomsForBooking(Booking booking)
        {
            if (booking.BookingRooms == null || !booking.BookingRooms.Any())
            {
                AnsiConsole.MarkupLine("[red]Inga rum kopplade till denna bokning.[/]");
                return;
            }

            foreach (var bookingRoom in booking.BookingRooms)
            {
                if (bookingRoom.Room != null)
                {
                    var room = bookingRoom.Room;
                    var roomInfo = new Markup($"[yellow]RumsNr:[/] {room.RoomId}\n" +
                                               $"[yellow]Rumstyp:[/] {room.RoomType}\n" +
                                               $"[yellow]Storlek:[/] {room.RoomSize} kvm");

                    AnsiConsole.Write(
                        new Panel(roomInfo)
                            .BorderColor(Color.Red)
                            .Header($"Bokningsnr {booking.BookingId}")
                            .Border(BoxBorder.Square)
                    );
                }
                else
                {
                    AnsiConsole.MarkupLine("[red]Rum kopplat till bokningen saknas.[/]");
                }
            }
        }
        public Table CreateBookingTable(Booking booking, string header, Color borderColor)
        {
            var table = new Table()
                .BorderColor(borderColor)
                .Border(TableBorder.Double)
                .Title(header);

            table.AddColumn(new TableColumn("Egenskap").Width(20));
            table.AddColumn(new TableColumn("Information").Width(30));

            table.AddRow("[yellow]Boknings-ID[/]", booking.BookingId.ToString());
            table.AddRow("[yellow]Ankomstdatum[/]", booking.ArrivalDate.ToString("yyyy-MM-dd"));
            table.AddRow("[yellow]Gästens namn[/]", booking.Guest.FirstName.ToString() + " " + booking.Guest.LastName.ToString());
            table.AddRow("[yellow]GästId[/]", booking.GuestId.ToString());
            table.AddRow("[yellow]Avresedatum[/]", booking.DepartureDate.ToString("yyyy-MM-dd"));
            table.AddRow("[yellow]Rumstyp[/]", booking.RoomType.ToString());
            table.AddRow("[yellow]Antal gäster[/]", booking.AmountOfGuests.ToString());
            table.AddRow("[yellow]Antal rum[/]", booking.AmountOfRooms.ToString());
            table.AddRow("[yellow]Status på bokningen[/]", booking.Status.ToString());

            return table;
        }

        public static void DisplayTable(Table table)
        {
            AnsiConsole.Write(table);
        }

        public void ShowCurrentBooking(Booking currentBooking)
        {
            var currentBookingTable = CreateBookingTable(currentBooking, "Nuvarande bokningsinformation", Color.Aqua);
            AnsiConsole.Write(currentBookingTable);
        }

        public void ShowUpdatedBookingSummary(Booking currentBooking, DateTime arrivalDate, DateTime departureDate, TypeOfRoom roomType, sbyte amountOfGuests, sbyte amountOfRooms)
        {
            AnsiConsole.MarkupLine("\n[bold green]Sammanfattning av uppdaterad bokning:[/]");

            var updatedTable = new Table()
                .BorderColor(Color.Green)
                .Border(TableBorder.Double);
            updatedTable.AddColumn(new TableColumn("Egenskap").Width(20));
            updatedTable.AddColumn(new TableColumn("Ny information").Width(30));

            updatedTable.AddRow("[red]Boknings-ID[/]", currentBooking.BookingId.ToString());
            updatedTable.AddRow("[red]Ankomstdatum[/]", arrivalDate.ToString("yyyy-MM-dd"));
            updatedTable.AddRow("[red]Avresedatum[/]", departureDate.ToString("yyyy-MM-dd"));
            updatedTable.AddRow("[red]Rumstyp[/]", roomType.ToString());
            updatedTable.AddRow("[red]Antal gäster[/]", amountOfGuests.ToString());
            updatedTable.AddRow("[red]Antal rum[/]", amountOfRooms.ToString());

            AnsiConsole.Write(updatedTable);
        }

    }
}

 
