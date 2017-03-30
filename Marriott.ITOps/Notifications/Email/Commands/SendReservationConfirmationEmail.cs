using System;

namespace Marriott.ITOps.Notifications.Email.Commands
{
    public class SendReservationConfirmationEmail
    {
        public Guid ReservationId { get; set; }
    }
}
