using HotellApp.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotellApp.Models
{
    public class Booking 
    {
        public int BookingId { get; set; }
        public DateTime ArrivalDate { get; set; }

        public DateTime DepartureDate { get; set; }

        public TypeOfRoom RoomType { get; set; }

        public sbyte AmountOfGuests { get; set; }

        public sbyte AmountOfRooms { get; set; }

        public int GuestId { get; set; }     //behöver hämtas från customerentiteten eller genereras ny (via detsamma)

        public int RoomId { get; set; }

        public ICollection<BookingRoom> BookingRooms { get; set; }

        // Navigation property till Guest
        public Guest Guest { get; set; }  // En bokning

    }
}
