using System;

namespace Marriott.Business.Billing.Events
{
    public class HoldOnCreditCardPlaced
    {
        public Guid ReservationId { get; set; }
        public int RoomNumber { get; set; }
        public double HoldAmount { get; set; }
    }
}
