using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotellApp.Models.Enums
{
    public enum TypeOfRoom
    {
        Double = 2,
        Single = 1
    }

    public enum StatusOfRoom
    {
        Active = 1,     //tillgängligt 
        InActive = 2,       //deletat
        UnderMaintenance = 3,
        Reserved = 4
    }


    public enum GuestType
    {
        NewGuest = 1,
        ExistingGuest = 2
    }

    public enum AmountOfExtraBedsAllowedInRoom
    {
        None = 0,
        One = 1,
        Two = 2
    }

    public enum GuestStatus
    {
        Active = 1,
        Inactive = 0
    }
}
