using HotellApp.Models.Enums;

namespace HotellApp.Models
{
    public class Room
    {

        public int RoomId { get; set; }
        public TypeOfRoom RoomType { get; set; }
        public int RoomSize { get; set; }
        public bool? IsExtraBedAllowed { get; set; }

        public AmountOfExtraBedsAllowedInRoom? AmountOfExtraBeds { get; set; }

        public StatusOfRoom Status { get; set; } = StatusOfRoom.Active;

        public decimal PricePerNight { get; set; }

        public ICollection<BookingRoom>? BookingRooms { get; set; }

    }

}
