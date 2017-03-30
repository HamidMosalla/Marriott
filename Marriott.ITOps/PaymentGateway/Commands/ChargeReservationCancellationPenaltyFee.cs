namespace Marriott.ITOps.PaymentGateway.Commands
{
    public class ChargeReservationCancellationPenaltyFee
    {
        public int CreditCardNumber { get; set; }
        public int CreditCardExpiryMonth { get; set; }
        public int CreditCardExpiryYear { get; set; }
        public int CreditCardCcv { get; set; }
        public double PenaltyFee { get; set; }
    }
}
