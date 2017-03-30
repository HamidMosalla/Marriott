using System.Data.Entity.Migrations;
using System.Threading.Tasks;
using Marriott.Business.Marketing;
using Marriott.Business.Marketing.Commands;
using Marriott.Business.Marketing.Data;
using NServiceBus;

namespace Marriott.Business.Endpoint.Marketing.Handlers
{
    public class SaveRoomTypeHandler : IHandleMessages<SaveRoomType>
    {
        public async Task Handle(SaveRoomType message, IMessageHandlerContext context)
        {
            using (var marketingContext = new MarketingContext())
            {
                marketingContext.Set<RoomType>().AddOrUpdate(message.RoomType);
                await marketingContext.SaveChangesAsync();
            }
        }
    }
}