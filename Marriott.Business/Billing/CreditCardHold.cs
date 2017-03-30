using System;
using System.ComponentModel.DataAnnotations;

namespace Marriott.Business.Billing
{
    public class CreditCardHold
    {
        [Key]
        public Guid ReservationdId { get; set; }
        public double HoldAmount { get; set; }
        public bool Released { get; private set; }

        public void Release()
        {
            Released = true;
        }
    }
}
