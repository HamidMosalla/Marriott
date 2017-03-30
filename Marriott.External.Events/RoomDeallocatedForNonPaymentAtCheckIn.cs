using System;

namespace Marriott.External.Events
{
    public class RoomDeallocatedForNonPaymentAtCheckIn
    {
        public Guid ReservationId { get; set; }
    }
}
