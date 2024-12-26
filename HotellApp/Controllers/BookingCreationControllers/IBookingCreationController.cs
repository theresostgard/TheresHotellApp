using HotellApp.Models;
using HotellApp.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotellApp.Controllers.BookingCreationController
{
    public interface IBookingCreationController
    {
        DateTime GetValidArrivalDate();

        DateTime GetValidDepartureDate(DateTime arrivalDate);

        (TypeOfRoom roomType, sbyte amountOfGuests, sbyte amountOfRooms, int amountOfExtraBeds) GetRoomDetails();

        List<Room> CheckRoomAvailability(TypeOfRoom roomType,
            DateTime arrivalDate,
            DateTime departureDate,
            sbyte amountOfRooms);

        List<Room> FilterRoomsForExtraBeds(List<Room> availableRooms, bool isExtraBedAllowed);

        void ShowAvailableRooms(List<Room> availableRooms);

        List<Room> SelectRooms(List<Room> availableRooms, sbyte amountOfRooms);

        Booking CreateBooking(int guestId,
            DateTime arrivalDate,
            DateTime departureDate,
            TypeOfRoom roomType,
            sbyte amountOfGuests,
            int amountOfRooms);

        void AssignRoomsToBooking(List<Room> selectedRooms,
            Booking booking,
            DateTime arrivalDate,
            DateTime departureDate);
    }
}
