﻿using HotellApp.Models;
using HotellApp.Models.Enums;
using HotellApp.Services.GuestServices;
using Microsoft.EntityFrameworkCore;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotellApp.Controllers.GuestController
{
    public class GuestController : IGuestController
    {
        private readonly IGuestService _guestService;

        public GuestController(IGuestService guestService)
        {
            _guestService = guestService;  
        }
        public void CreateGuestController()
        {


            var guest = new Guest
            {
                FirstName = AnsiConsole.Ask<string>("Förnamn: "),
                LastName = AnsiConsole.Ask<string>("Efternamn"),
                PhoneNumber = ValidatePhoneNumber(),
                EmailAdress = ValidateEmailAddress(),

            };

           var createdGuest = _guestService.CreateGuest(guest);

            AnsiConsole.WriteLine($"Ny gäst skapad med följande information:\n" +
              $"GästId: {createdGuest.GuestId}\n" +
              $"Förnamn: {createdGuest.FirstName}\n" +
              $"Efternamn: {guest.LastName}\n" +
              $"Telefonnummer: {guest.PhoneNumber}\n" +
              $"E-mailadress: {guest.EmailAdress}\n");
        }

        public void DeleteGuestController()
        {
            throw new NotImplementedException();
        }

        public void ReadAllGuestsController()
        {
            Console.WriteLine("Lista med alla registrerade gäster");
        }

        public void ReadGuestController()
        {
            var guestId = AnsiConsole.Prompt(
                   new TextPrompt<int>("Ange gästens kundnummer: "));

            var guest = _guestService.ReadGuest(guestId);

            if (guest != null)
            {
                AnsiConsole.WriteLine($"Gäst funnen:\n" +
                    $"ID: {guest.GuestId}\n" +
                    $"Förnamn: {guest.FirstName}\n" +
                    $"Efternamn: {guest.LastName}\n");
            }
            else
            {
                AnsiConsole.WriteLine("Gästen kunde inte hittas.");

                var IsCreatingNewCustomer = AnsiConsole.Prompt(
                new TextPrompt<bool>("Vill du skapa en ny gäst? (true för ja, false för nej)"));
                if (IsCreatingNewCustomer == true)
                {
                    CreateGuestController();
                }

            }

        }

      

        public void UpdateGuestController()
        {
            Console.WriteLine("Häe ska man kunna uppdatera en gäst");
        }

        public int GetLatestGuestId()
        {

            var latestGuest = _guestService.GetLatestGuestId();
            Console.WriteLine(latestGuest);
            return latestGuest;
        }

        public (GuestType, int?) SelectCustomerType()
        {
            GuestType guestType;
            int? guestId = null;

          
            while (true)
            {
                Console.WriteLine("Är det en ny (1) eller befintlig (2) kund?");
                var guestInput = Console.ReadLine();

                if (Enum.TryParse(guestInput, out guestType) && Enum.IsDefined(typeof(GuestType), guestType))
                {
                    break; 
                }
                else
                {
                    Console.WriteLine("Ogiltigt val. Försök igen.");
                }
            }

            if (guestType == GuestType.NewCustomer)
            {
                Console.WriteLine("Skapa ny kund:");
                CreateGuestController();  // Skapa ny kund
                guestId = GetLatestGuestId(); // Hämta ID för den nyss skapade kunden
            }
            else if (guestType == GuestType.ExistingCustomer)
            {
                // Om användaren väljer befintlig kund, fråga efter kundnummer
                Console.WriteLine("Ange gästens kundnummer: ");
                guestId = int.TryParse(Console.ReadLine(), out var id) ? id : (int?)null;

                if (guestId.HasValue)
                {
                    var existingGuest = _guestService.ReadGuest(guestId.Value); // Kolla om gästen finns i databasen

                    if (existingGuest == null)
                    {
                        Console.WriteLine("Det angivna kundnumret kunde inte hittas.");
                        // Här kan vi lägga till ett alternativ att skapa en ny kund om det behövs
                    }
                    else
                    {
                        Console.WriteLine($"Gäst funnen: {existingGuest.FirstName} {existingGuest.LastName}");
                    }
                }
            }

            return (guestType, guestId); // Returnera vilken typ av kund som valdes och deras ID (eller null om ingen hittades)
        }

        private string ValidatePhoneNumber()
        {
            string phoneNumber;
            while (true)
            {
                phoneNumber = AnsiConsole.Ask<string>("Telefonnummer (endast siffror): ");
                if (phoneNumber.All(char.IsDigit))
                {
                    break;
                }
                AnsiConsole.WriteLine("[red]Fel: Telefonnummer får endast innehålla siffror![/]");
            }
            return phoneNumber;
        }

        // Metod för att validera e-postadress (måste innehålla ett '@')
        private string ValidateEmailAddress()
        {
            string email;
            while (true)
            {
                email = AnsiConsole.Ask<string>("Emailadress: ");
                if (email.Contains("@") && email.Contains("."))
                {
                    break;
                }
                AnsiConsole.WriteLine("[red]Fel: Vänligen ange en giltig e-postadress som innehåller '@' och '.'[/]");
            }
            return email;
        }
    }
}
