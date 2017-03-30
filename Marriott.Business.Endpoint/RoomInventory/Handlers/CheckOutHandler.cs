using System.Data.Entity;
using System.Threading.Tasks;
using Marriott.Business.RoomInventory;
using Marriott.Business.RoomInventory.Commands;
using Marriott.Business.RoomInventory.Data;
using Marriott.External.Events;
using NServiceBus;

namespace Marriott.Business.Endpoint.RoomInventory.Handlers
{
    public class CheckOutHandler : IHandleMessages<CheckOut>
    {
        public async Task Handle(CheckOut message, IMessageHandlerContext context)
        {
            AllocatedRoom allocatedRoom;
            using (var roomInventoryContext = new RoomInventoryContext())
            {
                allocatedRoom = await roomInventoryContext.AllocatedRooms.SingleAsync(x => x.ReservationId == message.ReservationId);
                allocatedRoom.Deallocate();
                await roomInventoryContext.SaveChangesAsync();
            }

            await context.Publish(new GuestCheckedOut { ReservationId = message.ReservationId, RoomNumber = allocatedRoom.RoomNumber, CheckedOutDate = message.CheckOutDate });
        }
    }
}