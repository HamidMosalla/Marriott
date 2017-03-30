using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Marriott.Client.Web.Models.CancelReservation
{
    public class CancelReservation
    {
        [Required]
        [DisplayName("Reservation Id")]
        public int ExternalId { get; set; }
    }
}