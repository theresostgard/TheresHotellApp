using HotellApp.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotellApp.Controllers.GuestController
{
    public interface IGuestController
    {
        void CreateGuestController();
        void UpdateGuestController();
        void ReadGuestController();

        void ReadAllGuestsController();
        void DeleteGuestController();
        int GetLatestGuestId();
        (GuestType, int?) SelectCustomerType();

    }
}
