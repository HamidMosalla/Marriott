using System.Threading.Tasks;
using Marriott.Business.Reservation;
using Marriott.Business.Reservation.Commands;
using Marriott.Business.Reservation.Data;
using Marriott.Business.Reservation.Events;
using NServiceBus;

namespace Marriott.Business.Endpoint.Reservations.Handlers
{
    public class CreatePendingReservationHandler : IHandleMessages<CreatePendingReservation>
    {
        public async Task Handle(CreatePendingReservation message, IMessageHandlerContext context)
        {
            using (var reservationContext = new ReservationContext())
            {
                reservationContext.PendingReservations.Add(new PendingReservation { ReservationId = message.ReservationId, RoomTypeId = message.RoomTypeId, CheckIn = message.CheckIn, CheckOut = message.CheckOut });
                await reservationContext.SaveChangesAsync();
            }
            await context.Publish(new PendingReservationCreated { ReservationId = message.ReservationId });
        }
    }
}