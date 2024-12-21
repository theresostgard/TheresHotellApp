using HotellApp.Data;
using HotellApp.Models.Enums;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotellApp.Utilities
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

            foreach (var guest in readAllGuests)
            {
                AnsiConsole.MarkupLine($"GästId: [green]{guest.GuestId}[/]\n" +
                    $"Förnamn: {guest.FirstName}, " +
                    $"Efternamn: {guest.LastName}\n" +
                    $"___________________________________________");
            }
            
        }

        public void DisplayRooms()
        {
            var readAllRooms = _dbContext.Room
                .ToList();

            var table = new Table();
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
            AnsiConsole.Render(table);
        }

        public void DisplayBookings()
        {
            var readAllBookings = _dbContext.Booking
                .ToList();

          //var table = new Table();
          //  table.AddColumn("BokningsNr");
          //  table.AddColumn("Ankomstdag");
          //  table.AddColumn("Avresedag");

        }

        public void DisplayInvoices()
        {

        }
    }
}
