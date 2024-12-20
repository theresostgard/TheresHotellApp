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
            _guestService = guestService;  
        }
        public void CreateGuestController()
        {
            AnsiConsole.WriteLine("Registrera ny gäst:\n\n");

            var guest = new Guest
            {
                FirstName = AnsiConsole.Ask<string>("Förnamn: "),
                LastName = AnsiConsole.Ask<string>("Efternamn"),
                PhoneNumber = ValidatePhoneNumber(),
                EmailAdress = ValidateEmailAddress(),
                GuestStatus = GuestStatus.Active

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
            var guestId = AnsiConsole.Prompt(
                new TextPrompt<int>("Ange GästId för den kund du vill radera: "));

            var confirm = AnsiConsole.Confirm("Är du säker på att du vill radera gästen?");
            if (!confirm)
            {
                Console.WriteLine("Radering avbruten.");
                return;
            }

            var deleteGuestIfPossible = _guestService.DeleteGuest(guestId);

            AnsiConsole.MarkupLine($"[red]{deleteGuestIfPossible}[/]");
        }

        public void ReadAllGuestsController()
        {
            var guests = _guestService.GetAllGuests();

            if (guests != null && guests.Any())
            {
                foreach (var guest in guests)
                {
                    Console.WriteLine($"GästId: {guest.GuestId}\n" +
                        $"Förnamn: {guest.FirstName}\n" +
                        $"Efternamn: {guest.LastName}\n" +
                        $"_______________________________\n");
                }
            }
            else
            {
                Console.WriteLine("Inga gäster finns registrerade.");
            }
        }

        public void ReadGuestController()
        {
            var guestId = AnsiConsole.Prompt(
                   new TextPrompt<int>("Ange gästens kundnummer: "));

            var guest = _guestService.ReadGuest(guestId);

            if (guest != null)
            {
                AnsiConsole.WriteLine($"Gäst funnen:\n" +
                    $"GästId: {guest.GuestId}\n" +
                    $"Förnamn: {guest.FirstName}\n" +
                    $"Efternamn: {guest.LastName}\n");
            }
            else
            {
                AnsiConsole.WriteLine("Gästen kunde inte hittas.\n");

                var IsCreatingNewCustomer = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("Vill du skapa en ny gäst?")
                .AddChoices("Ja", "Nej"));
                if (IsCreatingNewCustomer.Trim() == "Ja")
                {
                    Console.Clear();
                    CreateGuestController();
                }

            }

        }

      

        public void UpdateGuestController()
        {
            var guestId = AnsiConsole.Ask<int>("Ange gästId för den gäst du vill uppdatera:");

            var currentGuestData = _guestService.ReadGuest(guestId);

            var updatedGuest = GetGuestDetailsFromUser(currentGuestData);

            _guestService.UpdateGuest(guestId, updatedGuest);
            AnsiConsole.WriteLine($"Gäst med gästId {guestId} har uppdaterats.");
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
                guestType = AnsiConsole.Prompt(
                    new SelectionPrompt<GuestType>()
                    .Title("Är det en ny eller befintlig gäst? ")
                    .AddChoices(GuestType.NewGuest, GuestType.ExistingGuest));

                //if (Enum.TryParse(guestInput, out guestType) && Enum.IsDefined(typeof(GuestType), guestType))
                //{
                //    break; 
                //}
                //else
                //{
                //    Console.WriteLine("Ogiltigt val. Försök igen.");
                //}

                if (guestType == GuestType.NewGuest || guestType == GuestType.ExistingGuest)
                {
                    break;
                }
                else
                {
                    AnsiConsole.Markup("[red]Ogiltigt val. Försök igen.[/]");
                }
            }

            // Handle NewGuest selection
            if (guestType == GuestType.NewGuest)
            {
                Console.WriteLine("Skapa ny kund:");
                CreateGuestController();  // Skapa ny kund
                guestId = GetLatestGuestId(); // Hämta ID för den nyss skapade kunden
            }
            // Handle ExistingGuest selection
            else if (guestType == GuestType.ExistingGuest)
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
                else
                {
                    Console.WriteLine("Ogiltigt kundnummer. Försök igen.");
                }
            }

            // Return the selected guest type and the associated guest ID
            return (guestType, guestId);
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
                AnsiConsole.Markup("[red]Felaktig input: Telefonnummer får endast innehålla siffror!\n[/]");
            }
            return phoneNumber;
        }

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
                AnsiConsole.Markup("[red]Felaktig input: Vänligen ange en giltig e-postadress som innehåller '@' och '.'\n[/]");
            }
            return email;
        }

        public Guest GetGuestDetailsFromUser(Guest currentGuestData)
        {
            int guestId;
            while (true)
            {
                guestId = AnsiConsole.Ask<int>("Ange gästens kundnummer: ");
                var existingGuest = _guestService.ReadGuest(guestId);  // Assuming _guestService.ReadGuest fetches guest by ID

                if (existingGuest != null)
                {
                    break; // If guest is found, break out of the loop
                }
                else
                {
                    Console.Clear();
                    AnsiConsole.Markup("[red]Ogiltigt kundnummer, försök igen.[/]\n");
                    
                }
            }

            //highlighta gamla namnet?
            var guestFirstName = AnsiConsole.Ask<string>(
                $"Registrerat förnamn på gäst: " +
                $"{currentGuestData.FirstName}. " +
                $"Ange nytt förnamn: ");

            var guestLastName = AnsiConsole.Ask<string>
                ($"Registrerat efternamn på gäst: " +
                $"{currentGuestData.LastName}. " +
                $"Ange nytt efternamn: ");

            //skulle vilja få in att man ser det förra numret också
            var guestPhoneNumber = ValidatePhoneNumber();

            //gamla emailaddressen syns inte nu
            var guestEmailAdress = ValidateEmailAddress();
            var statusOfGuest = AnsiConsole.Prompt(
                new SelectionPrompt<GuestStatus>()
                .Title("Ändra status på gäst: Välj ny status på gäst:")
                .AddChoices(GuestStatus.Inactive, GuestStatus.Active));

            return new Guest
            {
                FirstName = guestFirstName,
                LastName = guestLastName,
                PhoneNumber = guestPhoneNumber,
                EmailAdress = guestEmailAdress,
                GuestStatus = statusOfGuest
            };
        }
    }
}
