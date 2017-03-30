using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Marriott.Business.Data
{
    public abstract class MarriottContext : DbContext
    {
        protected MarriottContext()
        {
            Database.Connection.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB; Initial Catalog=Marriott; Integrated Security=True;";
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.HasDefaultSchema(GetType().Name.Replace("Context", ""));
        }
    }

    public class ContextInitializerBase<TContext> : DropCreateDatabaseIfModelChanges<TContext> where TContext : DbContext
    {
    }
}
