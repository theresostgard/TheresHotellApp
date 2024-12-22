using HotellApp.Models;
using Spectre.Console;

namespace HotellApp.Utilities.BookingDisplay
{
    public interface IDisplayBooking
    {
        Table CreateBookingTable(Booking booking, string tableTitle, Color borderColor);
        static void Pagination(List<Booking> bookings)
        {

        }

        static void RenderTable(List<Booking> bookings, int currentPage, int pageSize)
        {

        }
    }
}