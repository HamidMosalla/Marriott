using System.Data.Entity.Migrations;
using System.Threading.Tasks;
using Marriott.Business.Guest;
using Marriott.Business.Guest.Commands;
using Marriott.Business.Guest.Data;
using NServiceBus;

namespace Marriott.Business.Endpoint.Guest.Handlers
{
    public class SaveGuestInformationHandler : IHandleMessages<SaveGuestInformation>
    {
        public async Task Handle(SaveGuestInformation message, IMessageHandlerContext context)
        {
            using (var dbContext = new GuestContext())
            {
                dbContext.Set<GuestInformation>().AddOrUpdate(message.GuestInformation);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
