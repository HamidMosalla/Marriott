using System.Data.Entity;
using Marriott.Business.Data;

namespace Marriott.Business.RoomInventory.Data
{
    public class RoomInventoryContext : MarriottContext
    {
        public RoomInventoryContext()
        {
            Database.SetInitializer(new RoomInventoryContextInitializer());
        }

        public DbSet<RoomNumber> RoomNumbers { get; set; }
        public DbSet<RoomType> RoomTypes { get; set; }
        public DbSet<AllocatedRoom> AllocatedRooms { get; set; }
        public DbSet<FailedRoomAllocation> FailedRoomAllocations { get; set; }

        public static void Seed(RoomInventoryContext context)
        {
            context.RoomTypes.Add(new RoomType { Id = 1, Name = "Double" });
            context.RoomTypes.Add(new RoomType { Id = 2, Name = "Queen" });
            context.RoomTypes.Add(new RoomType { Id = 3, Name = "King" });

            context.RoomNumbers.Add(new RoomNumber { Number = 100, RoomTypeId = 1 });
            context.RoomNumbers.Add(new RoomNumber { Number = 101, RoomTypeId = 1 });
            context.RoomNumbers.Add(new RoomNumber { Number = 102, RoomTypeId = 1 });
            context.RoomNumbers.Add(new RoomNumber { Number = 103, RoomTypeId = 1 });
            context.RoomNumbers.Add(new RoomNumber { Number = 104, RoomTypeId = 1 });

            context.RoomNumbers.Add(new RoomNumber { Number = 200, RoomTypeId = 2 });
            context.RoomNumbers.Add(new RoomNumber { Number = 201, RoomTypeId = 2 });
            context.RoomNumbers.Add(new RoomNumber { Number = 202, RoomTypeId = 2 });
            context.RoomNumbers.Add(new RoomNumber { Number = 203, RoomTypeId = 2 });
            context.RoomNumbers.Add(new RoomNumber { Number = 204, RoomTypeId = 2 });

            context.RoomNumbers.Add(new RoomNumber { Number = 300, RoomTypeId = 3 });
            context.RoomNumbers.Add(new RoomNumber { Number = 301, RoomTypeId = 3 });
            context.RoomNumbers.Add(new RoomNumber { Number = 302, RoomTypeId = 3 });
            context.RoomNumbers.Add(new RoomNumber { Number = 303, RoomTypeId = 3 });
            context.RoomNumbers.Add(new RoomNumber { Number = 304, RoomTypeId = 3 });

            context.SaveChanges();
        }
    }

    public class RoomInventoryContextInitializer : DropCreateDatabaseIfModelChanges<RoomInventoryContext>
    {
    }
}
