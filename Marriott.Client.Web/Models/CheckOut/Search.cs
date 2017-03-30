using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Marriott.Client.Web.Models.CheckOut
{
    public class Search
    {
        [Required]
        [DisplayName("Room Number")]
        public int RoomNumber { get; set; }
        public SearchResult Result { get; set; }
        public bool SearchExecuted { get; set; }
    }

    public class SearchResult
    {
        public Guid ReservationId { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        [DisplayName("Guest Name")]
        public string GuestFullName { get; set; }
    }
}