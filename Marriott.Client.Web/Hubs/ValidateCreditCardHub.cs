using Marriott.Business.Reservation.Messages;
using Microsoft.AspNet.SignalR;
using NServiceBus;

namespace Marriott.Client.Web.Hubs
{
    public class ValidateCreditCardHub : Hub
    {
        public void Validate(int creditCardNumber, int creditCardExpiryMonth, int creditCardExpiryYear, int creditCardCcv)
        {
            MvcApplication.Endpoint.Send(new ValidateCreditCardRequest
            {
                ClientId = Context.ConnectionId,
                CreditCardNumber = creditCardNumber,
                CreditCardExpiryMonth = creditCardExpiryMonth,
                CreditCardExpiryYear = creditCardExpiryMonth,
                CreditCardCcv = creditCardCcv
            });
        }
    }
}