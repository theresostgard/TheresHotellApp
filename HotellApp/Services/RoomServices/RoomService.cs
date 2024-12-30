using HotellApp.Data;
using HotellApp.Models;
using HotellApp.Models.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotellApp.Services.RoomServices
{
    public class RoomService : IRoomService
    {
        private readonly ApplicationDbContext _dbContext;

        public RoomService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;

        }
        public void CreateRoom(Room room)
        {
            _dbContext.Room.Add(room);
            _dbContext.SaveChanges();
        }

        public Room ReadRoom(int roomId)
        {
            var room = _dbContext.Room.FirstOrDefault(r => r.RoomId == roomId);

            return room;
        }

        public List<Room> GetAllRooms()
        {
            if (_dbContext.Room == null)
            {
                Console.WriteLine("Inga rum hittades.");
            }
            return _dbContext.Room.ToList();
        }

        public void UpdateRoom(int roomId, Room updatedRoom)
        {
            var room = _dbContext.Room.FirstOrDefault(r => r.RoomId == roomId);

            if (room != null)
            {
                room.RoomType = updatedRoom.RoomType;
                room.RoomSize = updatedRoom.RoomSize;
                room.IsExtraBedAllowed = updatedRoom.IsExtraBedAllowed;
                room.AmountOfExtraBeds = updatedRoom.AmountOfExtraBeds;
                room.PricePerNight = updatedRoom.PricePerNight;
                

                _dbContext.SaveChanges();
               
            }
            else
            {
                Console.WriteLine("Inget rum hittades.");
            }

        }
        public bool ChangeStatusOnRoom(int roomId, StatusOfRoom newStatus)
        {

            if (HasUpcomingBookings(roomId))
            {
                Console.WriteLine("Kan inte ändra status på rummet, " +
                    "det finns bokningar de kommande 3 månaderna.");
                return false;
            }

            var room = _dbContext.Room.FirstOrDefault(r => r.RoomId == roomId);

            if (room != null)
            {
                room.Status = newStatus;
                _dbContext.SaveChanges();
                return true;
            }
            else
            {
                Console.WriteLine("Inget rum hittades.");
                return false;
            }

        }

        private bool HasUpcomingBookings(int roomId)
        {
            var bookings = _dbContext.BookingRoom
                .Include(br => br.Booking)
                .Where(br => br.RoomId == roomId &&
                             br.Booking.ArrivalDate <= DateTime.Now.AddMonths(3) &&
                             br.Booking.DepartureDate >= DateTime.Now)
                .ToList();

            Console.WriteLine($"Antal bokningar hittade för rummet: {bookings.Count}");

            return bookings.Count > 0;
        }

        public void ChangeRoomStatusForDateRange(int roomId, StatusOfRoom newStatus, DateTime startDate, DateTime endDate)
        {
            var existingStatus = _dbContext.RoomStatusHistory
                .FirstOrDefault(rsh => rsh.RoomId == roomId && rsh.StartDate <= startDate && (rsh.EndDate == null || rsh.EndDate >= startDate));

            if (existingStatus != null)
            {
                existingStatus.Status = newStatus;
                existingStatus.StartDate = startDate;
                existingStatus.EndDate = endDate;
            }
            else
            {
                var newRoomStatusHistory = new RoomStatusHistory
                {
                    RoomId = roomId,
                    Status = newStatus,
                    StartDate = startDate,
                    EndDate = endDate
                };
                _dbContext.RoomStatusHistory.Add(newRoomStatusHistory);
            }

            _dbContext.SaveChanges();
        }

        public List<Room> GetAvailableRooms(TypeOfRoom roomType, DateTime arrivalDate, DateTime departureDate, sbyte amountOfRooms)
        {
            var allRooms = _dbContext.Room
                .Where(r => r.RoomType == roomType)
                .ToList();

            var availableRooms = allRooms.Where(room =>
            {
                var roomStatuses = _dbContext.RoomStatusHistory
                    .Where(rsh => rsh.RoomId == room.RoomId &&
                                  rsh.StartDate < departureDate &&
                                  (rsh.EndDate == null || rsh.EndDate > arrivalDate))
                    .ToList();

                return !roomStatuses.Any();
            }).ToList();

            return availableRooms;
        }


    }
}
