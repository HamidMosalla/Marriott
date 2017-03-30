using System;

namespace Marriott.Business.Reservation.Events
{
    public class ReservationNoShowPolicyViolated
    {
        public Guid ReservationId { get; set; }
    }
}