﻿using HotellApp.Models;
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
            table.AddRow("[yellow]Pris/natt[/]", room.PricePerNight.ToString());


            return table;
        }

        public void Pagination(List<Room> rooms)
        {
            int pageSize = 2; 
            int totalPages = (int)Math.Ceiling(rooms.Count / (double)pageSize);
            int currentPage = 1;

            while (true)
            {
                Console.Clear();
                AnsiConsole.MarkupLine($"[bold yellow]Sida {currentPage}/{totalPages}[/]");

                var roomsToDisplay = rooms
                    .Skip(currentPage * pageSize)
                    .Take(pageSize)
                    .ToList();
                foreach (var room in roomsToDisplay)
                {
                    DisplayRoomInformation(room);
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
                        currentPage = 1; 
                    }
                }
                else if (key == ConsoleKey.LeftArrow && currentPage > 1)
                {
                    currentPage--;
                }
                else if (key == ConsoleKey.Escape)
                {
                    break; 
                }
            }
        }


        public static void DisplayTable(Table table)
        {
            AnsiConsole.Write(table);
        }
    }
}
