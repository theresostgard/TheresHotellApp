using HotellApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotellApp.Services.BookingServices
{
    public interface IBookingService
    {
        void CreateBooking(Booking booking);
        List<Booking> GetAllBookings();

        Booking ReadBooking(int id);
        void UpdateBooking(int id, Booking updatedBooking);
        void DeleteBooking(int id);
    }
}
