using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotellApp.Models.Enums
{
    public enum TypeOfRoom
    {
        Double,
        Single
    }

    public enum StatusOfRoom
    {
        Active,     //tillgängligt 
        InActive,       //deletat
        UnderMaintenance,
        Reserved
    }


    public enum GuestType
    {
        NewGuest,
        ExistingGuest
    }

    public enum AmountOfExtraBedsAllowedInRoom
    {
        None = 0,   
        One = 1,
        Two = 2,
    }

    public enum GuestStatus
    {
        Active,
        Inactive
    }

    public enum BookingStatus
    {
        Active,
        Inactive
    }
}
