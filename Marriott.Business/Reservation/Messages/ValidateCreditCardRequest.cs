namespace Marriott.Business.Reservation.Messages
{
    public class ValidateCreditCardRequest
    {
        public string ClientId { get; set; }
        public int CreditCardNumber { get; set; }
        public int CreditCardExpiryMonth { get; set; }
        public int CreditCardExpiryYear { get; set; }
        public int CreditCardCcv { get; set; }
    }
}
