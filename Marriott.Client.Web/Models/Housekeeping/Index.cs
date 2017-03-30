using System;
using System.Collections.Generic;

namespace Marriott.Client.Web.Models.Housekeeping
{
    public class Index
    {
        public List<Room> Rooms { get; set; } = new List<Room>();
    }

    public class Room
    {
        public int RoomNumber { get; set; }
        public bool Clean { get; set; }
        public DateTime DateTimeCheckedOut { get; set; }
    }
}