using System.Data.Entity;
using System.Threading.Tasks;
using Marriott.Business.RoomInventory.Commands;
using Marriott.Business.RoomInventory.Data;
using Marriott.External.Events;
using NServiceBus;

namespace Marriott.Business.Endpoint.RoomInventory.Handlers
{
    public class DeallocateRoomForNonPaymentAtCheckInHandler : IHandleMessages<DeallocateRoomForNonPaymentAtCheckIn>
    {
        public async Task Handle(DeallocateRoomForNonPaymentAtCheckIn message, IMessageHandlerContext context)
        {
            using (var roomInventoryContext = new RoomInventoryContext())
            {
                //TODO: why am I using SingleOrDefault here?
                var allocatedRoom = await roomInventoryContext.AllocatedRooms.SingleOrDefaultAsync(x => x.ReservationId == message.ReservationId);
                if (allocatedRoom != null)
                {
                    roomInventoryContext.AllocatedRooms.Remove(allocatedRoom);
                    await roomInventoryContext.SaveChangesAsync();
                }

                await context.Publish(new RoomDeallocatedForNonPaymentAtCheckIn { ReservationId = message.ReservationId });
            }
        }
    }
}