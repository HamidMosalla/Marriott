using System;

namespace Marriott.Business.Reservation.Events
{
    public class ReservationCancellationPolicyViolated
    {
        public Guid ReservationId { get; set; }
    }
}
