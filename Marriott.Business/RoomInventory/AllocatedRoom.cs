using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Marriott.Business.RoomInventory
{
    public class AllocatedRoom
    {
        [Key]
        public Guid ReservationId { get; set; }

        [Required]
        [DisplayName("Room Number")]
        public int RoomNumber { get; set; }

        [Required]
        [DisplayName("Check In")]
        public DateTime CheckInDate { get; set; }

        [Required]
        [DisplayName("Check Out")]
        public DateTime CheckOutDate { get; set; }

        public bool Deallocated { get; private set; }

        public void Deallocate()
        {
            Deallocated = true;
        }
    }
}