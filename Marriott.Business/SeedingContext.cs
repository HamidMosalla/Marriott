using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using Marriott.Business.Data;

namespace Marriott.Business
{
    public class SeedingContext : MarriottContext
    {
        public SeedingContext()
        {
            Database.SetInitializer(new SeedingContextInitializer());
        }

        public DbSet<SiteSeeded> SiteSeeded { get; set; }
    }

    public class SeedingContextInitializer : DropCreateDatabaseIfModelChanges<SeedingContext>
    {
    }

    public class SiteSeeded
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int SiteSeededId { get; set; }
    }
}
