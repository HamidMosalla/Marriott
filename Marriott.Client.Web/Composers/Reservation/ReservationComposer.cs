using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Marriott.Business.Marketing;
using Marriott.Business.Marketing.Data;
using Marriott.Business.Pricing;
using Marriott.Business.Pricing.Data;
using Marriott.Business.Reservation.Data;
using Marriott.Business.RoomInventory.Data;
using Marriott.Client.Web.Models.Reservation;

namespace Marriott.Client.Web.Composers.Reservation
{
    public class ReservationComposer
    {
        //TODO: this is a cheat for now until I can write a better scoped "check one more time for room availability for current serach criteria before creating a Reservation"
        public async Task<bool> IsRoomStillAvailableFor(DateTime checkIn, DateTime checkOut, int roomTypeId)
        {
            var search = await ComposeSearch(new Search { CheckIn = checkIn, CheckOut = checkOut });
            var resultsByRoomTypeId = search.Results.SingleOrDefault(x => x.RoomTypeId == roomTypeId);
            return resultsByRoomTypeId != null;
        }

        public async Task<Search> ComposeSearch(Search model)
        {
            List<RoomModel> roomModels;

            using (var roomInventoryContext = new RoomInventoryContext())
            {
                roomModels = await roomInventoryContext.RoomTypes.Select(x => new RoomModel { RoomTypeId = x.Id }).ToListAsync();
            }

            using (var reservationContext = new ReservationContext())
            {
                roomModels = await AssignAvailableRoomCountToRoomModels(reservationContext, model.CheckIn, model.CheckOut, roomModels);
            }

            using (var marketingContext = new MarketingContext())
            {
                foreach (var roomModel in roomModels)
                {
                    roomModel.RoomTypes.AddRange(await marketingContext.RoomTypes.Where(x => x.RoomTypeId == roomModel.RoomTypeId).ToListAsync());
                }
            }

            using (var pricingContext = new PricingContext())
            {
                foreach (var roomModel in roomModels)
                {
                    roomModel.Rooms.AddRange(await pricingContext.RoomRate.Where(x => x.RoomTypeId == roomModel.RoomTypeId).ToListAsync());
                }
            }

            using (var roomIventoryContext = new RoomInventoryContext())
            {
                foreach (var roomModel in roomModels)
                {
                    if (roomModel.AvailableRoomCount> 0)
                    {
                        model.Results.AddRange(ComposeSearchResult(roomIventoryContext, roomModel.RoomTypes, roomModel.Rooms, (model.CheckOut.Date - model.CheckIn.Date).TotalDays - 1));
                    }
                }
            }

            return model;
        }

        //passing in RoomRate and TotalRoomRate here so my composer class (event though it's hosted in the web project) does not have to take a dependency on Session
        public async Task<RoomInformation> ComposeRoomInformationFor(int roomTypeId, double roomRate, double totalRoomRate)
        {
            var model = new RoomInformation();

            using (var context = new RoomInventoryContext())
            {
                var roomType = await context.RoomTypes.SingleAsync(x => x.Id == roomTypeId);
                model.RoomTypeName = roomType.Name;
            }
            model.RoomRate = roomRate;
            model.TotalRoomRate = totalRoomRate;

            return model;
        }

        private static async Task<List<RoomModel>> AssignAvailableRoomCountToRoomModels(ReservationContext reservationContext, DateTime checkIn, DateTime checkOut, List<RoomModel> compositeRoomModels)
        {
            foreach (var compositeRoomModel in compositeRoomModels)
            {
                var confirmedReservationCount = await reservationContext.ConfirmedReservations.Where(x => x.RoomTypeId == compositeRoomModel.RoomTypeId && (checkIn >= x.CheckIn && checkIn <= x.CheckOut || checkOut >= x.CheckIn && checkOut <= x.CheckOut)).CountAsync();
                var pendingReservationCount = await reservationContext.PendingReservations.Where(x => x.RoomTypeId == compositeRoomModel.RoomTypeId && (checkIn >= x.CheckIn && checkIn <= x.CheckOut || checkOut >= x.CheckIn && checkOut <= x.CheckOut)).CountAsync();
                compositeRoomModel.AvailableRoomCount = 5 - (confirmedReservationCount + pendingReservationCount);
            }

            return compositeRoomModels;
        }

        private static IEnumerable<SearchResult> ComposeSearchResult(RoomInventoryContext roomInventoryContext, IEnumerable<RoomType> roomTypes, IEnumerable<RoomRate> roomRates, double totalNights)
        {
            return roomInventoryContext.RoomTypes.ToList()
                .Join(roomRates, rt => rt.Id, rr => rr.RoomTypeId, (rt, rr) => new { rt.Id, rt.Name, rr.Rate, rr.RoomTypeId })
                .Join(roomTypes, arg => arg.RoomTypeId, rt => rt.RoomTypeId, (arg, rt) => 
                new SearchResult
                {
                    //RoomInventory
                    RoomTypeId = arg.Id,
                    RoomTypeName = arg.Name,
                    //Marketing
                    ImageUrl = rt.ImageUrl,
                    RoomDescription = rt.Description,
                    //Pricing
                    RoomRate = arg.Rate,
                    TotalRoomRate = totalNights * arg.Rate
                });
        }

        private class RoomModel
        {
            public int RoomTypeId { get; set; }
            public int AvailableRoomCount { get; set; }
            public List<RoomType> RoomTypes { get; set; } = new List<RoomType>();
            public List<RoomRate> Rooms { get; set; } = new List<RoomRate>();
        }
    }
}