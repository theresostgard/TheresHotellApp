using HotellApp.Models;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotellApp.Utilities.RoomDisplay
{
    public class DisplayRoom
    {
        public static void DisplayRoomInformation(Room room)
        {
            if (room != null)
            {
                var roomInfo = new Markup($"[yellow]RumsId:[/] [red]{room.RoomId}[/]\n" +
                                        $"[yellow]Rumstyp:[/] {room.RoomType}\n" +
                                        $"[yellow]Storlek:[/] {room.RoomSize}\n" +
                                        $"[yellow]Är extrasäng tillåtet:[/] {room.IsExtraBedAllowed}\n" +
                                        $"[yellow]Antal extrasängar:[/] {room.AmountOfExtraBeds}\n" +
                                        $"[yellow]Status på rummet:[/] {room.Status}");

                // Visa ramat information i en Box
                AnsiConsole.Write(
                    new Panel(roomInfo)
                        .BorderColor(Color.Green) // Färg på ramen
                        .Header($" Rumsinformation ") // Lägg till header
                        .Border(BoxBorder.Double) // dubbel ram
                );
            }
            else
            {
                AnsiConsole.MarkupLine("[red]Inget rum med det rumsId:t hittades.[/]");
            }
        }
    }
}
