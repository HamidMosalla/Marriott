using System;

namespace Marriott.Business.RoomInventory.Commands
{
    public class DeallocateRoomForNonPaymentAtCheckIn
    {
        public Guid ReservationId { get; set; }
    }
}
