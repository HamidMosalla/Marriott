using System;

namespace Marriott.External.Events
{
    public class GuestCheckedIn
    {
        public Guid ReservationId { get; set; }
        public DateTime CheckedInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
    }
}
