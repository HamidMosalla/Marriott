using System.Linq;
using System.Threading.Tasks;
using Marriott.Business.Reservation.Data;
using Marriott.Business.Reservation.Events;
using NServiceBus;

namespace Marriott.Business.Endpoint.Reservations.Handlers
{
    public class ReservationCancellationPolicyHandler : IHandleMessages<ReservationCancelled>
    {
        public async Task Handle(ReservationCancelled message, IMessageHandlerContext context)
        {
            Reservation.ConfirmedReservation reservation;
            using (var reservationContext = new ReservationContext())
            {
                reservation = reservationContext.ConfirmedReservations.Single(x => x.ReservationId == message.ReservationId);
            }

            //has the ReservationCancellationPolicy been violated?
            //http://travelupdate.boardingarea.com/marriott-cancel-1159-pm-avoid-penalty-fees-new-policy-says/
            //"fee should be equal to one night room charge"
            //"Once the policy takes effect, consumers will have until 11:59 p.m. local time the day before check-in to cancel their reservation in order to avoid paying a penalty charge"
            var elevenFiftyNinePmTheDayBeforeCheckIn = reservation.CheckIn.AddDays(-1).AddHours(23).AddMinutes(59);
            if (message.CancelationDateTime >= elevenFiftyNinePmTheDayBeforeCheckIn)
            {
                await context.Publish(new ReservationCancellationPolicyViolated { ReservationId = message.ReservationId });
            }
        }
    }
}
