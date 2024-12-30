using HotellApp.Models.Enums;
using HotellApp.Models;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotellApp.Controllers.GuestController;
using HotellApp.Utilities.ListDisplay;
using HotellApp.Services.GuestServices;
using HotellApp.Utilities.GuestDisplay;

namespace HotellApp.Controllers.GuestInputValidatingControllers
{
    public class GuestInputValidatingController : IGuestInputValidatingController
    {

        private readonly IDisplayGuest _displayGuest;
      

        public GuestInputValidatingController(
            IDisplayGuest displayGuest)
        {
            _displayGuest = displayGuest;
        }
    
        public string ValidateNameNewGuest(string prompt, string errorMessage)
        {
            return AnsiConsole.Prompt(
                new TextPrompt<string>(prompt)
                    .ValidationErrorMessage(errorMessage)
                    .Validate(input => !string.IsNullOrWhiteSpace(input) && input.Length >= 2));
        }

        public string ValidatePhoneNumberNewGuest()
        {
            return AnsiConsole.Prompt(
                new TextPrompt<string>("Ange [yellow]telefonnummer[/]:")
                    .ValidationErrorMessage("[red]Telefonnumret måste vara numeriskt![/]")
                    .Validate(input => long.TryParse(input, out _) && input.Length >= 10));
        }

        public string ValidateEmailAddressNewGuest()
        {
            return AnsiConsole.Prompt(
                new TextPrompt<string>("Ange [yellow]e-post[/]:")
                    .ValidationErrorMessage("[red]Ogiltig e-postadress!\nEn e-mailadress måste innehålla ett @ och en punkt![/]")
                    .Validate(input =>
                    System.Text.RegularExpressions.Regex.IsMatch(input, @"^[^@\s]+@[^@\s]+\.[^@\s]+$")));

        }

        public Guest UpdateGuestDetails(Guest currentGuestData)
        {
            Console.Clear();
            AnsiConsole.MarkupLine("[bold green]Befintlig gästinformation:[/]");

            _displayGuest.DisplayGuestInformation(currentGuestData);

            AnsiConsole.MarkupLine("[gray]Om du vill behålla det befintliga värdet, tryck bara på[/] [green]Enter.[/]");

            var guestFirstName = ValidateNameExistingGuest(
                $"Förnamn: ",
                "[red]Namnet måste innehålla minst 2 tecken![/]",
                currentGuestData.FirstName);

            var guestLastName = ValidateNameExistingGuest(
                $"Efternamn: ",
                "[red]Namnet måste innehålla minst 2 tecken![/]",
                currentGuestData.LastName);

            var guestPhoneNumber = ValidatePhoneNumberExistingGuest(currentGuestData.PhoneNumber);

            var guestEmailAdress = ValidateEmailAddressExistingGuest(currentGuestData.EmailAdress);


            var statusOfGuest = AnsiConsole.Prompt(
                new SelectionPrompt<GuestStatus>()
                    .Title("Välj ny status på gäst:")
                    .HighlightStyle("cyan")
                    .AddChoices(GuestStatus.Active, GuestStatus.Inactive));

            var updatedGuest = new Guest
            {
                GuestId = currentGuestData.GuestId,
                FirstName = guestFirstName,
                LastName = guestLastName,
                PhoneNumber = guestPhoneNumber,
                EmailAdress = guestEmailAdress,
                GuestStatus = statusOfGuest
            };

            Console.Clear();
            AnsiConsole.MarkupLine("\n[bold green]Sammanfattning av uppdaterad gästinformation:[/]");
            _displayGuest.DisplayGuestInformation(updatedGuest); 

            Console.ReadKey();

            return updatedGuest; 
        }

        public string ValidateEmailAddressExistingGuest(string currentEmail)
        {
            return AnsiConsole.Prompt(
                new TextPrompt<string>("E-post:")
                    .AllowEmpty() 
                    .DefaultValue(currentEmail) 
                    .ValidationErrorMessage("[red]Ogiltig e-postadress!\nEn e-mailadress måste innehålla ett @ och en punkt![/]")
                    .Validate(input =>
                        string.IsNullOrEmpty(input) || 
                        System.Text.RegularExpressions.Regex.IsMatch(input, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))); 
        }

        public string ValidatePhoneNumberExistingGuest(string currentPhoneNumber)
        {
            return AnsiConsole.Prompt(
                new TextPrompt<string>("Telefonnummer:")
                    .AllowEmpty() 
                    .DefaultValue(currentPhoneNumber) 
                    .ValidationErrorMessage("[red]Telefonnumret måste vara numeriskt och innehålla minst 10 siffror![/]")
                    .Validate(input =>
                        string.IsNullOrEmpty(input) || 
                        (long.TryParse(input, out _) && input.Length >= 10))); 
        }

        public string ValidateNameExistingGuest(string prompt, string errorMessage, string currentName)
        {
            return AnsiConsole.Prompt(
                new TextPrompt<string>(prompt)
                    .AllowEmpty() 
                    .DefaultValue(currentName) 
                    .ValidationErrorMessage(errorMessage)
                    .Validate(input =>
                        string.IsNullOrEmpty(input) || 
                        input.Trim().Length >= 2)); 
        }
    }
}
