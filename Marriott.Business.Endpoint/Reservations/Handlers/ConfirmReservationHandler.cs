using System.Data.Entity;
using System.Threading.Tasks;
using Marriott.Business.Reservation.Commands;
using Marriott.Business.Reservation.Data;
using Marriott.Business.Reservation.Events;
using NServiceBus;

namespace Marriott.Business.Endpoint.Reservations.Handlers
{
    public class ConfirmReservationHandler : IHandleMessages<ConfirmReservation>
    {
        public async Task Handle(ConfirmReservation message, IMessageHandlerContext context)
        {
            using (var reservationContext = new ReservationContext())
            {
                var pendingReservation = await reservationContext.PendingReservations.SingleAsync(x => x.ReservationId == message.ReservationId);
                reservationContext.ConfirmedReservations.Add(pendingReservation.Confirm());

                //this removal used to take when DeletePendingReservationHandler handled ReservationConfirmed, but for transactional consistency, I put it here. Not too sure if it's 
                //the best place for it
                reservationContext.PendingReservations.Remove(pendingReservation);

                await reservationContext.SaveChangesAsync();
            }

            await context.Publish(new ReservationConfirmed { ReservationId = message.ReservationId, CheckIn = message.CheckIn, CheckOut = message.CheckOut });
        }
    }
}
