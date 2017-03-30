using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Marriott.Client.Web.Models.CheckIn
{
    public class Search
    {
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        public List<SearchResult> Results { get; set; } = new List<SearchResult>();

        public bool SearchExecuted { get; set; }
    }

    public class SearchResult
    {
        public Guid ReservationId { get; set; }
        public int ExternalId { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public string FullName { get; set; }
        public string RoomType { get; set; }
    }
}