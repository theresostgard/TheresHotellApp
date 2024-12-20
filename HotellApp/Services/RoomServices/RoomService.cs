using HotellApp.Data;
using HotellApp.Models;
using HotellApp.Models.Enums;
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
            var room = _dbContext.Room.FirstOrDefault(r => r.RoomID == roomId);

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
            var room = _dbContext.Room.FirstOrDefault(r => r.RoomID == roomId);

            if (room != null)
            {
                room.RoomType = updatedRoom.RoomType;
                room.RoomSize = updatedRoom.RoomSize;
                room.IsExtraBedAllowed = updatedRoom.IsExtraBedAllowed;
                room.AmountOfExtraBeds = updatedRoom.AmountOfExtraBeds;
                

                _dbContext.SaveChanges();
               
            }
            else
            {
                Console.WriteLine("Inget rum hittades.");
            }

        }
        public void DeleteRoom(int roomId, StatusOfRoom newStatus)
        {
            
            var room = _dbContext.Room.FirstOrDefault(r => r.RoomID == roomId);
            if (room != null)
            {
                room.Status = newStatus;  // Sätt den nya statusen för rummet
                _dbContext.SaveChanges();
            }
            else
            {
                Console.WriteLine($"Rummet med rumsnr {roomId} hittades inte.");
            }
           
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
            // Hitta alla rum av den angivna typen
            var allRooms = _dbContext.Room.Where(r => r.RoomType == roomType).ToList();

            // Hitta rummen som är tillgängliga för det angivna datumintervallet
            var availableRooms = new List<Room>();

            foreach (var room in allRooms)
            {
                var roomStatuses = _dbContext.RoomStatusHistory
                    .Where(rsh => rsh.RoomId == room.RoomID && rsh.StartDate < departureDate && (rsh.EndDate == null || rsh.EndDate > arrivalDate))
                    .ToList();

                // Om rummet inte har några statushistorik som blockerar det under den här perioden, är det tillgängligt
                if (!roomStatuses.Any())
                {
                    availableRooms.Add(room);
                }
            }

            return availableRooms.Take(amountOfRooms).ToList();
        }


    }
}
