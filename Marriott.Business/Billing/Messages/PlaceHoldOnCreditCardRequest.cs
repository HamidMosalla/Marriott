using System;
using Marriott.Business.Guest;

namespace Marriott.Business.Billing.Messages
{
    public class PlaceHoldOnCreditCardRequest
    {
        public string ClientId { get; set; }
        public Guid ReservationId { get; set; }
        public int CreditCardNumber { get; set; }
        public int CreditCardExpiryMonth { get; set; }
        public int CreditCardExpiryYear { get; set; }
        public int CreditCardCcv { get; set; }
        public GuestInformation GuestInformation { get; set; }
        public double HoldAmount { get; set; }
    }
}