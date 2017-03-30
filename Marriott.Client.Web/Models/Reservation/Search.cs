using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Marriott.Client.Web.Models.Reservation
{
    public class Search
    {
        [Required]
        [DisplayName("Check In")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        public DateTime CheckIn { get; set; } = DateTime.Now.Date;

        [Required]
        [DisplayName("Check Out")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        public DateTime CheckOut { get; set; } = DateTime.Now.AddDays(5).Date;

        public List<SearchResult> Results { get; set; } = new List<SearchResult>();

        public bool SearchExecuted { get; set; }
        public bool CheckOutDateIsTheSameDayAsOrBeforeCheckInDate { get; set; }
    }

    public class SearchResult
    {
        [DisplayName("Room Type")]
        public int RoomTypeId { get; set; }

        [DisplayName("Room Type")]
        public string RoomTypeName { get; set; }

        public string ImageUrl { get; set; }

        [DisplayName("Description")]
        public string RoomDescription { get; set; }

        [DisplayName("Rate")]
        public double RoomRate { get; set; }

        public double TotalRoomRate { get; set; }
    }
}