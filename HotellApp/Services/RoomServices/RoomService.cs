using HotellApp.Data;
using HotellApp.Models;
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
            //_dbContext.SaveChanges();
        }

        public Room ReadRoom(int roomId)
        {
            var room = _dbContext.Room.FirstOrDefault(r => r.RoomID == roomId);

            return room;
        }

        public List<Room> GetAllRooms()
        {
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
                //room.Status = updatedRoom.Status; eventuellt överflödigt, ska man endast i DELETE kunna ändra status?

                //_dbContext.SaveChanges();
                Console.WriteLine($"Rummet med rumsnr {roomId} har uppdaterats!");
            }
            else
            {
                Console.WriteLine("Inget rum hittades.");
            }

        }
        public void DeleteRoom(int roomId)
        {
            //var roomToChangeStatusOn = _dbContext.Rooms.FirstOrDefault(r => r.RoomID == roomId);
            //if (roomToChangeStatusOn != null)
            //{

            //    if (roomToChangeStatusOn.Status = StatusOfRoom.Active)
            //    {
            //        roomToChangeStatusOn.Status = StatusOfRoom.InActive;
            //        Console.WriteLine($"Rummet med rumsnr {roomId} är nu markerat som inaktivt i systemet.");
            //        //_dbContext.SaveChanges();
            //    }
            //    else
            //    {
            //        roomToChangeStatusOn.Status = StatusOfRoom.Active;
            //        Console.WriteLine($"Rummet med rumsnr {roomId} är nu markerat som aktivt i systemet.");
            //        //_dbContext.SaveChanges();
            //    }
            //}
            //else
            //{
            //    Console.WriteLine($"Rummet med rumsnr {roomId} hittades inte.");
            //}
        }


    }
}
