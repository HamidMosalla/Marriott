using System.Data.Entity.Migrations;
using System.Threading.Tasks;
using Marriott.Business.Billing;
using Marriott.Business.Billing.Commands;
using Marriott.Business.Billing.Data;
using NServiceBus;

namespace Marriott.Business.Endpoint.Billing.Handlers
{
    public class SavePaymentInformationHandler : IHandleMessages<SavePaymentInformation>
    {
        public async Task Handle(SavePaymentInformation message, IMessageHandlerContext context)
        {
            using (var billingConext = new BillingContext())
            {
                billingConext.Set<PaymentInformation>().AddOrUpdate(message.PaymentInformation);
                await billingConext.SaveChangesAsync();
            }
        }
    }
}
