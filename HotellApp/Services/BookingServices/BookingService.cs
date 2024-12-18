using HotellApp.Data;
using HotellApp.Models;
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
            //_dbContext.SaveChanges();

        }


        public Booking ReadBooking(int bookingId)
        {
            var booking = _dbContext.Booking
                         .FirstOrDefault(b => b.BookingId == bookingId);

            return booking;
        }

        public List<Booking> GetAllBookings()
        {
            return _dbContext.Booking.ToList();
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
    }
}
