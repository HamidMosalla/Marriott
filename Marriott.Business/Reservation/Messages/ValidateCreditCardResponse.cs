namespace Marriott.Business.Reservation.Messages
{
    public class ValidateCreditCardResponse
    {
        public string ClientId { get; set; }
        public bool Succeeded { get; set; }
    }
}
