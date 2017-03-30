using System;
using System.Threading.Tasks;
using Marriott.Business.Housekeeping;
using Marriott.Business.Housekeeping.Data;
using Marriott.External.Events;
using NServiceBus;

namespace Marriott.Business.Endpoint.Housekeeping.Handlers
{
    public class GuestCheckedOutHandler : IHandleMessages<GuestCheckedOut>
    {
        public async Task Handle(GuestCheckedOut message, IMessageHandlerContext context)
        {
            if (message.CheckedOutDate.Date == DateTime.Today.Date)
            {
                using (var housekeepingContext = new HousekeepingContext())
                {
                    housekeepingContext.TodaysCheckedOutRooms.Add(new TodaysCheckedOutRooms { RoomNumber = message.RoomNumber, CheckOutDate = message.CheckedOutDate.Date });
                    await housekeepingContext.SaveChangesAsync();
                }
            }
        }
    }
}
