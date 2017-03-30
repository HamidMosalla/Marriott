using System.Threading.Tasks;
using Marriott.Business.Reservation.Messages;
using Marriott.ITOps.PaymentGateway;
using NServiceBus;

namespace Marriott.Business.Endpoint.Reservations.Handlers
{
    public class ValidateCreditCardRequestHandler : IHandleMessages<ValidateCreditCardRequest>
    {
        private readonly ICreditCardService creditCardService;

        public ValidateCreditCardRequestHandler(ICreditCardService creditCardService)
        {
            this.creditCardService = creditCardService;
        }

        public async Task Handle(ValidateCreditCardRequest message, IMessageHandlerContext context)
        {
            //simulate slow RPC
            await Task.Delay(1000);

            var validateCreditCardResponse = new ValidateCreditCardResponse { ClientId = message.ClientId };

            if (await creditCardService.Validate(message.CreditCardNumber, message.CreditCardExpiryMonth, message.CreditCardExpiryYear, message.CreditCardCcv))
            {
                validateCreditCardResponse.Succeeded = true;
            }
            else
            {
                validateCreditCardResponse.Succeeded = false;
            }

            await context.Reply(validateCreditCardResponse);
        }
    }
}
