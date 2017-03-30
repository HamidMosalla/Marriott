using System.Threading.Tasks;
using System.Web.Mvc;
using Marriott.Business.Marketing;
using Marriott.Business.Marketing.Commands;
using Marriott.Client.Web.Composers.Marketing;
using NServiceBus;

namespace Marriott.Client.Web.Controllers
{
    public class MarketingController : Controller
    {
        private readonly IEndpointInstance endpoint;
        private readonly MarketingComposer composer;

        public MarketingController(IEndpointInstance endpoint, MarketingComposer composer)
        {
            this.endpoint = endpoint;
            this.composer = composer;
        }

        public async Task<ActionResult> Index()
        {
            var model = await composer.ComposeIndex();
            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> Edit(int roomTypeId)
        {
            var model = await composer.ComposeEdit(roomTypeId);
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(RoomType roomType)
        {
            await endpoint.Send(new SaveRoomType { RoomType = roomType });
            return RedirectToAction(nameof(Index));
        }
    }
}