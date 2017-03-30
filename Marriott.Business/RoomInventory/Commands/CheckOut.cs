using System;

namespace Marriott.Business.RoomInventory.Commands
{
    public class CheckOut
    {
        public Guid ReservationId { get; set; }
        public DateTime CheckOutDate { get; set; }
    }
}
