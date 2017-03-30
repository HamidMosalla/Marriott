using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Marriott.Business.Reservation
{
    //this entity represents resevation's that have not been confirmed yet via the Reservation UC/UI
    public class PendingReservation
    {
        [Key]
        public Guid ReservationId { get; set; }

        public int RoomTypeId { get; set; }

        [DisplayName("Check In")]
        public DateTime CheckIn { get; set; }

        [DisplayName("Check Out")]
        public DateTime CheckOut { get; set; }

        public ConfirmedReservation Confirm()
        {
            return new ConfirmedReservation
            {
                ReservationId = this.ReservationId,
                RoomTypeId = this.RoomTypeId,
                CheckIn = this.CheckIn,
                CheckOut = this.CheckOut
            };
        }
    }
}
