using System.Data.Entity;
using Marriott.Business.Data;

namespace Marriott.Business.Housekeeping.Data
{
    public class HousekeepingContext : MarriottContext
    {
        public HousekeepingContext()
        {
            Database.SetInitializer(new HousekeepingContextInitializer());
        }

        public DbSet<TodaysCleanRooms> TodaysCleanRooms { get; set; }
        public DbSet<TodaysCheckedOutRooms> TodaysCheckedOutRooms { get; set; }
    }

    public class HousekeepingContextInitializer : DropCreateDatabaseIfModelChanges<HousekeepingContext>
    {
        //protected override void Seed(HousekeepingContext context)
        //{
        //}
    }
}
