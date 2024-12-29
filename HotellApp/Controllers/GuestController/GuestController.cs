using HotellApp.Controllers.GuestInputValidatingControllers;
using HotellApp.Models;
using HotellApp.Models.Enums;
using HotellApp.Services.GuestServices;
using HotellApp.Utilities.DisplayGuest;
using HotellApp.Utilities.GuestDisplay;
using HotellApp.Utilities.ListDisplay;
using Spectre.Console;

namespace HotellApp.Controllers.GuestController
{
    public class GuestController : IGuestController
    {
        private readonly IGuestService _guestService;
        private readonly IDisplayLists _displayLists;
        private readonly IDisplayGuest _displayGuest;
        private readonly IGuestInputValidatingController _guestInputValidatingController;


        public GuestController(IGuestService guestService,
            IDisplayLists displayLists, IDisplayGuest displayGuest,
            IGuestInputValidatingController guestInputValidatingController
            )
        {
            _guestService = guestService;
            _displayLists = displayLists;
            _displayGuest = displayGuest;
            _guestInputValidatingController = guestInputValidatingController;
        }
        public void CreateGuestController()
        {
            AnsiConsole.MarkupLine("[bold green]Välkommen till kundregistrering![/]");
            AnsiConsole.WriteLine();

            var guest = new Guest
            {
                FirstName = _guestInputValidatingController.ValidateNameNewGuest(
                    "Ange [yellow]förnamn[/]:", 
                    "[red]Namnet måste bestå av minst två tecken![/]"),
                LastName = _guestInputValidatingController.ValidateNameNewGuest(
                    "Ange [yellow]efternamn[/]:", 
                    "[red]Namnent måste bestå av minst två tecken![/]"),
                PhoneNumber = _guestInputValidatingController.ValidatePhoneNumberNewGuest(),
                EmailAdress = _guestInputValidatingController.ValidateEmailAddressNewGuest(),
                GuestStatus = GuestStatus.Active
            };

            var createdGuest = _guestService.CreateGuest(guest);

            _displayGuest.DisplayGuestInformation(createdGuest);

            bool confirm = AnsiConsole.Confirm("\nÄr alla uppgifter korrekta?");

            if (confirm)
            {
                AnsiConsole.MarkupLine("[bold green]Gäst registrerad framgångsrikt![/]");
                Console.ReadKey();
            }
            else
            {
                AnsiConsole.MarkupLine("[bold red]Registrering avbruten.[/]");
                Console.ReadKey();
            }

        }

        public void DeleteGuestController()
        {
            bool isDeletingGuest = true;

            while (isDeletingGuest)
            {
                _displayLists.DisplayGuests();
                var guestId = AnsiConsole.Prompt(
                    new TextPrompt<int>("Ange GästId för den kund du vill radera: "));

                var confirm = AnsiConsole.Confirm("Är du säker på att du vill ta bort gästen?");
                if (!confirm)
                {
                    Console.WriteLine("Radering avbruten.");
                    return;
                }
                else
                {
                    var deleteGuestIfPossible = _guestService.DeleteGuest(guestId);

                    AnsiConsole.MarkupLine($"[red]{deleteGuestIfPossible}[/]");

                    var response = AnsiConsole.Prompt(
                   new SelectionPrompt<string>()
                       .Title("Vill du ta bort en till gäst?")
                       .AddChoices("Ja", "Nej"));
                    Console.Clear();

                    if (response == "Nej")
                    {
                        isDeletingGuest = false;
                    }
                }

            }
        }

        public void ReadAllGuestsController()
        {
            //var guests = _guestService.GetAllGuests();

            //if (guests != null && guests.Any())
            //{
            //        _displayLists.DisplayGuests();
            //}
            //else
            //{
            //    Console.WriteLine("Inga gäster finns registrerade.");
            //}

            _displayLists.DisplayGuests();
        }

        public void ReadGuestController()
        {
            bool isReadingGuest = true;

            while (isReadingGuest)
            {
                _displayLists.DisplayGuests();

                var guestId = AnsiConsole.Prompt(
                new TextPrompt<int>("Ange gästens kundnummer: ")
                 .Validate(id => id > 0 ? ValidationResult.Success() 
                 : ValidationResult.Error("[red]Kundnummer måste vara positivt![/]")));

                Console.Clear();

                var guest = _guestService.ReadGuest(guestId);

                if (guest != null)
                {
                    _displayGuest.DisplayGuestInformation(guest);
                    var continueReading = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                    .Title("Vill du se en till gästs information?")
                    .AddChoices("Ja", "Nej"));

                    // Continue or exit the loop
                    isReadingGuest = continueReading.Trim() == "Ja";
                }
                else
                {
                    AnsiConsole.WriteLine("Gästen kunde inte hittas.\n");

                    var createNewGuest = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                    .Title("Vill du skapa en ny gäst?")
                    .AddChoices("Ja", "Nej"));
                    if (createNewGuest.Trim() == "Ja")
                    {
                        Console.Clear();
                        CreateGuestController();
                    }

                }
                Console.Clear();
            }

        }



        public void UpdateGuestController()
        {
            Console.Clear();
            bool keepAsking = true;
            while (keepAsking)
            { 
                _displayLists.DisplayGuests();
                var guestId = AnsiConsole.Ask<int>("Ange gästId för den gäst du vill uppdatera:");

                var currentGuestData = _guestService.ReadGuest(guestId);
                if (currentGuestData == null)
                {
                    Console.Clear();
                    AnsiConsole.MarkupLine($"[red]Ingen gäst hittades med gästId {guestId}. Försök igen.[/]");
                    
                }
                else
                {
                    var updatedGuest = _guestInputValidatingController.UpdateGuestDetails(currentGuestData);

                   
                    _displayGuest.DisplayGuestInformation(updatedGuest);
                    
                    bool confirm = AnsiConsole.Confirm("\nÄr alla uppgifter korrekta?");

                    if (confirm)
                    {
                        _guestService.UpdateGuest(guestId, updatedGuest);
                        AnsiConsole.MarkupLine($"Gäst med gästId [green]{guestId}[/] har uppdaterats.");
                        Console.ReadKey();
                    }
                    else
                    {
                        AnsiConsole.MarkupLine("[bold red]Registrering avbruten.[/]");
                        Console.ReadKey();
                    }
                    

                    var response = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("Vill du uppdatera en till gäst?")
                        .AddChoices("Ja", "Nej"));
                    
                    Console.Clear();
                    
                    if (response == "Nej")
                    {
                        keepAsking = false;
                    }
                }
            } 
        }


        public (GuestType, int?) SelectGuestType()
        {
            GuestType guestType;
            int? guestId = null;

            while (true)
            {
                guestType = AnsiConsole.Prompt(
                    new SelectionPrompt<GuestType>()
                        .Title("Är det en ny eller befintlig gäst? ")
                        .AddChoices(GuestType.NewGuest, GuestType.ExistingGuest));

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
                CreateGuestController();
                // guestId = GetLatestGuestId(); // Om du har en metod för att hämta det senaste ID:t
            }
            // Handle ExistingGuest selection
            else if (guestType == GuestType.ExistingGuest)
            {
                _displayLists.DisplayGuests();

                while (true) // Loop för att låta användaren försöka igen
                {
                    Console.WriteLine("Ange gästens kundnummer: ");
                    guestId = int.TryParse(Console.ReadLine(), out var id) ? id : (int?)null;

                    if (guestId.HasValue)
                    {
                        var existingGuest = _guestService.ReadGuest(guestId.Value); // Kolla om gästen finns i databasen

                        if (existingGuest == null)
                        {
                            Console.WriteLine("Det angivna kundnumret kunde inte hittas.");
                            if (!AnsiConsole.Confirm("Vill du försöka igen?"))
                            {
                                // Avbryt om användaren inte vill försöka igen
                                return (guestType, null);
                            }
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine($"Gäst funnen: {existingGuest.FirstName} {existingGuest.LastName}");
                            break; // Avsluta loopen om en gäst hittades
                        }
                    }
                    else
                    {
                        Console.WriteLine("Ogiltigt kundnummer. Försök igen.");
                    }
                }
            }

            // Return the selected guest type and the associated guest ID
            return (guestType, guestId);
        }



    }
}
