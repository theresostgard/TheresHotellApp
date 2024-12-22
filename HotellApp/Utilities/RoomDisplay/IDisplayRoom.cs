using HotellApp.Models;
using Spectre.Console;

namespace HotellApp.Utilities.RoomDisplay
{
    public interface IDisplayRoom
    {
        Table CreateRoomTable(Room room, string header, Color borderColor);
        void DisplayRoomInformation(Room room);
        void Pagination(List<Room> rooms);
    }
}