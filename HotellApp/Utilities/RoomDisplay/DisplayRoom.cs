using HotellApp.Models;
using HotellApp.Utilities.BookingDisplay;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotellApp.Utilities.RoomDisplay
{
    public class DisplayRoom : IDisplayRoom
    {
        public void DisplayRoomInformation(Room room)
        {
            var table = CreateRoomTable(room, "Rumsinformation", Color.Green);
            AnsiConsole.Write(table);
   
        }

        public Table CreateRoomTable(Room room, string header, Color borderColor)
        {
            var table = new Table()
                .BorderColor(borderColor)
                .Border(TableBorder.Double);
            table.AddColumn(new TableColumn("Egenskap").Width(20));
            table.AddColumn(new TableColumn("Information").Width(30));

            table.AddRow("[yellow]RumsNr[/]", room.RoomId.ToString());
            table.AddRow("[yellow]Rumstyp[/]", room.RoomType.ToString());
            table.AddRow("[yellow]Rumsstorlek (kvm)[/]", room.RoomSize.ToString());
            table.AddRow("[yellow]Extrasängar tillåtet?[/]", room.IsExtraBedAllowed.ToString());
            table.AddRow("[yellow]Antal extrasängar (om tillåtet)[/]", room.AmountOfExtraBeds.ToString());
            table.AddRow("[yellow]Status[/]", room.Status.ToString());


            return table;
        }

        public void Pagination(List<Room> rooms)
        {
            int pageSize = 2; // Antal bokningar per sida
            int totalPages = (int)Math.Ceiling(rooms.Count / (double)pageSize);
            int currentPage = 1;

            while (true)
            {
                Console.Clear();
                AnsiConsole.MarkupLine($"[bold yellow]Sida {currentPage}/{totalPages}[/]");

                // Hämta bokningar för aktuell sida
                var roomsToDisplay = rooms
                    .Skip(currentPage * pageSize)
                    .Take(pageSize)
                    .ToList();
                foreach (var room in roomsToDisplay)
                {
                    DisplayRoomInformation(room);
                }
                // Rendera bokningar på aktuell sida
                //foreach (var room in roomsToDisplay)
                //{
                //    var guestName = booking.Guest != null ? $"{booking.Guest.FirstName} {booking.Guest.LastName}" : "Ingen gäst kopplad";
                //   // _displayBooking.DisplayBookingInformation(booking);
                //}

                // Visa sidinformation och navigering
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


        public static void DisplayTable(Table table)
        {
            AnsiConsole.Write(table);
        }
    }
}
