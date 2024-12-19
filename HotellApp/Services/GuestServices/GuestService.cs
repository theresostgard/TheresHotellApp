using HotellApp.Data;
using HotellApp.Models;
using HotellApp.Services.GuestServices;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotellApp.Services.GuestServices
{
    public class GuestService : IGuestService
    {
        private readonly ApplicationDbContext _dbContext;

        public GuestService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext; // ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public Guest CreateGuest(Guest guest)
        {
            
                _dbContext.Guest.Add(guest);
                _dbContext.SaveChanges();
                return guest;
            
        }

        public Guest ReadGuest(int guestId)
        {
            
                var guestbyId = _dbContext.Guest
                    .FirstOrDefault(g => g.GuestId == guestId);

                return guestbyId;
            
            //bool IsContinuingReading = true;

            //while (IsContinuingReading)
            //{
            //    Console.Clear();
            //    var allCustomers = _dbContext.Guest;

            //    AnsiConsole.WriteLine("Alla kunder:");
            //    foreach (var items in _dbContext.Guest)
            //    {
            //        AnsiConsole.WriteLine($"ID: {items.GuestId}\nFörnamn: {items.FirstName}\nEfternamn: {items.LastName}\n");

            //    }
            //    Console.ReadKey();
            //    var customerId = AnsiConsole.Prompt(
            //        new TextPrompt<int>("Ange kundens ID: "));

            //    var customer = _dbContext.Guest.FirstOrDefault(c => c.GuestId == customerId);

            //    if (customer != null)
            //    {
            //        AnsiConsole.WriteLine($"Kund funnen:\n" +
            //            $"ID: {customer.GuestId}\n" +
            //            $"Förnamn: {customer.FirstName}\n" +
            //            $"Efternamn: {customer.LastName}\n" +
            //            $"Telefonnummer: {customer.PhoneNumber}\n" +
            //            $"E-mailadress: {customer.EmailAdress}\n");
            //    }
            //    else
            //    {
            //        AnsiConsole.WriteLine("Kunden kunde inte hittas.");

            //        var IsCreatingNewCustomer = AnsiConsole.Prompt(
            //        new TextPrompt<bool>("Vill du skapa en ny kund? (true för ja, false för nej)"));
            //        if (IsCreatingNewCustomer == true)
            //        {
            //            //CreateGuest();
            //        }

            //    }
            //    var continueOption = AnsiConsole.Prompt(
            //    new TextPrompt<bool>("Vill du läsa en annan kund? (true för ja, false för nej)"));

            //    if (!continueOption)
            //    {
            //        IsContinuingReading = false;  // Stäng av loopen om användaren inte vill fortsätta
            //    }
            //}
            //return _dbContext.Guest;
        }

        public void UpdateGuest(int guestId, Guest guest)
        {
            var customerId = AnsiConsole.Prompt(
           new TextPrompt<int>("Ange kundens ID att uppdatera: "));

            var customer = _dbContext.Guest.FirstOrDefault(c => c.GuestId == customerId);

            if (customer != null)
            {
                var newFirstName = AnsiConsole.Prompt(
                    new TextPrompt<string>($"Ange nytt förnamn (nuvarande: {customer.FirstName}): "));
                var newLastName = AnsiConsole.Prompt(
                    new TextPrompt<string>($"Ange nytt efternamn (nuvarande: {customer.LastName}): "));
                var newPhoneNumber = AnsiConsole.Prompt(
                    new TextPrompt<string>($"Ange nytt telefonnummer (nuvarande: {customer.PhoneNumber}): "));
                var newEmail = AnsiConsole.Prompt(
                    new TextPrompt<string>($"Ange ny E-mailadress (nuvarande: {customer.EmailAdress}): "));

                customer.FirstName = newFirstName;
                customer.LastName = newLastName;
                customer.PhoneNumber = newPhoneNumber;
                customer.EmailAdress = newEmail;

                AnsiConsole.WriteLine("Kundens information har uppdaterats.");
            }
            else
            {
                AnsiConsole.WriteLine("Kunden kunde inte hittas.");
            }

            _dbContext.SaveChanges();
        }
        public void DeleteGuest(int guestId)
        {
            var customerId = AnsiConsole.Prompt(
            new TextPrompt<int>("Ange gästId för den gäst du vill ta bort: "));

            var customer = _dbContext.Guest.FirstOrDefault(c => c.GuestId == customerId);

            if (customer != null)
            {
                _dbContext.Guest.Remove(customer); // Ta bort kunden från listan
                AnsiConsole.WriteLine("Gästen har tagits bort.");
            }
            else
            {
                AnsiConsole.WriteLine("Ingen gäst med det Id:t kunde inte hittas.");
            }
        }

        public int GetLatestGuestId()
        {
           
                var latestGuest = _dbContext.Guest
                    .OrderByDescending(g => g.GuestId)
                    .FirstOrDefault();

                if (latestGuest == null)
                {
                    Console.WriteLine("Inga gäster finns i databasen.");
                    return 0;
                }

                return latestGuest.GuestId;
            

        }
    }
}
