using System;
using System.ComponentModel;

namespace Marriott.Client.Web.Models.CheckIn
{
    public class ReservationInformation
    {
        public int ExternalId { get; set; }

        [DisplayName("Check In")]
        public DateTime CheckIn { get; set; }

        [DisplayName("Check In")]
        public DateTime CheckOut { get; set; }

        [DisplayName("Room Type")]
        public string RoomTypeName { get; set; }

        [DisplayName("Total Nights of Stay")]
        public int TotalNightsOfStay { get; set; }

        [DisplayName("Room Rate per Night")]
        public double RoomRate { get; set; }

        [DisplayName("Total Room Rate")]
        public double TotalRoomRate { get; set; }
    }
}