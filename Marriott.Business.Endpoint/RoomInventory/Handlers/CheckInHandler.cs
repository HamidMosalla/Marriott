using System.Data.Entity;
using System.Threading.Tasks;
using Marriott.Business.RoomInventory;
using Marriott.Business.RoomInventory.Commands;
using Marriott.Business.RoomInventory.Data;
using Marriott.External.Events;
using NServiceBus;

namespace Marriott.Business.Endpoint.RoomInventory.Handlers
{
    public class CheckInHandler : IHandleMessages<CheckIn>
    {
        public async Task Handle(CheckIn message, IMessageHandlerContext context)
        {
            AllocatedRoom allocatedRoom;
            using (var roomInventoryContext = new RoomInventoryContext())
            {
                allocatedRoom = await roomInventoryContext.AllocatedRooms.SingleAsync(x => x.ReservationId == message.ReservationId);
            }

            await context.Publish(new GuestCheckedIn
            {
                ReservationId = message.ReservationId,
                CheckedInDate = allocatedRoom.CheckInDate,
                CheckOutDate = allocatedRoom.CheckOutDate
            });
        }
    }
}
