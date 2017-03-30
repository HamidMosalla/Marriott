using System;

namespace Marriott.External.Events
{
    public class GuestStayExtended
    {
        public Guid ReservationId { get; set; }
        public DateTime NewCheckOutDate { get; set; }
    }
}
