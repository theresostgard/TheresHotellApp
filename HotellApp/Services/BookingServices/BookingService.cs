﻿using HotellApp.Data;
using HotellApp.Models;
using HotellApp.Models.Enums;
using HotellApp.Services.RoomServices;
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
            UpdateBookingStatus(booking);
            _dbContext.Booking.Add(booking);
            _dbContext.SaveChanges();
        }

        public void UpdateBookingStatus(Booking booking)
        {
            if (booking.ArrivalDate < DateTime.Today)
            {
                booking.Status = BookingStatus.Inactive;
            }
            else
            {
                booking.Status = BookingStatus.Active;
            }
        }

        public void AddRoomsToBooking(List<BookingRoom> bookingRooms)
        {
            _dbContext.BookingRoom.AddRange(bookingRooms); 
            _dbContext.SaveChanges(); 
        }


        public Booking ReadBooking(int bookingId)
        {
            var booking = _dbContext.Booking
            .Include(b => b.BookingRooms)
            .ThenInclude(br => br.Room)  
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

                _dbContext.SaveChanges();
                Console.WriteLine($"Bokningen med bokningsnr {bookingId} har uppdaterats!");
            }
            else
            {
                Console.WriteLine("Ingen bokning hittades.");
            }
        }

        public string DeleteBooking(int id)
        {
            var booking = _dbContext.Booking.FirstOrDefault(b => b.BookingId == id);
            if (booking == null)
            {
                return "Bokningen hittades inte.";
            }

            if (booking.ArrivalDate <= DateTime.Now && booking.DepartureDate >= DateTime.Now)
            {
                return "Bokningen kan inte raderas eftersom den redan pågår eller har avslutats.";
            }

            if (booking.ArrivalDate > DateTime.Now)
            {
                _dbContext.Booking.Remove(booking);
                _dbContext.SaveChanges();
                return "Bokningen har raderats framgångsrikt!";
            }

            return "Bokningen kan inte raderas eftersom den redan är avslutad.";
        }
        public bool TryGetAvailableRoomsForBooking(TypeOfRoom roomType, DateTime arrivalDate, DateTime departureDate, int amountOfRooms, out List<Room> availableRooms)
        {
       
            availableRooms = _dbContext.Room
                .Where(r => r.RoomType == roomType && r.Status == StatusOfRoom.Active)
                .ToList();

            availableRooms = availableRooms.Where(r => !_dbContext.BookingRoom
            .Any(br => br.RoomId == r.RoomId &&
                  ((arrivalDate >= br.Booking.ArrivalDate && arrivalDate <= br.Booking.DepartureDate) ||
                   (departureDate >= br.Booking.ArrivalDate && departureDate <= br.Booking.DepartureDate))))
            .ToList();


            if (roomType == TypeOfRoom.Double)
            {
                availableRooms = availableRooms.Where(r => r.RoomSize >= 15).ToList(); 


                foreach (var room in availableRooms)
                {
                    if (room.RoomSize < 30)
                    {
                        room.AmountOfExtraBeds = Models.Enums.AmountOfExtraBedsAllowedInRoom.One; 
                    }
                    else
                    {
                        room.AmountOfExtraBeds = Models.Enums.AmountOfExtraBedsAllowedInRoom.Two;  
                    }
                }
            }

            return availableRooms.Count >= amountOfRooms;
        }
       
    }
}
