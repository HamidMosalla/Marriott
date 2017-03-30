using System.Data.Entity;
using System.Threading.Tasks;
using Marriott.Business.Billing;
using Marriott.Business.Billing.Data;
using Marriott.Business.Pricing.Data;
using Marriott.External.Events;
using Marriott.ITOps.PaymentGateway.Commands;
using NServiceBus;

namespace Marriott.ITOps.Endpoint.PaymentGateway.Read.Handlers
{
    public class GuestCheckedOutHandler : IHandleMessages<GuestCheckedOut>
    {
        public async Task Handle(GuestCheckedOut message, IMessageHandlerContext context)
        {
            PaymentInformation paymentInformation;
            CreditCardHold creditCardHold;
            using (var billingContext = new BillingContext())
            {
                paymentInformation = await billingContext.PaymentInformation.SingleAsync(x => x.ReservationId == message.ReservationId);
                creditCardHold = await billingContext.CreditCardHold.SingleAsync(x => x.ReservationdId== message.ReservationId);
            }

            double roomTotal;
            using (var pricingContext = new PricingContext())
            {
                var reservationRoomRate = await pricingContext.ReservationRoomRate.SingleAsync(x => x.ReservationId == message.ReservationId);
                roomTotal = reservationRoomRate.TotalRoomRate; 
            }

            await context.Send(new ChargeCreditCardForTotalStay
            {
                CreditCardNumber = paymentInformation.CreditCardNumber,
                CreditCardExpiryMonth = paymentInformation.CreditCardExpiryMonth,
                CreditCardExpiryYear = paymentInformation.CreditCardExpiryYear,
                CreditCardCcv = paymentInformation.CreditCardCcv,
                RoomTotal = roomTotal
            });

            await context.Send(new ReleaseHoldOnCreditCard
            {
                CreditCardNumber = paymentInformation.CreditCardNumber,
                CreditCardExpiryMonth = paymentInformation.CreditCardExpiryMonth,
                CreditCardExpiryYear = paymentInformation.CreditCardExpiryYear,
                CreditCardCcv = paymentInformation.CreditCardCcv,
                HoldAmount = creditCardHold.HoldAmount
            });
        }
    }
}
