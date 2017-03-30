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
    public class ReservationNoShowPolicyViolatedHandler : IHandleMessages<ReservationNoShowPolicyViolated>
    {
        public async Task Handle(ReservationNoShowPolicyViolated message, IMessageHandlerContext context)
        {
            double noShowFee;
            using (var pricingContext = new PricingContext())
            {
                var reservationRoomRate = await pricingContext.ReservationRoomRate.SingleAsync(x => x.ReservationId == message.ReservationId);
                noShowFee = reservationRoomRate.RoomRate;
            }

            PaymentInformation paymentInformation;
            using (var billingContext = new BillingContext())
            {
                paymentInformation = await billingContext.PaymentInformation.SingleAsync(x => x.ReservationId == message.ReservationId);
            }

            await context.Send(new ChargeReservationNoShowFee
            {
                CreditCardNumber = paymentInformation.CreditCardNumber,
                CreditCardExpiryMonth = paymentInformation.CreditCardExpiryMonth,
                CreditCardExpiryYear = paymentInformation.CreditCardExpiryYear,
                CreditCardCcv = paymentInformation.CreditCardCcv,
                NoShowFee = noShowFee
            });
        }
    }
}