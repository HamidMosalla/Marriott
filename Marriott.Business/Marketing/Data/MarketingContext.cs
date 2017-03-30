using System.Data.Entity;
using Marriott.Business.Data;

namespace Marriott.Business.Marketing.Data
{
    public class MarketingContext : MarriottContext
    {
        public MarketingContext()
        {
            Database.SetInitializer(new MarketingContextInitializer());
        }

        public DbSet<RoomType> RoomTypes { get; set; }

        public static void Seed(MarketingContext context)
        {
            context.RoomTypes.Add(new RoomType { RoomTypeId = 1, Description = "a double room", ImageUrl = "http://cache.marriott.com/propertyimages/s/sfocn/phototour/sfocn_phototour21_s.jpg" });
            context.RoomTypes.Add(new RoomType { RoomTypeId = 2, Description = "a queen room", ImageUrl = "http://cache.marriott.com/propertyimages/s/sfocn/phototour/sfocn_phototour23_s.jpg" });
            context.RoomTypes.Add(new RoomType { RoomTypeId = 3, Description = "a king room", ImageUrl = "http://cache.marriott.com/propertyimages/s/sfocn/phototour/sfocn_phototour25_s.jpg" });
            context.SaveChanges();
        }
    }

    public class MarketingContextInitializer : DropCreateDatabaseIfModelChanges<MarketingContext>
    {
        //used to seed here, but b/c of multiple DbContext's (one per schema), EnsureTablesAreCreatedWhenConfiguringEndpoint coordinates the physical db creation and then the seeding of any contexts that need to be seeded
    }
}
