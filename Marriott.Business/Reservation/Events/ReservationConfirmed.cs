using System;

namespace Marriott.Business.Reservation.Events
{
    public class ReservationConfirmed
    {
        public Guid ReservationId { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
    }
}
