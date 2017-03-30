using System;

namespace Marriott.Business.Reservation.Commands
{
    public class ConfirmPendingReservation
    {
        public Guid ReservationId { get; set; }
    }
}
