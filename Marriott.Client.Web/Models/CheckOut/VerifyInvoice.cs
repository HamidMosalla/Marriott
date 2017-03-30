using System;
using System.ComponentModel;
using Marriott.Business.Billing;
using Marriott.Business.Guest;

namespace Marriott.Client.Web.Models.CheckOut
{
    //http://l.yimg.com/a/p/sp/tools/med/2010/09/ipt/1285725603.jpg
    public class VerifyInvoice
    {
        public const double RoomTaxRate = .08;

        public Guid ReservationId { get; set; }

        [DisplayName("Room #")]
        public int RoomNumber { get; set; }

        public GuestInformation GuestInformation { get; set; }

        public PaymentInformation PaymentInformation { get; set; }

        [DisplayName("Room Type")]
        public string RoomType { get; set; }

        [DisplayName("Check In")]
        public DateTime CheckIn { get; set; }

        [DisplayName("Check Out")]
        public DateTime CheckOut { get; set; }

        [DisplayName("Rate")]
        public double RoomRate { get; set; }

        [DisplayName("Room Total")]
        public double TotalRoomRate { get; set; }

        public double Tax { get; set; }

        public double Total { get; set; }

        //public double Tax => TotalRoomRate * RoomTaxRate;
        //public double Total => TotalRoomRate + Tax;
    }
}