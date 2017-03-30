using System.Threading.Tasks;
using Marriott.Business.Guest;

namespace Marriott.ITOps.PaymentGateway
{
    public interface ICreditCardService
    {
        Task<bool> Validate(int creditCardNumber, int creditCardExpiryMonth, int creditCardExpiryYear, int creditCardCcv);
        Task<bool> Charge(int creditCardNumber, int creditCardExpiryMonth, int creditCardExpiryYear, int creditCardCcv, double amount);
        Task<bool> Charge(int creditCardNumber, int creditCardExpiryMonth, int creditCardExpiryYear, int creditCardCcv, double amount, GuestInformation guestInformation);
        Task<bool> ReleaseHoldOn(int creditCardNumber, int creditCardExpiryMonth, int creditCardExpiryYear, int creditCardCcv, double holdAmount);
    }
}
