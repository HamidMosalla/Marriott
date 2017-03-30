using System.Threading.Tasks;

namespace Marriott.CreditCard
{
    public interface ICreditCardService
    {
        Task<bool> Validate(int creditCardNumber, int creditCardExpiryMonth, int creditCardExpiryYear, int creditCardCcv);
        Task<bool> PlaceHoldOn(int creditCardNumber, int creditCardExpiryMonth, int creditCardExpiryYear, int creditCardCcv, double holdAmount);
        Task<bool> Charge(int creditCardNumber, int creditCardExpiryMonth, int creditCardExpiryYear, int creditCardCcv, double amount);
    }
}
