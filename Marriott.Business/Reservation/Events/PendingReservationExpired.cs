using System;

namespace Marriott.Business.Reservation.Events
{
    public class PendingReservationExpired
    {
        public Guid ReservationId { get; set; }
    }
}
