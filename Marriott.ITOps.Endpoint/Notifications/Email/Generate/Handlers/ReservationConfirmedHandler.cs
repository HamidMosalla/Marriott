using System.Threading.Tasks;
using Marriott.Business.Reservation.Events;
using Marriott.ITOps.Notifications.Email.Commands;
using NServiceBus;

namespace Marriott.ITOps.Endpoint.Notifications.Email.Generate.Handlers
{
    public class ReservationConfirmedHandler : IHandleMessages<ReservationConfirmed>
    {
        public async Task Handle(ReservationConfirmed message, IMessageHandlerContext context)
        {
            await context.Send(new SendReservationConfirmationEmail { ReservationId = message.ReservationId });
        }
    }
}
