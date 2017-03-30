using System;
using System.Data.Entity;
using System.Threading.Tasks;
using Marriott.Business.Billing.Data;
using Marriott.Business.Guest;
using Marriott.Business.Guest.Data;
using Marriott.Business.Pricing.Data;
using Marriott.Business.RoomInventory.Data;
using Marriott.Client.Web.Models.CheckOut;
using Marriott.Business.RoomInventory;

namespace Marriott.Client.Web.Composers.CheckOut
{
    public class CheckOutComposer
    {
        public async Task<Search> ComposeSearchResults(Search model)
        {
            AllocatedRoom allocatedRoom;
            using (var roomInventoryContext = new RoomInventoryContext())
            {
                var dateTimeNow = DateTime.Now;
                //get me the AllocatedRoom by the RoomNumber based on who should be checked into the room right now
                allocatedRoom = await roomInventoryContext.AllocatedRooms.SingleOrDefaultAsync(x => x.RoomNumber == model.RoomNumber && 
                    x.CheckInDate <= dateTimeNow && 
                    x.CheckOutDate >= dateTimeNow && 
                    x.Deallocated == false);
            }

            if (allocatedRoom == null)
            {
                return model;
            }

            GuestInformation guestInformation;
            using (var guestContext = new GuestContext())
            {
                guestInformation = await guestContext.GuestInformation.SingleAsync(x => x.ReservationId == allocatedRoom.ReservationId);
            }

            model.Result = new SearchResult
            {
                ReservationId = allocatedRoom.ReservationId,
                CheckIn = allocatedRoom.CheckInDate,
                CheckOut = allocatedRoom.CheckOutDate,
                GuestFullName = guestInformation.FullName
            };

            return model;
        }

        public async Task<VerifyInvoice> ComposeVerifyInvoice(Guid reservationId)
        {
            const double roomTaxRate = .08;
            var model = new VerifyInvoice { ReservationId = reservationId };

            using (var roomInventoryContext = new RoomInventoryContext())
            {
                var allocatedRoom = await roomInventoryContext.AllocatedRooms.SingleAsync(x => x.ReservationId == reservationId);
                model.CheckIn = allocatedRoom.CheckInDate;
                model.CheckOut = allocatedRoom.CheckOutDate;
                model.RoomNumber = allocatedRoom.RoomNumber;

                var roomNumber = await roomInventoryContext.RoomNumbers.SingleAsync(x => x.Number == allocatedRoom.RoomNumber);
                var roomType = await roomInventoryContext.RoomTypes.SingleAsync(x => x.Id == roomNumber.RoomTypeId);
                model.RoomType = roomType.Name;
            }

            using (var guestContext = new GuestContext())
            {
                model.GuestInformation = await guestContext.GuestInformation.SingleAsync(x => x.ReservationId == reservationId);
            }

            using (var billingContext = new BillingContext())
            {
                model.PaymentInformation = await billingContext.PaymentInformation.SingleAsync(x => x.ReservationId == reservationId);
            }

            using (var pricingContext = new PricingContext())
            {
                var reservationRoomRate = await pricingContext.ReservationRoomRate.SingleAsync(x => x.ReservationId == reservationId);
                model.RoomRate = reservationRoomRate.RoomRate;
                model.TotalRoomRate = reservationRoomRate.TotalRoomRate;
            }

            model.Tax = model.TotalRoomRate * roomTaxRate;
            model.Total = model.TotalRoomRate + model.Tax;

            return model;
        }
    }

    //https://martinfowler.com/eaaDev/Range.html
    //http://stackoverflow.com/questions/4781611/how-to-know-if-a-datetime-is-between-a-daterange-in-c-sharp
    //none of this is usable b/c entity framework cannot parse/execute this logic as part of the LINQ to entities statement. Kind of sucks
    //allocatedRoom = await roomInventoryContext.AllocatedRooms.SingleOrDefaultAsync(x => x.RoomNumber == model.RoomNumber && x.CheckIn.BeforeOrEqualTo(dateTimeNow) && x.CheckOut.AfterOrEqualTo(dateTimeNow));
    //public static class DateTimeExtensions
    //{
    //    public static bool BeforeOrEqualTo(this DateTime thisDateTime, DateTime dateTime)
    //    {
    //        return thisDateTime <= dateTime;
    //    }

    //    public static bool AfterOrEqualTo(this DateTime thisDateTime, DateTime dateTime)
    //    {
    //        return thisDateTime >= dateTime;
    //    }

    //    public static bool InBetween(this DateTime thisDateTime, DateTime startDateTime, DateTime endDateTime)
    //    {
    //        return false;
    //    }
    //}
}