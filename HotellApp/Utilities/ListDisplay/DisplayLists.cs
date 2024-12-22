using HotellApp.Data;
using HotellApp.Models;
using HotellApp.Models.Enums;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotellApp.Utilities.ListDisplay
{
    public class DisplayLists : IDisplayLists
    {
        private readonly ApplicationDbContext _dbContext;

        public DisplayLists(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void DisplayGuests()
        {
            AnsiConsole.MarkupLine("[blue]Lista med alla registrerade gäster:[/]\n");
            var readAllGuests = _dbContext.Guest
                .ToList();

            var table = new Table();
            table.AddColumn("GästId");
            table.AddColumn("Förnamn");
            table.AddColumn("Efternamn");
            foreach (var guest in readAllGuests)
            {

                string guestIdWithColor = $"[green]{guest.GuestId}[/]";
                string guestFirstNameWithColor = $"[white]{guest.FirstName}[/]";
                string guestLastNameWithColor = $"[blue]{guest.LastName}[/]";

                table.AddRow(guestIdWithColor, guestFirstNameWithColor, guestLastNameWithColor);
            }
            AnsiConsole.Render(table);

        }

        public void DisplayRooms()
        {
            var readAllRooms = _dbContext.Room
                .ToList();

            var table = new Table()
            {
                Border = TableBorder.Double,
            };
            
            table.AddColumn("RumsId");
            table.AddColumn("Rumstyp");
            table.AddColumn("Status");

            foreach (var room in readAllRooms)
            {

                string roomIdWithColor = $"[green]{room.RoomId}[/]";  // Grön färg för RumsId
                string roomTypeWithColor = room.RoomType == TypeOfRoom.Single ? "[blue]Single[/]" : "[yellow]Double[/]";
                string roomStatusWithColor = room.Status == StatusOfRoom.Active ? "[green]Active[/]" :
                                             room.Status == StatusOfRoom.InActive ? "[red]InActive[/]" :
                                             "[white]UnderMaintenance[/]";

                table.AddRow(roomIdWithColor, roomTypeWithColor, roomStatusWithColor);

               
            }
            AnsiConsole.Write(
                  new Panel(table)
                      .BorderColor(Color.Green) // Färg på ramen
                      .Header($" Rumsinformation ") // Lägg till header
                      .Border(BoxBorder.Double)
                      );// dubbel ram
            //AnsiConsole.Render(table);
        }

        public void DisplayBookings()
        {
            var readAllBookings = _dbContext.Booking
                .ToList();

            var table = new Table();
            table.AddColumn("BokningsNr");
            table.AddColumn("Ankomstdag");
            table.AddColumn("Avresedag");

            foreach (var booking in readAllBookings)
            {
                string bookingIdWithColor = $"[green]{booking.BookingId}[/]";
                string arrivalDateWithColor = $"[white]{booking.ArrivalDate}[/]";
                string departureWithColor = $"[blue]{booking.DepartureDate}[/]";

                table.AddRow(bookingIdWithColor, arrivalDateWithColor, departureWithColor);
            }

            AnsiConsole.Render(table);

        }

        public void DisplayInvoices()
        {

        }
    }
}
