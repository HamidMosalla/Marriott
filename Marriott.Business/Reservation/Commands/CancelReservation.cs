using System;

namespace Marriott.Business.Reservation.Commands
{
    public class CancelReservation
    {
        public Guid ReservationId { get; set; }
        public DateTime CancelationDateTime { get; set; }
    }
}
