using System.Data.Entity;
using System.Threading.Tasks;
using Marriott.Business.Reservation.Commands;
using Marriott.Business.Reservation.Data;
using Marriott.Business.Reservation.Events;
using NServiceBus;

namespace Marriott.Business.Endpoint.Reservations.Handlers
{
    public class CancelReservationHandler : IHandleMessages<CancelReservation>
    {
        public async Task Handle(CancelReservation message, IMessageHandlerContext context)
        {
            using (var reservationContext = new ReservationContext())
            {
                var reservation = await reservationContext.ConfirmedReservations.SingleAsync(x => x.ReservationId == message.ReservationId);
                reservation.Cancel();
                await reservationContext.SaveChangesAsync();
            }

            await context.Publish(new ReservationCancelled { ReservationId = message.ReservationId, CancelationDateTime = message.CancelationDateTime });
        }
    }
}