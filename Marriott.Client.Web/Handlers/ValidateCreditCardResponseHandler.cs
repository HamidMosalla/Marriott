using System.Threading.Tasks;
using Marriott.Business.Reservation.Messages;
using Marriott.Client.Web.Hubs;
using Microsoft.AspNet.SignalR;
using NServiceBus;

namespace Marriott.Client.Web.Handlers
{
    public class ValidateCreditCardResponseHandler : IHandleMessages<ValidateCreditCardResponse>
    {
        public Task Handle(ValidateCreditCardResponse message, IMessageHandlerContext context)
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<ValidateCreditCardHub>();
            hubContext.Clients.Client(message.ClientId).validateCreditCardResult(message.Succeeded);
            return Task.CompletedTask;
        }
    }
}
