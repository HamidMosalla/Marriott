using System;
using System.Data.Entity;
using System.Threading.Tasks;
using Marriott.Business.Reservation.Data;
using Marriott.Business.Reservation.Events;
using NServiceBus;

namespace Marriott.Business.Endpoint.Reservations.Handlers
{
    public class DeletePendingReservationHandler :IHandleMessages<PendingReservationExpired>
    {
        public async Task Handle(PendingReservationExpired message, IMessageHandlerContext context)
        {
            await DeletePendingReservation(message.ReservationId);
        }

        private async Task DeletePendingReservation(Guid reservationId)
        {
            using (var reservationContext = new ReservationContext())
            {
                var pendingReservation = await reservationContext.PendingReservations.SingleAsync(x => x.ReservationId == reservationId);
                reservationContext.PendingReservations.Remove(pendingReservation);
                await reservationContext.SaveChangesAsync();
            }
        }
    }
}
