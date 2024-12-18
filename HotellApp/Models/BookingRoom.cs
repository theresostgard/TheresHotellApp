using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotellApp.Models
{
    public class BookingRoom
    {
        public int BookingRoomId { get; set; }
        public int BookingId { get; set; }
        public int RoomId { get; set; }

        public Booking Booking { get; set; }    // En bokning är kopplad till ett rum
        public Room Room { get; set; }          // Ett rum är kopplat till en bokning
    }
}
