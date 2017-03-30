using System;

namespace Marriott.External.Events
{
    public class GuestCheckedOut
    {
        public Guid ReservationId { get; set; }
        public DateTime CheckedInDate { get; set; }
        public DateTime CheckedOutDate { get; set; }
        public int RoomNumber { get; set; }
    }
}
