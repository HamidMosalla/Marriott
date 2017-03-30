using System.Data.Entity;
using System.Threading.Tasks;
using Marriott.Business.Billing.Data;
using Marriott.External.Events;
using NServiceBus;

namespace Marriott.Business.Endpoint.Billing.Handlers
{
    public class GuestCheckedOutHandler : IHandleMessages<GuestCheckedOut>
    {
        public async Task Handle(GuestCheckedOut message, IMessageHandlerContext context)
        {
            using (var billingContext = new BillingContext())
            {
                var creditCardHold = await billingContext.CreditCardHold.SingleAsync(x => x.ReservationdId == message.ReservationId);
                creditCardHold.Release();
                await billingContext.SaveChangesAsync();
            }
        }
    }
}