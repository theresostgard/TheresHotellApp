using HotellApp.Models;
using HotellApp.Models.Enums;
using Spectre.Console;

namespace HotellApp.Utilities.BookingDisplay
{
    public interface IDisplayBooking
    {
        //Table CreateBookingTable(Booking booking, string tableTitle, Color borderColor);
        //static void Pagination(List<Booking> bookings)
        //{

        //}

        //static void RenderTable(List<Booking> bookings, int currentPage, int pageSize)
        //{

        //}

        void DisplayBookingInformation(Booking booking);


        void Pagination(List<Booking> bookings);
        

        void DisplayRoomsForBooking(Booking booking);
        Table CreateBookingTable(Booking booking, string header, Color borderColor);

        void ShowCurrentBooking(Booking currentBooking);
        void ShowUpdatedBookingSummary(Booking currentBooking,
            DateTime arrivalDate,
            DateTime departureDate,
            TypeOfRoom roomType,
            sbyte amountOfGuests,
            sbyte amountOfRooms);

    }
}