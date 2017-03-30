using System;

namespace Marriott.External.Events
{
    public class RoomAllocationFailed
    {
        public Guid ReservationId { get; set; }
    }
}
