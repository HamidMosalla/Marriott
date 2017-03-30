using System.Data.Entity;
using Marriott.Business.Data;

namespace Marriott.Business.Billing.Data
{
    public class BillingContext : MarriottContext
    {
        public BillingContext()
        {
            Database.SetInitializer(new BillingContextInitializer());
        }

        public DbSet<PaymentInformation> PaymentInformation { get; set; }
        public DbSet<CreditCardHold> CreditCardHold { get; set; }
    }

    public class BillingContextInitializer : DropCreateDatabaseIfModelChanges<BillingContext>
    {
        //protected override void Seed(BillingContext context)
        //{
        //}
    }
}
