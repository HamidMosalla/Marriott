using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Marriott.Business.RoomInventory;
using Marriott.Business.RoomInventory.Commands;
using Marriott.Business.RoomInventory.Data;
using Marriott.External.Events;
using NServiceBus;

namespace Marriott.Business.Endpoint.RoomInventory.Handlers
{
    public class AllocateRoomHandler : IHandleMessages<AllocateRoom>
    {
        public async Task Handle(AllocateRoom message, IMessageHandlerContext context)
        {
            using (var roomInventoryContext = new RoomInventoryContext())
            {
                var roomsOfRoomTypeId = await roomInventoryContext.RoomNumbers.Where(x => x.RoomTypeId == message.RoomTypeId).ToListAsync();
                var roomNumbersOfRoomTypeId = roomsOfRoomTypeId.Select(x => x.Number).ToList();

                //currently allocated rooms of room type where today is in between or equal to CheckIn/CheckOut
                var allocatedRoomNumbersOfRoomTypeId = await roomInventoryContext.AllocatedRooms.Where(x => roomNumbersOfRoomTypeId.Contains(x.RoomNumber) && x.Deallocated == false)
                    .Select(y => y.RoomNumber).ToListAsync();

                //filter out allocated rooms from the list of all rooms in inventory by the room type
                var availableRoomNumbers = roomsOfRoomTypeId.Select(x => x.Number).Except(allocatedRoomNumbersOfRoomTypeId);

                //return first available room from the list of unallocated rooms
                var room = roomsOfRoomTypeId.FirstOrDefault(x => availableRoomNumbers.Contains(x.Number));

                if (room != null)
                {
                    roomInventoryContext.AllocatedRooms.Add(new AllocatedRoom { ReservationId = message.ReservationId, RoomNumber = room.Number, CheckInDate = message.CheckIn, CheckOutDate = message.CheckOut });
                }
                else
                {
                    roomInventoryContext.FailedRoomAllocations.Add(new FailedRoomAllocation { ReservationId = message.ReservationId });
                    await context.Publish(new RoomAllocationFailed { ReservationId = message.ReservationId });
                }

                await roomInventoryContext.SaveChangesAsync();
            }
        }
    }
}
