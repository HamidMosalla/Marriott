using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Marriott.Business.Housekeeping
{
    public class TodaysCheckedOutRooms
    {
        [Key, Column(Order = 0)]
        public int RoomNumber { get; set; }
        [Key, Column(Order = 1)]
        public DateTime CheckOutDate { get; set; }
    }
}
