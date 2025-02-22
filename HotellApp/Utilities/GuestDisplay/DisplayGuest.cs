﻿using HotellApp.Models;
using HotellApp.Utilities.GuestDisplay;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotellApp.Utilities.DisplayGuest
{
    public class DisplayGuest : IDisplayGuest
    {
        public void DisplayGuestInformation(Guest guest)
        {
            Console.Clear();
            AnsiConsole.MarkupLine("\n[bold green]Sammanfattning av gästinformation:[/]");
            if (guest != null)
            {
                var guestInfo = new Markup($"[yellow]GästId:[/] [red]{guest.GuestId}[/]\n" +
                                        $"[yellow]Förnamn:[/] {guest.FirstName}\n" +
                                        $"[yellow]Efternamn:[/] {guest.LastName}\n" +
                                        $"[yellow]Telefonnummer:[/] {guest.PhoneNumber}\n" +
                                        $"[yellow]Mailadress:[/] {guest.EmailAdress}\n" +
                                        $"[yellow]Status:[/] {guest.GuestStatus}");

                AnsiConsole.Write(
                    new Panel(guestInfo)
                        .BorderColor(Color.Green) 
                        .Header($" Gästinformation ") 
                        .Border(BoxBorder.Double) 
                );
            }
            else
            {
                AnsiConsole.MarkupLine("[red]Ingen gäst funnen.[/]");
            }
        }
    }
}
