using System.Threading.Tasks;

namespace Marriott.CreditCard
{
    public class CreditCardService : ICreditCardService
    {
        public Task<bool> Validate(int creditCardNumber, int creditCardExpiryMonth, int creditCardExpiryYear, int creditCardCcv)
        {
            return Task.FromResult(creditCardNumber != 0);
        }

        public Task<bool> PlaceHoldOn(int creditCardNumber, int creditCardExpiryMonth, int creditCardExpiryYear, int creditCardCcv, double holdAmount)
        {
            //http://www.moneytalksnews.com/heres-why-hotels-put-that-mysterious-hold-your-credit-card/
            //"Most hotels place a hold on your credit card, according to Dale Blosser, a lodging consultant. The amount varies, but as a rule, it’s the cost of the room, including tax, plus a set charge of between $50 and $200 per day."
            //this might be too much to model at this point. "as you keep adding charges to your room", does that mean each night gets billed separately, or is this for things like room service, ni-room tv rentals, dry clearning, etc...
            //"“This charge happened at check-in when your card was first swiped and our system automatically authorizes for possible incidental charges,” Bush wrote in an email. “Our system will continue to check that the card has enough funds as you add charges to your room.”"
            return Task.FromResult(true);
        }

        public Task<bool> Charge(int creditCardNumber, int creditCardExpiryMonth, int creditCardExpiryYear, int creditCardCcv, double amount)
        {
            return Task.FromResult(true);
        }
    }
}