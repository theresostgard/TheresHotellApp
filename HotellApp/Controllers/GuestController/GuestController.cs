using HotellApp.Models;
using HotellApp.Services.GuestServices;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotellApp.Controllers.GuestController
{
    public class GuestController(IGuestService guestService) : IGuestController
    {
        private readonly IGuestService _guestService = guestService;

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
               $"Kundnr: {guest.GuestId}\n" +
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
            throw new NotImplementedException();
        }

        public void ReadGuestController()
        {
            throw new NotImplementedException();
        }

        public void UpdateGuestController()
        {
            throw new NotImplementedException();
        }
    }
}
