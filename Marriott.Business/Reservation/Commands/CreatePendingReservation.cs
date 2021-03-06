﻿using System;

namespace Marriott.Business.Reservation.Commands
{
    public class CreatePendingReservation
    {
        public Guid ReservationId { get; set; }
        public int RoomTypeId { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
    }
}
