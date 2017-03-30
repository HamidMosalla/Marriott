using System.Data.Entity;
using System.Threading.Tasks;
using Marriott.Business.Billing;
using Marriott.Business.Billing.Data;
using Marriott.Business.Pricing.Data;
using Marriott.Business.Reservation.Events;
using Marriott.ITOps.PaymentGateway.Commands;
using NServiceBus;

namespace Marriott.ITOps.Endpoint.PaymentGateway.Read.Handlers
{
    public class ReservationCancellationPolicyViolatedHandler : IHandleMessages<ReservationCancellationPolicyViolated>
    {
        public async Task Handle(ReservationCancellationPolicyViolated message, IMessageHandlerContext context)
        {
            double penaltyFee;
            using (var pricingContext = new PricingContext())
            {
                var room = await pricingContext.ReservationRoomRate.SingleAsync(x => x.ReservationId == message.ReservationId);
                penaltyFee = room.RoomRate;
            }

            PaymentInformation paymentInformation;
            using (var billingContext = new BillingContext())
            {
                paymentInformation = await billingContext.PaymentInformation.SingleAsync(x => x.ReservationId == message.ReservationId);
            }

            await context.Send(new ChargeReservationCancellationPenaltyFee
            {
                CreditCardNumber = paymentInformation.CreditCardNumber,
                CreditCardExpiryMonth = paymentInformation.CreditCardExpiryMonth,
                CreditCardExpiryYear = paymentInformation.CreditCardExpiryYear,
                CreditCardCcv = paymentInformation.CreditCardCcv,
                PenaltyFee = penaltyFee
            });
        }
    }
}
