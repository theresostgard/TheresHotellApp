using HotellApp.Models;
using HotellApp.Models.Enums;

namespace HotellApp.Controllers.RoomController
{
    public interface IRoomController
    {
        void CreateRoomController();

        void ReadAllRoomsController();

        void ReadRoomController();

        void UpdateRoomController();
        void DeleteRoomController();
        Room GetRoomDetailsFromUser(Room currentRoom);
        bool ShouldAllowExtraBed(int roomSize, TypeOfRoom roomType);



    }
}