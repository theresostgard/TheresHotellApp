using HotellApp.Models;

namespace HotellApp.Services.GuestServices
{
    public interface IGuestService
    {
        void CreateGuest(Guest guest);
        void ReadGuest(int id);
        void UpdateGuest(int id, Guest updatedGuest);
        void DeleteGuest(int id);
    }
}