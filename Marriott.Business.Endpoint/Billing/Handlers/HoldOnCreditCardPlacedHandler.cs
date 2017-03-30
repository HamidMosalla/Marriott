using System.Threading.Tasks;
using Marriott.Business.Billing;
using Marriott.Business.Billing.Commands;
using Marriott.Business.Billing.Events;
using NServiceBus;

namespace Marriott.Business.Endpoint.Billing.Handlers
{
    public class HoldOnCreditCardPlacedHandler : IHandleMessages<HoldOnCreditCardPlaced>
    {
        public async Task Handle(HoldOnCreditCardPlaced message, IMessageHandlerContext context)
        {
            await context.Send(new SaveCreditCardHold { CreditCardHold = new CreditCardHold { ReservationdId = message.ReservationId, HoldAmount = message.HoldAmount }});
        }
    }
}
