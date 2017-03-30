using System;
using System.Collections.Generic;
using System.ComponentModel;
using Marriott.Business.Billing;
using Marriott.Business.Guest;

namespace Marriott.Client.Web.Models.Reservation
{
    public class Reservation
    {
        public Guid ReservationId { get; set; }

        [DisplayName("Check In")]
        public DateTime CheckIn { get; set; }

        [DisplayName("Check Out")]
        public DateTime CheckOut { get; set; }

        public int RoomTypeId { get; set; }

        public RoomInformation RoomInformation { get; set; }

        public GuestInformation GuestInformation { get; set; }

        public PaymentInformation PaymentInformation { get; set; }

        public List<RoomTypeIdRoomTotalAndTotalRoomRate> RoomTypeIdRoomTotalAndTotalRoomRates { get; set; } = new List<RoomTypeIdRoomTotalAndTotalRoomRate>();

        public int TimeLeftOnPendingReservation { get; set; }
    }

    public class RoomTypeIdRoomTotalAndTotalRoomRate
    {
        public int RoomTypeId { get; set; }
        public double RoomRate { get; set; }
        public double TotalRoomRate { get; set; }
    }
}