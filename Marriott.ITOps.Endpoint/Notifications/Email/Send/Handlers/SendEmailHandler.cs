using System.Threading.Tasks;
using Marriott.ITOps.Notifications.Email;
using Marriott.ITOps.Notifications.Email.Commands;
using NServiceBus;

namespace Marriott.ITOps.Endpoint.Notifications.Email.Send.Handlers
{
    public class SendEmailHandler : IHandleMessages<SendEmail>
    {
        private readonly ISendEmail emailSender;

        public SendEmailHandler(ISendEmail emailSender)
        {
            this.emailSender = emailSender;
        }

        public async Task Handle(SendEmail message, IMessageHandlerContext context)
        {
            await emailSender.SendMail(message.Email);
        }
    }
}
