using System;

namespace Marriott.Business.Reservation.Commands
{
    public class CancelPendingReservation
    {
        public Guid ReservationId { get; set; }
    }
}
