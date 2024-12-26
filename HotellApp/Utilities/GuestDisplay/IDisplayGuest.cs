using HotellApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotellApp.Utilities.GuestDisplay
{
    public interface IDisplayGuest
    {
        void DisplayGuestInformation(Guest guest);
    }
}
