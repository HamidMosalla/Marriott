using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using Marriott.Business.Billing;
using Marriott.Business.Billing.Commands;
using Marriott.Business.Guest;
using Marriott.Business.Guest.Commands;
using Marriott.Business.RoomInventory.Commands;
using Marriott.Client.Web.Composers.CheckIn;
using Marriott.Client.Web.Extensions;
using Marriott.Client.Web.Models.CheckIn;
using NServiceBus;
using AllocateRoom = Marriott.Business.RoomInventory.Commands.AllocateRoom;
using CheckIn = Marriott.Client.Web.Models.CheckIn.CheckIn;

namespace Marriott.Client.Web.Controllers
{
    public class CheckInController : Controller
    {
        private const string CheckIn = "checkin";
        public static readonly string ProgressBarText = "Search, Verify Guest Information, Verify Payment Information, Allocate Room, Confirmation";

        private readonly IEndpointInstance endpoint;
        private readonly CheckInComposer composer;

        public CheckInController(IEndpointInstance endpoint, CheckInComposer composer)
        {
            this.endpoint = endpoint;
            this.composer = composer;
            ViewBag.ActiveNavBarItem = NavigationBarItem.FrontDesk;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View(new Search());
        }

        [HttpPost]
        public async Task<ActionResult> Index(Search model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            model = await composer.ComposeSearchResults(model);
            model.SearchExecuted = true;

            return View(model);
        }

        //TODO: is there any way to make this a POST?
        [HttpGet]
        public async Task<ActionResult> VerifyGuestInformation(Guid reservationId)
        {
            var model = await composer.ComposeVerifyGuestInformation(reservationId);

            await endpoint.Send(new AllocateRoom { ReservationId = reservationId, RoomTypeId = model.Reservation.RoomTypeId, CheckIn = model.Reservation.CheckIn, CheckOut = model.Reservation.CheckOut });

            var checkIn = new CheckIn
            {
                ReservationId = reservationId,
                Reservation = model.Reservation,
                RoomTypeName = model.RoomTypeName,
                RoomRate = model.RoomRate,
                TotalRoomRate = model.TotalRoomRate
            };
            Session[CheckIn] = checkIn;

            return View(model);
        }

        //[ChildActionOnly] may only be invoked synchronously: http://stackoverflow.com/questions/24072720/async-partialview-causes-httpserverutility-execute-blocked-exception
        [HttpGet]
        [ChildActionOnly]
        public ActionResult ReservationInformation(Guid reservationId)
        {
            var model = composer.ComposeReservationInformation(reservationId);
            return PartialView("_ReservationInformation", model);
        }

        [HttpGet]
        [ChildActionOnly]
        public ActionResult GuestInformation(Guid reservationId)
        {
            var model = composer.ComposeGuestInformation(reservationId);
            return PartialView("_EditGuestInformation", model);
        }

        [HttpPost]
        public ActionResult VerifyGuestInformation(GuestInformation model)
        {
            if (!ModelState.IsValid)
            {
                return View("VerifyGuestInformation", new VerifyGuestInformation { ReservationId = model.ReservationId });
            }

            endpoint.Send(new SaveGuestInformation { GuestInformation = model });

            var checkIn = (CheckIn)Session[CheckIn];
            checkIn.GuestInformation = model;
            Session[CheckIn] = checkIn;

            return RedirectToAction(nameof(VerifyPaymentInformation), new { reservationId = checkIn.ReservationId });
        }

        [HttpGet]
        public async Task<ActionResult> VerifyPaymentInformation(Guid reservationId)
        {
            var roomAllocationResult = await composer.RoomAllocationResult(reservationId);
            if (roomAllocationResult.Result == Result.Failed)
            {
                //if this happens the hotel cannot provide the guest the room type over the time span we promised the guest in the reservation.
                return RedirectToAction(nameof(RoomAllocationFailed));
            }

            //TODO: need to come up with a plan if the room allocation is still in progress. This is b/c of the asynronicity of the AllocateRoom command dispatched earlier in the controller.
            //we could potentiall poll here/use SignalR until we receive the "RoomAllocationResult" and disable the Next button on the UI until we recieve a response?

            var checkIn = (CheckIn)Session[CheckIn];
            checkIn.RoomNumber = roomAllocationResult.RoomNumber;
            Session[CheckIn] = checkIn;

            var model = await composer.ComposePaymentInformation(reservationId);
                        
            return View(model);
        }

        public ActionResult RoomAllocationFailed()
        {
            //if we get here, it means no more physical rooms of the type on the Reservation are available to be allocated
            //the compensating action for the fda would be to create a new Reservation/change their existing reservation 
            //for a different room type and try to find the guest a room, or to call a partnering hotel and have the guest set up there
            return View("RoomAllocationFailed");
        }

        public async Task<ActionResult> DeallocateRoomForNonPayment(Guid reservationId)
        {
            await endpoint.Send(new DeallocateRoomForNonPaymentAtCheckIn { ReservationId = reservationId });
            return RedirectToAction(nameof(HomeController.Index));
        }

        [HttpPost]
        public async Task<ActionResult> VerifyPaymentInformation(PaymentInformation model)
        {
            //if we get here then PlaceHoldOnCreditCardHub sent PlaceHoldOnCreditCardRequest and it prossed correctly. If it didn't, then the user will be stuck on the VerifyPaymentInfo 
            //GET view until a validate credit card is provided
            if (!ModelState.IsValid)
            {
                return View("VerifyPaymentInformation", model);
            }

            var checkIn = (CheckIn)Session[CheckIn];

            await endpoint.Send(new SavePaymentInformation { PaymentInformation = model });
            await endpoint.Send(new Business.RoomInventory.Commands.CheckIn { ReservationId = checkIn.ReservationId });

            return RedirectToAction(nameof(Confirmation), new { roomNumber = checkIn.RoomNumber });
        }

        [HttpGet]
        public ActionResult Confirmation(int roomNumber)
        {
            return View(new Confirmation { RoomNumber = roomNumber });
        }
    }
}