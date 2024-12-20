using HotellApp.Models;
using HotellApp.Models.Enums;

namespace HotellApp.Services.RoomServices
{
    public interface IRoomService
    {
        void CreateRoom(Room room);
        Room ReadRoom(int roomId);
        List<Room> GetAllRooms();
        void UpdateRoom(int roomId, Room updatedRoom);
        void DeleteRoom(int roomId, StatusOfRoom newStatus);
    }
}