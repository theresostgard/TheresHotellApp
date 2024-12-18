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
        Active = 1,
        InActive = 2,
        UnderMaintenance = 3,
        Reserved = 4
    }


    public enum GuestType
    {
        NewCustomer = 1,
        ExistingCustomer = 2
    }

    public enum AmountOfExtraBedsAllowedInRoom
    {
        None = 0,
        One = 1,
        Two = 2
    }
}
