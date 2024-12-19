﻿using HotellApp.Models;

namespace HotellApp.Services.GuestServices
{
    public interface IGuestService
    {
        Guest CreateGuest(Guest guest);
        Guest ReadGuest(int id);
        void UpdateGuest(int id, Guest updatedGuest);
        void DeleteGuest(int id);
        int GetLatestGuestId();
    }
}