using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using Marriott.Business.RoomInventory.Commands;
using Marriott.Client.Web.Composers.CheckOut;
using Marriott.Client.Web.Extensions;
using Marriott.Client.Web.Models.CheckOut;
using NServiceBus;

namespace Marriott.Client.Web.Controllers
{
    public class CheckOutController : Controller
    {
        public static readonly string ProgressBarText = "Search, Verify Invoice and Payment Method, Confirmation";

        private readonly IEndpointInstance endpoint;
        private readonly CheckOutComposer composer;

        public CheckOutController(IEndpointInstance endpoint, CheckOutComposer composer)
        {
            this.endpoint = endpoint;
            this.composer = composer;
            ViewBag.ActiveNavBarItem = NavigationBarItem.FrontDesk;
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

            model = await composer.ComposeSearchResults(model);
            model.SearchExecuted = true;

            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> VerifyInvoice(Guid reservationId)
        {
            var model = await composer.ComposeVerifyInvoice(reservationId);
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> VerifyInvoice(VerifyInvoice model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await endpoint.Send(new CheckOut { ReservationId = model.ReservationId, CheckOutDate = DateTime.Now });

            return RedirectToAction(nameof(Confirmation));
        }

        [HttpGet]
        public ActionResult Confirmation()
        {
            return View();
        }
    }
}