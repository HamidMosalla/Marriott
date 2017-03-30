using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Marriott.Business.Housekeeping.Commands;
using Marriott.Business.Housekeeping.Data;
using NServiceBus;

namespace Marriott.Business.Endpoint.Housekeeping.Handlers
{
    public class UpdateTodaysCleanRoomsHandler : IHandleMessages<UpdateTodaysCleanRooms>
    {
        public async Task Handle(UpdateTodaysCleanRooms message, IMessageHandlerContext context)
        {
            using (var housekeepingContext = new HousekeepingContext())
            {
                var dateTimeNowDate = DateTime.Now.Date;
                var todaysCleanRooms = await housekeepingContext.TodaysCleanRooms.Where(x => x.DayCleaned == dateTimeNowDate).ToListAsync();

                //this is a very non-collborative approach. Since I lack a domain expert, I'm not too sure if every housekeeper would need to update the rooms they're cleaning via a 
                //handheld device/phone/etc (aka, a very collaborative approach). If that's the case, this would be a very bad approach as it uses optimisic concurrency and changes 
                //could be lost when two housekeepers go to update the rooms they're cleaning at the same time.
                //Although two housekeepers would never be cleaning the same room at the the same time (and if there are two physical housekeepers cleaning, only one of them would need to 
                //update the room as "clean", b/c this approach blows ALL the rooms away then re-creates them, it's still a dangerous operation for more than one person to have access to.
                housekeepingContext.TodaysCleanRooms.RemoveRange(todaysCleanRooms);
                housekeepingContext.TodaysCleanRooms.AddRange(message.TodaysCleanRooms);

                await housekeepingContext.SaveChangesAsync();
            }
        }
    }
}
