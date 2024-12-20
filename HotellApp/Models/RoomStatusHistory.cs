using HotellApp.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotellApp.Models
{
    public class RoomStatusHistory
    {
        public int RoomStatusHistoryId { get; set; }
        public int RoomId { get; set; }
        public StatusOfRoom Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; } // Kan vara null om statusen är aktuell
                                              // Navigation property
        public Room Room { get; set; } // Relaterad rum (valfritt beroende på om du använder Entity Framework för navigering)
    }
}
