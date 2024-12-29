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
        bool ChangeStatusOnRoom(int roomId, StatusOfRoom newStatus);

        void ChangeRoomStatusForDateRange(int roomId, StatusOfRoom newStatus, DateTime startDate, DateTime endDate);
        List<Room> GetAvailableRooms(TypeOfRoom roomType, DateTime arrivalDate, DateTime departureDate, sbyte amountOfRooms);
        
    }
}