using System.Threading.Tasks;
using Marriott.Business.Billing.Messages;
using Marriott.Client.Web.Hubs;
using Microsoft.AspNet.SignalR;
using NServiceBus;

namespace Marriott.Client.Web.Handlers
{
    public class PlaceHoldOnCreditCardResponseHandler : IHandleMessages<PlaceHoldOnCreditCardResponse>
    {
        public Task Handle(PlaceHoldOnCreditCardResponse message, IMessageHandlerContext context)
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<PlaceHoldOnCreditCardHub>();
            hubContext.Clients.Client(message.ClientId).placeHoldOnCreditCardResult(message.Succeeded);
            return Task.CompletedTask;
        }
    }
}