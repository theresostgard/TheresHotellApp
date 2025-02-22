﻿using HotellApp.Models.Enums;
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

        public int GuestId { get; set; }    

        public Guest Guest { get; set; } 
       
        public int AmountOfExtraBeds { get; set; } 

        public ICollection<BookingRoom> BookingRooms { get; set; }

        public BookingStatus Status { get; set; }


    }
}
