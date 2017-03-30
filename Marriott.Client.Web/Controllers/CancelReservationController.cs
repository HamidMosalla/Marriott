using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using Marriott.Client.Web.Composers.CancelReservation;
using Marriott.Client.Web.Extensions;
using Marriott.Client.Web.Models.CancelReservation;
using NServiceBus;

namespace Marriott.Client.Web.Controllers
{
    public class CancelReservationController : Controller
    {
        private readonly IEndpointInstance endpoint;
        private readonly CancelReservationComposer composer;

        public CancelReservationController(IEndpointInstance endpoint, CancelReservationComposer composer)
        {
            this.endpoint = endpoint;
            this.composer = composer;
            ViewBag.ActiveNavBarItem = NavigationBarItem.GuestServices;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View(new CancelReservation());
        }

        [HttpPost]
        public async Task<ActionResult> Index(CancelReservation model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var reservationId = await composer.ComposeCancelReservation(model.ExternalId);

            await endpoint.Send(new Business.Reservation.Commands.CancelReservation { ReservationId = reservationId, CancelationDateTime = DateTime.Now });

            return RedirectToAction(nameof(CancelReservationController.Index));
        }
    }
}