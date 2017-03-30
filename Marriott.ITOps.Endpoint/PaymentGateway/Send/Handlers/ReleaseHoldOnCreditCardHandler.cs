using System.Threading.Tasks;
using Marriott.ITOps.PaymentGateway;
using Marriott.ITOps.PaymentGateway.Commands;
using NServiceBus;

namespace Marriott.ITOps.Endpoint.PaymentGateway.Send.Handlers
{
    public class ReleaseHoldOnCreditCardHandler : IHandleMessages<ReleaseHoldOnCreditCard>
    {
        private readonly ICreditCardService creditCardService;

        public ReleaseHoldOnCreditCardHandler(ICreditCardService creditCardService)
        {
            this.creditCardService = creditCardService;
        }

        public async Task Handle(ReleaseHoldOnCreditCard message, IMessageHandlerContext context)
        {
            await creditCardService.ReleaseHoldOn(message.CreditCardNumber, message.CreditCardExpiryMonth, message.CreditCardExpiryYear,
                message.CreditCardCcv, message.HoldAmount);
        }
    }
}