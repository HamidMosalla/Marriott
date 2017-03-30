using System.Data.Entity.Migrations;
using System.Threading.Tasks;
using Marriott.Business.Pricing;
using Marriott.Business.Pricing.Commands;
using Marriott.Business.Pricing.Data;
using NServiceBus;

namespace Marriott.Business.Endpoint.Pricing.Handlers
{
    public class SaveReservationRoomRateHandler : IHandleMessages<SaveReservationRoomRate>
    {
        public async Task Handle(SaveReservationRoomRate message, IMessageHandlerContext context)
        {
            using (var dbContext = new PricingContext())
            {
                dbContext.Set<ReservationRoomRate>().AddOrUpdate(message.ReservationRoomRate);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
