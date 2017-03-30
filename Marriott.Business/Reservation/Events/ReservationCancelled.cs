using System;

namespace Marriott.Business.Reservation.Events
{
    public class ReservationCancelled
    {
        public Guid ReservationId { get; set; }
        public DateTime CancelationDateTime { get; set; }
    }
}
