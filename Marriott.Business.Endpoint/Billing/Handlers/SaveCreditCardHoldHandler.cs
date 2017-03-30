using System.Threading.Tasks;
using Marriott.Business.Billing.Commands;
using Marriott.Business.Billing.Data;
using NServiceBus;

namespace Marriott.Business.Endpoint.Billing.Handlers
{
    public class SaveCreditCardHoldHandler : IHandleMessages<SaveCreditCardHold>
    {
        public async Task Handle(SaveCreditCardHold message, IMessageHandlerContext context)
        {
            using (var billingContext = new BillingContext())
            {
                billingContext.CreditCardHold.Add(message.CreditCardHold);
                await billingContext.SaveChangesAsync();
            }
        }
    }
}
