using System.Threading.Tasks;
using Marriott.ITOps.PaymentGateway;
using Marriott.ITOps.PaymentGateway.Commands;
using NServiceBus;

namespace Marriott.ITOps.Endpoint.PaymentGateway.Send.Handlers
{
    public class ChargeCreditCardHandler : IHandleMessages<ChargeReservationNoShowFee>,
        IHandleMessages<ChargeCreditCardForTotalStay>,
        IHandleMessages<ChargeReservationCancellationPenaltyFee>
    {
        private readonly ICreditCardService creditCardService;

        public ChargeCreditCardHandler(ICreditCardService creditCardService)
        {
            this.creditCardService = creditCardService;
        }

        public async Task Handle(ChargeReservationNoShowFee message, IMessageHandlerContext context)
        {
            await ChargeCreditCard(message.CreditCardNumber, message.CreditCardExpiryMonth, message.CreditCardExpiryYear, message.CreditCardCcv,
                message.NoShowFee);
        }

        public async Task Handle(ChargeCreditCardForTotalStay message, IMessageHandlerContext context)
        {
            await ChargeCreditCard(message.CreditCardNumber, message.CreditCardExpiryMonth, message.CreditCardExpiryYear, message.CreditCardCcv,
                message.RoomTotal);
        }

        public async Task Handle(ChargeReservationCancellationPenaltyFee message, IMessageHandlerContext context)
        {
            await ChargeCreditCard(message.CreditCardNumber, message.CreditCardExpiryMonth, message.CreditCardExpiryYear, message.CreditCardCcv,
                message.PenaltyFee);
        }

        private async Task ChargeCreditCard(int creditCardNumber, int creditCardExpiryMonth, int creditCardExpiryYear, int creditCardCcv, double amount)
        {
            await creditCardService.Charge(creditCardNumber, creditCardExpiryMonth, creditCardExpiryYear, creditCardCcv, amount);
        }
    }
}
