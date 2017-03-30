using System.Data.Entity;
using Marriott.Business.Data;

namespace Marriott.Business.Pricing.Data
{
    public class PricingContext : MarriottContext
    {
        public PricingContext()
        {
            Database.SetInitializer(new PricingContextInitializer());
        }

        public DbSet<RoomRate> RoomRate { get; set; }
        public DbSet<ReservationRoomRate> ReservationRoomRate { get; set; }

        public static void Seed(PricingContext context)
        {
            context.RoomRate.Add(new RoomRate { RoomTypeId = 1, Rate = 100.00 });
            context.RoomRate.Add(new RoomRate { RoomTypeId = 2, Rate = 150.00 });
            context.RoomRate.Add(new RoomRate { RoomTypeId = 3, Rate = 200.00 });
            context.SaveChanges();
        }
    }

    public class PricingContextInitializer : DropCreateDatabaseIfModelChanges<PricingContext>
    {
        //protected override void Seed(BillingContext context)
        //{
        //}
    }
}
