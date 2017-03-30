using System;
using System.Threading.Tasks;
using Marriott.Business.Reservation.Commands;
using Marriott.External.Events;
using NServiceBus;

namespace Marriott.Business.Endpoint.Reservations.Handlers
{
    public class RoomDeallocatedForNonPaymentAtCheckInHandler : IHandleMessages<RoomDeallocatedForNonPaymentAtCheckIn>
    {
        public async Task Handle(RoomDeallocatedForNonPaymentAtCheckIn message, IMessageHandlerContext context)
        {
            await context.Send(new CancelReservation { ReservationId = message.ReservationId, CancelationDateTime = DateTime.Now });
        }
    }
}
