using System.Data.Entity;
using Marriott.Business.Data;

namespace Marriott.Business.Guest.Data
{
    public class GuestContext : MarriottContext
    {
        public GuestContext()
        {
            Database.SetInitializer(new GuestContextContextInitializer());
        }

        public DbSet<GuestInformation> GuestInformation { get; set; }
    }

    public class GuestContextContextInitializer : DropCreateDatabaseIfModelChanges<GuestContext>
    {
        //protected override void Seed(GuestContext context)
        //{
        //}
    }
}
