using System;
using System.ComponentModel;

namespace Marriott.Client.Web.Models.CheckIn
{
    public class VerifyGuestInformation
    {
        public Guid ReservationId { get; set; }

        public Business.Reservation.ConfirmedReservation Reservation { get; set; }

        [DisplayName("Room Type")]
        public string RoomTypeName { get; set; }

        [DisplayName("Room Rate per Night")]
        public double RoomRate { get; set; }

        [DisplayName("Total Room Rate")]
        public double TotalRoomRate { get; set; }
    }
}