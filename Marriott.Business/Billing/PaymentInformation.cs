using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Marriott.Business.Billing
{
    public class PaymentInformation
    {
        [Key]
        public Guid ReservationId { get; set; }

        [Required]
        [DisplayName("Credit Card Number")]
        public int CreditCardNumber { get; set; }

        [Required]
        [DisplayName("Credit Card Expiry Month")]
        public int CreditCardExpiryMonth { get; set; }

        [Required]
        [DisplayName("Credit Card Expiry Year")]
        public int CreditCardExpiryYear { get; set; }

        [Required]
        [DisplayName("Credit Card CCV")]
        [Range(000, int.MaxValue)]
        public int CreditCardCcv { get; set; }

        [DisplayName("Credit Card Expiry")]
        public string CreditCardExpiry => $"{CreditCardExpiryMonth}/{CreditCardExpiryYear}";
    }
}