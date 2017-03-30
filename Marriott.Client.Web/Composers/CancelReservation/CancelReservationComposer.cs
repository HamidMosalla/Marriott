using System;
using System.Data.Entity;
using System.Threading.Tasks;
using Marriott.Business.Reservation.Data;

namespace Marriott.Client.Web.Composers.CancelReservation
{
    public class CancelReservationComposer
    {
        public async Task<Guid> ComposeCancelReservation(int externalId)
        {
            using (var context = new ReservationContext())
            {
                var reservation = await context.ConfirmedReservations.SingleAsync(x => x.ExternalId == externalId);
                return reservation.ReservationId;
            }
        }
    }
}