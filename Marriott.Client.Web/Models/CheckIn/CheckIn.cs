using System;
using System.ComponentModel;
using Marriott.Business.Billing;
using Marriott.Business.Guest;

namespace Marriott.Client.Web.Models.CheckIn
{
    public class CheckIn
    {
        public Guid ReservationId { get; set; }

        public Business.Reservation.ConfirmedReservation Reservation { get; set; }
        public GuestInformation GuestInformation { get; set; }
        public PaymentInformation PaymentInformation { get; set; }

        [DisplayName("Room Type")]
        public string RoomTypeName { get; set; }

        [DisplayName("Room Rate")]
        public double RoomRate { get; set; }

        [DisplayName("Total Room Rate")]
        public double TotalRoomRate { get; set; }

        [DisplayName("Room Number")]
        public int RoomNumber { get; set; }
    }
}