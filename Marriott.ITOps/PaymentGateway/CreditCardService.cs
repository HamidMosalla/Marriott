using System.Threading.Tasks;
using Marriott.Business.Guest;

namespace Marriott.ITOps.PaymentGateway
{
    public class CreditCardService : ICreditCardService
    {
        public Task<bool> Validate(int creditCardNumber, int creditCardExpiryMonth, int creditCardExpiryYear, int creditCardCcv)
        {
            return ReturnTrueIfCreditCardNumberEqualsOneAndFalseIfNot(creditCardNumber);
        }

        public Task<bool> Charge(int creditCardNumber, int creditCardExpiryMonth, int creditCardExpiryYear, int creditCardCcv, double amount)
        {
            return ReturnTrueIfCreditCardNumberEqualsOneAndFalseIfNot(creditCardNumber);
        }

        public Task<bool> Charge(int creditCardNumber, int creditCardExpiryMonth, int creditCardExpiryYear, int creditCardCcv, double amount, GuestInformation guestInformation)
        {
            return ReturnTrueIfCreditCardNumberEqualsOneAndFalseIfNot(creditCardNumber);
        }

        public Task<bool> ReleaseHoldOn(int creditCardNumber, int creditCardExpiryMonth, int creditCardExpiryYear, int creditCardCcv, double holdAmount)
        {
            return ReturnTrueIfCreditCardNumberEqualsOneAndFalseIfNot(creditCardNumber);
        }

        private static Task<bool> ReturnTrueIfCreditCardNumberEqualsOneAndFalseIfNot(int creditCardNumber)
        {
            return Task.FromResult(creditCardNumber == 1);
        }
    }
}
