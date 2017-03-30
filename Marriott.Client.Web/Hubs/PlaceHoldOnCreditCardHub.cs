using System;
using System.Linq;
using Marriott.Business.Billing.Messages;
using Marriott.Business.Guest;
using Marriott.Business.Guest.Data;
using Marriott.Business.Reservation.Data;
using Microsoft.AspNet.SignalR;
using NServiceBus;
using Marriott.Business.Pricing;
using Marriott.Business.Pricing.Data;

namespace Marriott.Client.Web.Hubs
{
    public class PlaceHoldOnCreditCardHub : Hub
    {
        public void PlaceHoldOnCreditCard(Guid reservationId, int creditCardNumber, int creditCardExpiryMonth, int creditCardExpiryYear, int creditCardCcv)
        {
            //this hub is taking the place of a controller/composer call in order to dispatch a message to get something done

            GuestInformation guestInformation;
            using (var guestContext = new GuestContext())
            {
                guestInformation = guestContext.GuestInformation.Single(x => x.ReservationId == reservationId);
            }

            Business.Reservation.ConfirmedReservation reservation;
            using (var reservationContext = new ReservationContext())
            {
                reservation = reservationContext.ConfirmedReservations.Single(x => x.ReservationId == reservationId);
            }

            ReservationRoomRate reservationRoomRate;
            using (var pricingContext = new PricingContext())
            {
                reservationRoomRate = pricingContext.ReservationRoomRate.Single(x => x.ReservationId == reservationId);
            }

            //http://www.moneytalksnews.com/heres-why-hotels-put-that-mysterious-hold-your-credit-card/
            //"Most hotels place a hold on your credit card, according to Dale Blosser, a lodging consultant. The amount varies, but as a rule, it’s the cost of the room, including tax, plus a set charge of between $50 and $200 per day."
            //"“This charge happened at check-in when your card was first swiped and our system automatically authorizes for possible incidental charges,” Bush wrote in an email. “Our system will continue to check that the card has enough funds as you add charges to your room.”"
            var holdAmount = reservationRoomRate.TotalRoomRate + (reservation.TotalNightsOfStay * 50.00);

            MvcApplication.Endpoint.Send(new PlaceHoldOnCreditCardRequest
            {
                ClientId = Context.ConnectionId,
                ReservationId = reservationId,
                CreditCardNumber = creditCardNumber,
                CreditCardExpiryMonth = creditCardExpiryMonth,
                CreditCardExpiryYear = creditCardExpiryMonth,
                CreditCardCcv = creditCardCcv,
                HoldAmount = holdAmount,
                GuestInformation = guestInformation
            });
        }
    }
}