using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotellApp.Controllers.BookingController
{
    public interface IBookingController
    {
        void CreateBookingController();
        void DeleteBookingController();
        void ReadBookingController();
        void UpdateBookingController();

        void ReadAllBookingsController();
    }
}
