using HotellApp.Models;
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
            _guestService = guestService ?? throw new ArgumentNullException(nameof(guestService)); 
        }
        public void CreateGuestController()
        {
            Console.WriteLine("Ange den nya gästens uppgifter: ");

            var guest = new Guest
            {
                FirstName = AnsiConsole.Ask<string>("Förnamn: "),
                LastName = AnsiConsole.Ask<string>("Efternamn"),
                PhoneNumber = AnsiConsole.Ask<string>("Telefonnummer: "),
                EmailAdress = AnsiConsole.Ask<string>("Emailadress: "),

            };

            AnsiConsole.WriteLine($"Ny kund skapad med följande information:\n" +
               $"Förnamn: {guest.FirstName}\n" +
               $"Efternamn: {guest.LastName}\n" +
               $"Telefonnummer: {guest.PhoneNumber}\n" +
               $"E-mailadress: {guest.EmailAdress}\n");

            _guestService.CreateGuest(guest);
            
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
            Console.WriteLine("Hämtar id för den senast skapade kunden: ");

            var latestGuest = _guestService.GetLatestGuestId();
            return latestGuest;
        }

        public (GuestType, int?) SelectCustomerType()
        {
            GuestType guestType;
            int? guestId = null;

            // Fråga användaren om de vill skapa en ny eller välja en befintlig kund
            while (true)
            {
                Console.WriteLine("Är det en ny (1) eller befintlig (2) kund?");
                var guestInput = Console.ReadLine();

                if (Enum.TryParse(guestInput, out guestType) && Enum.IsDefined(typeof(GuestType), guestType))
                {
                    break; // Om vi får ett giltigt svar, gå vidare
                }
                else
                {
                    Console.WriteLine("Ogiltigt val. Försök igen.");
                }
            }

            // Om användaren väljer ny kund
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
    }
}
