using HotellApp.Data;
using HotellApp.Models;
using HotellApp.Services.GuestServices;
using Microsoft.EntityFrameworkCore;
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
            _dbContext = dbContext; 
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
            
          
        }

        public List<Guest> GetAllGuests()
        {
            return _dbContext.Guest.ToList();   
        }

        public void UpdateGuest(int guestId, Guest updatedGuest)
        {

            var guest = _dbContext.Guest.FirstOrDefault(c => c.GuestId == guestId);

            if (guest != null)
            {
               
                guest.FirstName = updatedGuest.FirstName;
                guest.LastName = updatedGuest.LastName;
                guest.PhoneNumber = updatedGuest.PhoneNumber;
                guest.EmailAdress = updatedGuest.EmailAdress;
                guest.GuestStatus = updatedGuest.GuestStatus;

                _dbContext.Entry(guest).State = EntityState.Modified;
                _dbContext.SaveChanges();
                
            }
            else
            {
                AnsiConsole.WriteLine("Kunden kunde inte hittas.");
            }

            
        }
        public string DeleteGuest(int guestId)
        {

            var guest = _dbContext.Guest.FirstOrDefault(c => c.GuestId == guestId);

            if (guest != null)
            {
                // Check if the guest has any bookings. Assuming Bookings is a collection (like a list or a navigation property).
                if (guest.Bookings == null || guest.Bookings.Count == 0)
                {
                    _dbContext.Guest.Remove(guest);  // Remove the guest from the context
                    _dbContext.SaveChanges();  // Persist the changes to the database
                    return "Gästen har raderats.";
                }
                else
                {
                    return "Gästen har bokningar och kan inte tas bort";

                }
            }
            else
            {
                return "Ingen gäst med det GästId:t kunde hittas.";
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
