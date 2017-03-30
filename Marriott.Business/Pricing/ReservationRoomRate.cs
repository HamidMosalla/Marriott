using System;
using System.ComponentModel.DataAnnotations;

namespace Marriott.Business.Pricing
{
    public class ReservationRoomRate
    {
        [Key]
        public Guid ReservationId { get; set; }
        public double RoomRate { get; set; }
        public double TotalRoomRate { get; set; }
    }
}
