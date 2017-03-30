using System.Data.Entity;
using Marriott.Business.Data;

namespace Marriott.Business.Reservation.Data
{
    public class ReservationContext : MarriottContext
    {
        public ReservationContext()
        {
            Database.SetInitializer(new ReservationContextInitializer());
        }

        public DbSet<ConfirmedReservation> ConfirmedReservations { get; set; }
        public DbSet<PendingReservation> PendingReservations { get; set; }
    }

    public class ReservationContextInitializer : DropCreateDatabaseIfModelChanges<ReservationContext>
    {
        //protected override void Seed(ReservationContext context)
        //{
        //}
    }
}
