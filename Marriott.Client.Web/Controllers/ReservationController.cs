using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Marriott.Business.Billing;
using Marriott.Business.Billing.Commands;
using Marriott.Business.Guest;
using Marriott.Business.Guest.Commands;
using Marriott.Business.Pricing;
using Marriott.Business.Pricing.Commands;
using Marriott.Business.Reservation.Commands;
using Marriott.Client.Web.Composers.Reservation;
using Marriott.Client.Web.Extensions;
using Marriott.Client.Web.Models.Reservation;
using Marriott.ITOps.PaymentGateway;
using NServiceBus;
using Reservation = Marriott.Client.Web.Models.Reservation.Reservation;

namespace Marriott.Client.Web.Controllers
{
    public class ReservationController : Controller
    {
        private const string Reservation = "reservation";
        private const string TimeLeftOnPendingReservation = "TimeLeftOnPendingReservation";
        public static readonly string ProgressBarText = "Search, Guest Information, Payment Information, Review, Confirmation";

        private readonly IEndpointInstance endpoint;
        private readonly ReservationComposer composer;
        public Func<Guid> CreateReservationId = () => Guid.NewGuid();

        public ReservationController(IEndpointInstance endpoint, ICreditCardService creditCardService, ReservationComposer composer)
        {
            this.endpoint = endpoint;
            this.composer = composer;
            ViewBag.ActiveNavBarItem = NavigationBarItem.Reservations;
        }

        [HttpGet]
        public ActionResult Search()
        {
            return View(new Search());
        }

        [HttpPost]
        public async Task<ActionResult> Search(Search model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.CheckOut.Date <= model.CheckIn.Date)
            {
                model.CheckOutDateIsTheSameDayAsOrBeforeCheckInDate = true;
                return View(model);
            }

            model = await composer.ComposeSearch(model);
            model.SearchExecuted = true;

            //Why am I doing this?
            //I'm showing the user room rates based on their search results. When the user picks a room and the page posts back, if I were to re-compose room rate information and it's 
            //different than what the user was shown on the search resuls, that's bad. By committing all possible room rate information to session for each search result returned, I can 
            //guarantee that price to the user even if rates were to change between the point they viewed the search results, picked a room and confirmed their reservation
            var reservation = new Reservation { CheckIn = model.CheckIn, CheckOut = model.CheckOut };
            foreach (var result in model.Results)
            {
                reservation.RoomTypeIdRoomTotalAndTotalRoomRates.Add(new RoomTypeIdRoomTotalAndTotalRoomRate { RoomTypeId = result.RoomTypeId, RoomRate = result.RoomRate, TotalRoomRate = result.TotalRoomRate });
            }
            Session[Reservation] = reservation;

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> SelectRoom(int roomTypeId)
        {
            var reservationId = CreateReservationId();

            var reservation = (Reservation)Session[Reservation];

            var selectedRoomRateAndTotalRoomRate = reservation.RoomTypeIdRoomTotalAndTotalRoomRates.Single(x => x.RoomTypeId == roomTypeId);

            reservation.ReservationId = reservationId;
            reservation.RoomTypeId = roomTypeId;
            reservation.RoomInformation = await composer.ComposeRoomInformationFor(roomTypeId, selectedRoomRateAndTotalRoomRate.RoomRate, selectedRoomRateAndTotalRoomRate.TotalRoomRate);
            Session[Reservation] = reservation;

            //Reservation Pattern (http://arnon.me/soa-patterns/reservation/)
            await endpoint.Send(new CreatePendingReservation { ReservationId = reservationId, RoomTypeId = roomTypeId, CheckIn = reservation.CheckIn, CheckOut = reservation.CheckOut });
            await endpoint.Send(new SaveReservationRoomRate { ReservationRoomRate = new ReservationRoomRate
            {
                ReservationId = reservationId,
                RoomRate = selectedRoomRateAndTotalRoomRate.RoomRate,
                TotalRoomRate = selectedRoomRateAndTotalRoomRate.TotalRoomRate
            }});

            return RedirectToAction(nameof(GuestInformation), new { roomTypeId });
        }

        [HttpGet]
        public ActionResult GuestInformation(int roomTypeId)
        {
            var reservation = (Reservation)Session[Reservation];
            reservation.TimeLeftOnPendingReservation = 900;
            Session[Reservation] = reservation;

            ViewBag.TimeLeftOnPendingReservation = reservation.TimeLeftOnPendingReservation;
            return View(new GuestInformation());
        }

        [HttpPost]
        public async Task<ActionResult> GuestInformation(GuestInformation model)
        {
            UpdateTimeLeftOnPendingReservationFromRequestFormToToSession();

            if (!ModelState.IsValid)
            {
                AssignTimeLeftOnPendingReservationFromRequestFormToViewBag();
                return View(model);
            }

            var reservation = (Reservation)Session[Reservation];
            model.ReservationId = reservation.ReservationId;
            reservation.GuestInformation = model;
            Session[Reservation] = reservation;

            await endpoint.Send(new SaveGuestInformation { GuestInformation = model });

            return RedirectToAction(nameof(Payment));
        }

        //http://www.marriott.com/help/reserve-faqs.mi
        //Why do I need a credit card number to reserve a room?
        //Marriott.com requires all room reservations to be guaranteed by a valid credit card. Your credit card is not charged at the time of reservation, and is used as a guarantee 
        //that a room will be held in advance of your arrival. Your credit card will be charged only if pre-payment is required for a specific rate, or in the event that you do not 
        //cancel your reservation within the stated cancellation time period and do not check in.
        [HttpGet]
        public ActionResult Payment()
        {
            AssignTimeLeftOnPendingReservationFromSessionToViewBag();
            return View(new PaymentInformation());
        }

        [HttpPost]
        public async Task<ActionResult> Payment(PaymentInformation model)
        {
            UpdateTimeLeftOnPendingReservationFromRequestFormToToSession();

            if (!ModelState.IsValid)
            {
                AssignTimeLeftOnPendingReservationFromRequestFormToViewBag();
                return View(model);
            }

            var reservation = (Reservation)Session[Reservation];

            model.ReservationId = reservation.ReservationId;
            await endpoint.Send(new SavePaymentInformation { PaymentInformation = model });

            reservation.PaymentInformation = model;
            Session[Reservation] = reservation;

            return RedirectToAction(nameof(Review));
        }

        [HttpGet]
        public ActionResult Review()
        {
            AssignTimeLeftOnPendingReservationFromSessionToViewBag();
            return View((Reservation) Session[Reservation]);
        }

        [HttpPost]
        public async Task<ActionResult> Reserve(Reservation model)
        {
            var reservation = (Reservation)Session[Reservation];

            //I don't need to make this check anymore, b/c PendingReservation offers a limited, time-based lock on the "resource", which here, is a room type over a given timespan
            //if (!await composer.IsRoomStillAvailableFor(reservation.CheckIn, reservation.CheckOut, reservation.RoomTypeId))
            //{
            //    return View("RoomNoLongerAvailable", (Reservation) Session[Reservation]);
            //}

            await endpoint.Send(new ConfirmPendingReservation { ReservationId = reservation.ReservationId });

            //clean up session
            Session.Remove(Reservation);

            TempData[Reservation] = reservation;
            return RedirectToAction(nameof(Confirmation));
        }

        [HttpGet]
        public ActionResult Confirmation()
        {
            return View((Reservation)TempData[Reservation]);
        }

        [HttpGet]
        public async Task<ActionResult>PendingReservationTimeOut()
        {
            var reservation = (Reservation)Session[Reservation];
            await endpoint.Send(new CancelPendingReservation { ReservationId = reservation.ReservationId });
            Session.Remove(Reservation);

            return RedirectToAction("Search");
        }

        private void UpdateTimeLeftOnPendingReservationFromRequestFormToToSession()
        {
            var reservation = (Reservation)Session[Reservation];
            reservation.TimeLeftOnPendingReservation = Convert.ToInt32(Request.Form[TimeLeftOnPendingReservation]);
            Session[Reservation] = reservation;
        }

        private void AssignTimeLeftOnPendingReservationFromSessionToViewBag()
        {
            var reservation = (Reservation)Session [Reservation];
            ViewBag.TimeLeftOnPendingReservation = reservation.TimeLeftOnPendingReservation;
        }

        private void AssignTimeLeftOnPendingReservationFromRequestFormToViewBag()
        {
            ViewBag.TimeLeftOnPendingReservation = Convert.ToInt32(Request.Form[TimeLeftOnPendingReservation]);
        }
    }
}