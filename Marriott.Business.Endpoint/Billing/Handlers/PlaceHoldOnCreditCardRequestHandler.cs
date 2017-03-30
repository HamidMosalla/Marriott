using System.Threading.Tasks;
using Marriott.Business.Billing.Events;
using Marriott.Business.Billing.Messages;
using Marriott.ITOps.PaymentGateway;
using NServiceBus;

namespace Marriott.Business.Endpoint.Billing.Handlers
{
    //http://www.moneytalksnews.com/heres-why-hotels-put-that-mysterious-hold-your-credit-card/
    //"Most hotels place a hold on your credit card, according to Dale Blosser, a lodging consultant. The amount varies, but as a rule, it’s the cost of the room, including tax, plus a set charge of between $50 and $200 per day."
    //"“This charge happened at check-in when your card was first swiped and our system automatically authorizes for possible incidental charges,” Bush wrote in an email. “Our system will continue to check that the card has enough funds as you add charges to your room.”"
    public class PlaceHoldOnCreditCardRequestHandler : IHandleMessages<PlaceHoldOnCreditCardRequest>
    {
        private readonly ICreditCardService creditCardService;

        public PlaceHoldOnCreditCardRequestHandler(ICreditCardService creditCardService)
        {
            this.creditCardService = creditCardService;
        }

        public async Task Handle(PlaceHoldOnCreditCardRequest message, IMessageHandlerContext context)
        {
            var placeHoldOnCreditCardResponse = new PlaceHoldOnCreditCardResponse { ClientId = message.ClientId };
            
            //simulate slow RPC
            await Task.Delay(2000);
            
            if (await creditCardService.Charge(message.CreditCardNumber, message.CreditCardExpiryMonth, message.CreditCardExpiryYear, message.CreditCardCcv, message.HoldAmount, 
                message.GuestInformation))
            {
                placeHoldOnCreditCardResponse.Succeeded = true;
                await context.Publish(new HoldOnCreditCardPlaced { ReservationId = message.ReservationId, HoldAmount = message.HoldAmount });
            }
            else
            {
                placeHoldOnCreditCardResponse.Succeeded = false;
            }

            await context.Reply(placeHoldOnCreditCardResponse);
        }
    }
}