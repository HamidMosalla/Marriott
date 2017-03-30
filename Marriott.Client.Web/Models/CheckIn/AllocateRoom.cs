using System;
using System.ComponentModel;

namespace Marriott.Client.Web.Models.CheckIn
{
    public class AllocateRoom
    {
        public Guid ReservationId { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public int RoomTypeId { get; set; }

        [DisplayName("Reservation Id")]
        public int ExternalId { get; set; }
        public bool RoomAllocationFailed { get; set; }
        public int RoomNumber { get; set; }
    }
}