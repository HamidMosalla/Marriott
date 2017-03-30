using System;
using System.Linq;
using System.Web.Mvc;
using Marriott.Client.Web.Composers.Housekeeping;
using Marriott.Client.Web.Models.Housekeeping;
using NServiceBus;
using System.Threading.Tasks;
using Marriott.Business.Housekeeping;
using Marriott.Business.Housekeeping.Commands;
using Marriott.Client.Web.Extensions;

namespace Marriott.Client.Web.Controllers
{
    public class HousekeepingController : Controller
    {
        private readonly IEndpointInstance endpoint;
        private readonly HousekeepingComposer composer;

        public HousekeepingController(IEndpointInstance endpoint, HousekeepingComposer composer)
        {
            this.endpoint = endpoint;
            this.composer = composer;
            ViewBag.ActiveNavBarItem = NavigationBarItem.Housekeeping;
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var model = await composer.ComposeRoomsThatNeedToBeCleanedToday();
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Index(Index model)
        {
            var todaysCleanRooms = model.Rooms.Where(x => x.Clean).Select(x => new TodaysCleanRooms
            {
                RoomNumber = x.RoomNumber,
                DayCleaned = DateTime.Now.Date
            }).ToList();

            await endpoint.Send(new UpdateTodaysCleanRooms { TodaysCleanRooms = todaysCleanRooms });

            return View("Index", model);
        }
    }
}