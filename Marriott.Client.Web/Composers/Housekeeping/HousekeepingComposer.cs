using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Marriott.Business.Housekeeping;
using Marriott.Business.Housekeeping.Data;
using Marriott.Business.RoomInventory;
using Marriott.Business.RoomInventory.Data;
using Marriott.Client.Web.Models.Housekeeping;

namespace Marriott.Client.Web.Composers.Housekeeping
{
    public class HousekeepingComposer
    {
        public async Task<Index> ComposeRoomsThatNeedToBeCleanedToday()
        {
            var dateTimeNowDate = DateTime.Now.Date;

            var model = new Index();

            List<TodaysCheckedOutRooms> todaysCheckedOutRooms;
            List<TodaysCleanRooms> cleanRooms;
            using (var housekeepingContext = new HousekeepingContext())
            {
                todaysCheckedOutRooms = await housekeepingContext.TodaysCheckedOutRooms.Where(x => x.CheckOutDate == dateTimeNowDate).ToListAsync();
                cleanRooms = await housekeepingContext.TodaysCleanRooms.Where(x => x.DayCleaned == dateTimeNowDate).ToListAsync();
            }

            List<RoomNumber> rooms;
            using (var roomInventoryContext = new RoomInventoryContext())
            {
                rooms = await roomInventoryContext.RoomNumbers.ToListAsync();
            }
            
            //var result1 = rooms
            //    .GroupJoin(cleanRooms, rn => rn.Number, cr => cr.RoomNumber, (rn, cr) => new { rn = rn, cr = cr.SingleOrDefault() });
            //var resultTransformingOutputToListOfRooms = rooms
            //    .GroupJoin(cleanRooms, rn => rn.Number, cr => cr.RoomNumber, (rn, cr) => new Room { RoomNumber = rn.Number, Clean = cr.Any()} );
            //model.Rooms.AddRange(rooms.GroupJoin(cleanRooms, rn => rn.Number, cr => cr.RoomNumber, (rn, cr) => new Room { RoomNumber = rn.Number, Clean = cr.Any() }));

            //var results = rooms
            //    .GroupJoin(cleanRooms, rn => rn.Number, cr => cr.RoomNumber, (rn, cr) => new { rn, cr })
            //    .GroupJoin(todaysCheckedOutRooms, arg => arg.rn.Number, tcor => tcor.RoomNumber, (arg, tcor) => 
            //        new Room { RoomNumber = arg.rn.Number, Clean = arg.cr.Any(), DateTimeCheckedOut = tcor.Any() ? tcor.Single().DateTimeCheckedOut: DateTime.MinValue })
            //    .OrderByDescending(x => x.DateTimeCheckedOut);

            var results = rooms
                .GroupJoin(cleanRooms, rn => rn.Number, cr => cr.RoomNumber, (rn, cr) => new { rn, cr })
                .GroupJoin(todaysCheckedOutRooms, arg => arg.rn.Number, tcor => tcor.RoomNumber, (arg, tcor) =>
                new Room { RoomNumber = arg.rn.Number, Clean = arg.cr.Any(), DateTimeCheckedOut = tcor.Any() ? tcor.Single().CheckOutDate : DateTime.MinValue })
                .OrderByDescending(x => x.DateTimeCheckedOut);

            model.Rooms.AddRange(results);

            return model;
        }
    }
}