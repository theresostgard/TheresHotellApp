using HotellApp.Models;

namespace HotellApp.Services.GuestServices
{
    public interface IGuestService
    {
        Guest CreateGuest(Guest guest);
        Guest ReadGuest(int id);
        List<Guest> GetAllGuests();
        void UpdateGuest(int id, Guest updatedGuest);
        string DeleteGuest(int id);
        int GetLatestGuestId();
    }
}