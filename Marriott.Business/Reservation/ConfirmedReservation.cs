using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Marriott.Business.Reservation
{
    public class ConfirmedReservation
    {
        [Key]
        public Guid ReservationId { get; set; }

        [Index(IsUnique = true)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DisplayName("Reservation Id")]
        public int ExternalId { get; set; }

        public int RoomTypeId { get; set; }

        [DisplayName("Check In")]
        public DateTime CheckIn { get; set; }

        [DisplayName("Check Out")]
        public DateTime CheckOut { get; set; }

        public bool Canceled { get; private set; }

        public void Cancel()
        {
            Canceled = true;
        }

        [NotMapped]
        [DisplayName("Total Nights Of Stay")]
        public int TotalNightsOfStay => Convert.ToInt32((CheckOut.Date - CheckIn.Date).TotalDays) - 1;
    }
}
