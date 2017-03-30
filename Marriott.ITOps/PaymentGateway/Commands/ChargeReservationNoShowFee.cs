namespace Marriott.ITOps.PaymentGateway.Commands
{
    public class ChargeReservationNoShowFee
    {
        public int CreditCardNumber { get; set; }
        public int CreditCardExpiryMonth { get; set; }
        public int CreditCardExpiryYear { get; set; }
        public int CreditCardCcv { get; set; }
        public double NoShowFee { get; set; }
    }
}
