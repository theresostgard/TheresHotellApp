using HotellApp.Models;
using HotellApp.Models.Enums;
using HotellApp.Services.GuestServices;
using HotellApp.Utilities.DisplayGuest;
using HotellApp.Utilities.ListDisplay;
using Spectre.Console;

namespace HotellApp.Controllers.GuestController
{
    public class GuestController : IGuestController
    {
        private readonly IGuestService _guestService;
        private readonly IDisplayLists _displayLists;


        public GuestController(IGuestService guestService,
            IDisplayLists displayLists
            )
        {
            _guestService = guestService;
            _displayLists = displayLists;

        }
        public void CreateGuestController()
        {


            AnsiConsole.MarkupLine("[bold green]Välkommen till kundregistrering![/]");
            AnsiConsole.WriteLine();

            var guest = new Guest
            {
                FirstName = ValidateName("Ange [yellow]förnamn[/]:", "[red]Namnet får inte vara tomt![/]"),
                LastName = ValidateName("Ange [yellow]efternamn[/]:", "[red]Namnet får inte vara tomt![/]"),
                PhoneNumber = ValidatePhoneNumber(),
                EmailAdress = ValidateEmailAddress(),
                GuestStatus = GuestStatus.Active
            };

            var createdGuest = _guestService.CreateGuest(guest);

            Console.Clear();
            AnsiConsole.MarkupLine("\n[bold green]Sammanfattning av gästinformation:[/]");
            var table = new Table();
            table.AddColumn("[red]Fält[/]");
            table.AddColumn("[red]Värde[/]");
            table.AddRow("Förnamn", guest.FirstName);
            table.AddRow("Efternamn", guest.LastName);
            table.AddRow("E-post", guest.EmailAdress);
            table.AddRow("Telefonnummer", guest.PhoneNumber);
            table.AddRow("Status", guest.GuestStatus.ToString());

            AnsiConsole.Write(new Panel(table)
                .BorderColor(Color.Green)
                .Header("[bold yellow]Registrerad gästinformation[/]")
                .Expand());

            bool confirm = AnsiConsole.Confirm("\nÄr alla uppgifter korrekta?");

            if (confirm)
            {
                AnsiConsole.MarkupLine("[bold green]Gäst registrerad framgångsrikt![/]");
            }
            else
            {
                AnsiConsole.MarkupLine("[bold red]Registrering avbruten.[/]");
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
            var guests = _guestService.GetAllGuests();

            if (guests != null && guests.Any())
            {
                    _displayLists.DisplayGuests();
            }
            else
            {
                Console.WriteLine("Inga gäster finns registrerade.");
            }
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
                    DisplayGuest.DisplayGuestInformation(guest);
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
                    var updatedGuest = GetGuestDetailsFromUser(currentGuestData);

                    _guestService.UpdateGuest(guestId, updatedGuest);
                    //AnsiConsole.MarkupLine($"Gäst med gästId [green]{guestId}[/] har uppdaterats.");

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
                guestId = GetLatestGuestId();
            }
            // Handle ExistingGuest selection
            else if (guestType == GuestType.ExistingGuest)
            {
                _displayLists.DisplayGuests();
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
                        Console.Clear();
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

      

        private string ValidateName(string prompt, string errorMessage, string defaultValue = null)
        {
            return AnsiConsole.Prompt(
                new TextPrompt<string>(prompt)
                    .ValidationErrorMessage(errorMessage)
                    .Validate(input => !string.IsNullOrWhiteSpace(input))
                    .DefaultValue(defaultValue));
        }

        private string ValidatePhoneNumber(string defaultValue = null)
        {
            return AnsiConsole.Prompt(
                new TextPrompt<string>("Ange [yellow]telefonnummer[/]:")
                    .ValidationErrorMessage("[red]Telefonnumret måste vara numeriskt![/]")
                    .Validate(input => long.TryParse(input, out _))
                    .DefaultValue(defaultValue));
        }

        private string ValidateEmailAddress(string defaultValue = null)
        {
            return AnsiConsole.Prompt(
                new TextPrompt<string>("Ange [yellow]e-post[/]:")
                    .ValidationErrorMessage("[red]Ogiltig e-postadress![/]")
                    .Validate(input => input.Contains("@"))
                    .DefaultValue(defaultValue));
        }

        public Guest GetGuestDetailsFromUser(Guest currentGuestData)
        {

            Console.Clear();
            AnsiConsole.MarkupLine("[bold green]Befintlig gästinformation:[/]");
            var existingTable = new Table()
                .AddColumn("[yellow]Fält[/]")
                .AddColumn("[yellow]Registrerad information[/]");
            existingTable.AddRow("Förnamn", currentGuestData.FirstName);
            existingTable.AddRow("Efternamn", currentGuestData.LastName);
            existingTable.AddRow("Telefonnummer", currentGuestData.PhoneNumber);
            existingTable.AddRow("E-post", currentGuestData.EmailAdress);
            existingTable.AddRow("Status", currentGuestData.GuestStatus.ToString());

            AnsiConsole.Write(new Panel(existingTable)
                .BorderColor(Color.Blue)
                .Header("[bold yellow]Nuvarande gästinformation[/]")
                .Expand());

            AnsiConsole.MarkupLine("[gray]Om du vill behålla det befintliga värdet, tryck bara på[/] [green]Enter.[/]");

            var guestFirstName = ValidateName(
                $"Förnamn ([blue]{currentGuestData.FirstName}[/]): ",
                "[red]Namnet får inte vara tomt![/]",
                currentGuestData.FirstName);

            var guestLastName = ValidateName(
                $"Efternamn ([blue]{currentGuestData.LastName}[/]): ",
                "[red]Namnet får inte vara tomt![/]",
                currentGuestData.LastName);

            var guestPhoneNumber = ValidatePhoneNumber(currentGuestData.PhoneNumber);
            var guestEmailAdress = ValidateEmailAddress(currentGuestData.EmailAdress);

            var statusOfGuest = AnsiConsole.Prompt(
                new SelectionPrompt<GuestStatus>()
                    .Title("Välj ny status på gäst:")
                    .HighlightStyle("cyan")
                    .AddChoices(GuestStatus.Active, GuestStatus.Inactive));

            Console.Clear();
            AnsiConsole.MarkupLine("\n[bold green]Sammanfattning av uppdaterad gästinformation:[/]");
            var updatedTable = new Table()
                .AddColumn("[red]Fält[/]")
                .AddColumn("[red]Ny information[/]");
            updatedTable.AddRow("Förnamn", guestFirstName);
            updatedTable.AddRow("Efternamn", guestLastName);
            updatedTable.AddRow("Telefonnummer", guestPhoneNumber);
            updatedTable.AddRow("E-post", guestEmailAdress);
            updatedTable.AddRow("Status", statusOfGuest.ToString());

            
            AnsiConsole.Write(new Panel(updatedTable)
                .BorderColor(Color.Green)
                .Header("[bold yellow]Uppdaterad gästinformation[/]")
                .Expand());

            Console.ReadKey();
            return new Guest
            {
                GuestId = currentGuestData.GuestId,
                FirstName = guestFirstName,
                LastName = guestLastName,
                PhoneNumber = guestPhoneNumber,
                EmailAdress = guestEmailAdress,
                GuestStatus = statusOfGuest
            };
        }
    }
}
