using System;

namespace Marriott.Business.RoomInventory.Commands
{
    public class AllocateRoom
    {
        public Guid ReservationId { get; set; }
        public int RoomTypeId { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
    }
}
