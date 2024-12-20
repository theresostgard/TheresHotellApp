using HotellApp.Data;
using HotellApp.Models;
using HotellApp.Models.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotellApp.Services.BookingServices
{
    public class BookingService : IBookingService
    {
        private readonly ApplicationDbContext _dbContext;

        public BookingService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public void CreateBooking(Booking booking)
        {
            
            _dbContext.Booking.Add(booking);
            _dbContext.SaveChanges();
        }

        public void AddRoomsToBooking(List<BookingRoom> bookingRooms)
        {
            _dbContext.BookingRoom.AddRange(bookingRooms); // Lägger till alla BookingRoom-poster
            _dbContext.SaveChanges(); // Spara ändringarna i databasen
        }


        public Booking ReadBooking(int bookingId)
        {
            var booking = _dbContext.Booking
            .Include(b => b.BookingRooms)
            .ThenInclude(br => br.Room)  // Inkludera rummen som är kopplade till bokningen
            .Include(g => g.Guest)
            .FirstOrDefault(b => b.BookingId == bookingId);

            return booking;
        }

        public List<Booking> GetAllBookings()
        {
            return _dbContext.Booking
                .Include(g => g.Guest)
                .Include(b => b.BookingRooms)
                .ThenInclude(r => r.Room)
                .ToList();
        }

        // Uppdaterar en bokning
        public void UpdateBooking(int bookingId, Booking updatedBooking)
        {
            var booking = _dbContext.Booking.FirstOrDefault(b => b.BookingId == bookingId);
            if (booking != null)
            {
                booking.ArrivalDate = updatedBooking.ArrivalDate;
                booking.DepartureDate = updatedBooking.DepartureDate;
                booking.RoomType = updatedBooking.RoomType;
                booking.AmountOfGuests = updatedBooking.AmountOfGuests;
                booking.AmountOfRooms = updatedBooking.AmountOfRooms;

                //_dbContext.SaveChanges();
                Console.WriteLine($"Bokningen med bokningsnr {bookingId} har uppdaterats!");
            }
            else
            {
                Console.WriteLine("Ingen bokning hittades.");
            }
        }

        // Tar bort en bokning
        public void DeleteBooking(int id)
        {
            var booking = _dbContext.Booking.FirstOrDefault(b => b.BookingId == id);
            if (booking != null)
            {

                _dbContext.Booking.Remove(booking);            //set as inactive?
                Console.WriteLine("Booking deleted successfully!");
            }
            else
            {
                Console.WriteLine("Booking not found.");
            }
        }
        public bool TryGetAvailableRoomsForBooking(TypeOfRoom roomType, DateTime arrivalDate, DateTime departureDate, int amountOfRooms, out List<Room> availableRooms)
        {
            // Hämta tillgängliga rum baserat på rumstyp och datumintervall
            availableRooms = _dbContext.Room
                .Where(r => r.RoomType == roomType && r.Status == StatusOfRoom.Active)
                .ToList();

            // Om användaren valde extrasängar för dubbelrum
            if (roomType == TypeOfRoom.Double)
            {
                // Kontrollera om det finns rum med tillräckligt utrymme för extrasängar
                availableRooms = availableRooms.Where(r => r.RoomSize >= 15).ToList();
            }

            // Om det inte finns tillräckligt med rum returnera false
            return availableRooms.Count >= amountOfRooms;
        }
    }
}
