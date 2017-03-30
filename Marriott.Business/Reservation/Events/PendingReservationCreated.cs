using System;

namespace Marriott.Business.Reservation.Events
{
    public class PendingReservationCreated
    {
        public Guid ReservationId { get; set; }
    }
}
