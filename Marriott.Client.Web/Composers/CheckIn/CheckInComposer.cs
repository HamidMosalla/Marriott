using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Marriott.Business.Billing;
using Marriott.Business.Billing.Data;
using Marriott.Business.Guest;
using Marriott.Business.Guest.Data;
using Marriott.Business.Marketing.Data;
using Marriott.Business.Pricing.Data;
using Marriott.Client.Web.Models.CheckIn;
using Marriott.Business.Reservation.Data;
using Marriott.Business.RoomInventory.Data;
using RoomType = Marriott.Business.Marketing.RoomType;

namespace Marriott.Client.Web.Composers.CheckIn
{
    public class CheckInComposer
    {
        public async Task<Search> ComposeSearchResults(Search model)
        {
            List<GuestInformation> guestInformation;
            using (var guestContext = new GuestContext())
            {
                guestInformation = await guestContext.GuestInformation.Where(x => x.Email == model.Email).ToListAsync();
            }

            List<RoomType> roomTypes;
            using (var marketingContext = new MarketingContext())
            {
                roomTypes = await marketingContext.RoomTypes.ToListAsync();
            }

            using (var reservationContext = new ReservationContext())
            {
                if (!string.IsNullOrEmpty(model.Email))
                {
                    var results = reservationContext.ConfirmedReservations.AsEnumerable()
                        .Join(guestInformation, r => r.ReservationId, gi => gi.ReservationId, (r, gi) => new { r, gi })
                        .Join(roomTypes, arg => arg.r.RoomTypeId, rt => rt.RoomTypeId, (arg, rt) => 
                        new SearchResult
                        {
                            ReservationId = arg.r.ReservationId,
                            ExternalId = arg.r.ExternalId,
                            FullName = arg.gi.FullName,
                            RoomType = rt.Description,
                            CheckIn = arg.r.CheckIn,
                            CheckOut = arg.r.CheckOut
                        })
                        .OrderByDescending(x => x.CheckIn)
                        .ToList();

                    model.Results = results;
                }
            }

            return model;
        }

        public ReservationInformation ComposeReservationInformation(Guid reservationId)
        {
            var model = new ReservationInformation();

            Business.Reservation.ConfirmedReservation reservation;
            using (var reservationContext = new ReservationContext())
            {
                reservation = reservationContext.ConfirmedReservations.Single(x => x.ReservationId == reservationId);

                model.ExternalId = reservation.ExternalId;
                model.CheckIn = reservation.CheckIn;
                model.CheckOut = reservation.CheckOut;
                model.TotalNightsOfStay = reservation.TotalNightsOfStay;
            }

            using (var pricingContext = new PricingContext())
            {
                var reservationRoomRate = pricingContext.ReservationRoomRate.Single(x => x.ReservationId == reservationId);
                model.RoomRate = reservationRoomRate.RoomRate;
                model.TotalRoomRate = reservationRoomRate.TotalRoomRate;
            }

            using (var roomInventoryContext = new RoomInventoryContext())
            {
                var roomType = roomInventoryContext.RoomTypes.Single(x => x.Id == reservation.RoomTypeId);
                model.RoomTypeName = roomType.Name;
            }

            return model;
        }

        public GuestInformation ComposeGuestInformation(Guid reservationId)
        {
            using (var guestContext = new GuestContext())
            {
                return guestContext.GuestInformation.Single(x => x.ReservationId == reservationId);
            }
        }

        public async Task<VerifyGuestInformation> ComposeVerifyGuestInformation(Guid reservationId)
        {
            var model = new VerifyGuestInformation { ReservationId = reservationId };

            using (var reservationContext = new ReservationContext())
            {
                model.Reservation = await reservationContext.ConfirmedReservations.SingleAsync(x => x.ReservationId == reservationId);
            }

            using (var pricingContext = new PricingContext())
            {
                var reservationRoomRate = await pricingContext.ReservationRoomRate.SingleAsync(x => x.ReservationId == reservationId);
                model.RoomRate = reservationRoomRate.RoomRate;
                model.TotalRoomRate = reservationRoomRate.TotalRoomRate;
            }

            using (var roomInventoryContext = new RoomInventoryContext())
            {
                var roomType = await roomInventoryContext.RoomTypes.SingleAsync(x => x.Id == model.Reservation.RoomTypeId);
                model.RoomTypeName = roomType.Name;
            }

            return model;
        }

        public async Task<PaymentInformation> ComposePaymentInformation(Guid reservationId)
        {
            using (var billingContext = new BillingContext())
            {
                return await billingContext.PaymentInformation.SingleAsync(x => x.ReservationId == reservationId);
            }
        }

        public async Task<RoomAllocationResult> RoomAllocationResult (Guid reservationId)
        {
            using (var roomInventoryContext = new RoomInventoryContext())
            {
                var allocatedRoom = await roomInventoryContext.AllocatedRooms.SingleOrDefaultAsync(x => x.ReservationId == reservationId);
                if (allocatedRoom != null)
                {
                    return new RoomAllocationResult { RoomNumber = allocatedRoom.RoomNumber, Result = Result.Succeeded };
                }

                if (await roomInventoryContext.FailedRoomAllocations.AnyAsync(x => x.ReservationId == reservationId))
                {
                    return new RoomAllocationResult { Result = Result.Failed };
                }

                return new RoomAllocationResult { Result = Result.InProgress };
            }
        }
    }
}