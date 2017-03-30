using System.Web.Mvc;
using Marriott.Client.Web.Extensions;

namespace Marriott.Client.Web.Controllers
{
    public class GuestServicesController : Controller
    {
        public GuestServicesController()
        {
            ViewBag.ActiveNavBarItem = NavigationBarItem.GuestServices;
        }

        public ActionResult Index()
        {
            return View();
        }
    }
}