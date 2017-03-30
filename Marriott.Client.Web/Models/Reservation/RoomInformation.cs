using System.ComponentModel;

namespace Marriott.Client.Web.Models.Reservation
{
    public class RoomInformation
    {
        [DisplayName("Room Type")]
        public string RoomTypeName { get; set; }
        
        [DisplayName("Room Rate")]
        public double RoomRate { get; set; }

        [DisplayName("Total Room Rate")]
        public double TotalRoomRate { get; set; }
    }
}